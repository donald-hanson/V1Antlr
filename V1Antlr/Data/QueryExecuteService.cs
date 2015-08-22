using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using V1Antlr.Meta;

namespace V1Antlr.Data
{
    public class QueryExecuteService
    {
        private static ModuleBuilder _moduleBuilder;
        private static ModuleBuilder ModuleBuilder
        {
            get
            {
                if (_moduleBuilder == null)
                {
                    var dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("MyDynamicAssembly"), AssemblyBuilderAccess.Run);
                    _moduleBuilder = dynamicAssembly.DefineDynamicModule("MyDynamicAssemblyModule");
                }
                return _moduleBuilder;
            }
        }

        public QueryResult Execute(IQueryable queryable, Query query)
        {
            queryable = ApplyFilter(queryable, query);

            var totalAvailable = DetermineTotalAvailable(queryable);

            queryable = ApplySorting(queryable, query);
            queryable = ApplyPaging(queryable, query);
            queryable = ApplySelection(queryable, query);

            var assets = InternalExecute(queryable, query);

            return new QueryResult(query, assets, totalAvailable);
        }

        private static IEnumerable<Asset> InternalExecute(IQueryable queryable, Query query)
        {
            List<Asset> assets = new List<Asset>();

            var selectionFieldMapping = GetSelectionMapping(query.Selection);

            var fieldMap = queryable.ElementType.GetFields().ToDictionary(x => x.Name);

            foreach (var obj in queryable)
            {
                Asset asset = new Asset(query.AssetType);

                foreach (var kvp in selectionFieldMapping)
                {
                    var name = kvp.Key;
                    var attributeDefinition = kvp.Value;

                    var field = fieldMap[name];
                    var value = field.GetValue(obj);

                    asset.LoadAttribute(attributeDefinition, value);
                }

                assets.Add(asset);
            }

            return assets;
        }

        private static long DetermineTotalAvailable(IQueryable queryable)
        {
            var call = Expression.Call(typeof(Queryable), "LongCount", new[] { queryable.ElementType }, queryable.Expression);
            return queryable.Provider.Execute<long>(call);
        }

        private static IQueryable ApplyPaging(IQueryable queryable, Query query)
        {
            if (query.Skip.HasValue)
            {
                var skip = Expression.Constant(query.Skip.Value);
                var call = Expression.Call(typeof(Queryable), "Skip", new[] { queryable.ElementType }, queryable.Expression, skip);
                queryable = queryable.Provider.CreateQuery(call);
            }

            if (query.Take.HasValue)
            {
                var take = Expression.Constant(query.Take.Value);
                var call = Expression.Call(typeof(Queryable), "Take", new[] { queryable.ElementType }, queryable.Expression, take);
                queryable = queryable.Provider.CreateQuery(call);
            }

            return queryable;
        }

        private static IDictionary<string, AttributeDefinition> GetSelectionMapping(IEnumerable<AttributeDefinition> selection)
        {
            return selection.Select((a, i) => new { a, i }).ToDictionary(x => $"v{x.i}", x => x.a);
        }

        private static IQueryable ApplySelection(IQueryable queryable, Query query)
        {
            var typeName = $"t{Guid.NewGuid().ToString("N")}";
            var typeBuilder = ModuleBuilder.DefineType(typeName);

            var parameter = Expression.Parameter(queryable.ElementType);

            var selectionFieldMapping = GetSelectionMapping(query.Selection);

            IDictionary<string, Expression> mapping = new Dictionary<string, Expression>();

            foreach (var kvp in selectionFieldMapping)
            {
                var name = kvp.Key;
                var attributeDefinition = kvp.Value;

                var expression = attributeDefinition.CreateExpression(parameter);
                mapping.Add(name, expression);

                var fieldType = expression.Type;
                typeBuilder.DefineField(name, fieldType, FieldAttributes.Public);
            }

            var generatedType = typeBuilder.CreateType();

            List<MemberAssignment> bindings = new List<MemberAssignment>();

            foreach (var kvp in mapping)
            {
                var propertyName = kvp.Key;
                var rightExpression = kvp.Value;

                var targetField = generatedType.GetField(propertyName);
                var binding = Expression.Bind(targetField, rightExpression);
                bindings.Add(binding);
            }

            var newContext = Expression.New(generatedType);
            var memberInit = Expression.MemberInit(newContext, bindings);

            var select = Expression.Lambda(memberInit, parameter);

            var call = Expression.Call(typeof(Queryable), "Select", new[] { queryable.ElementType, @select.ReturnType }, queryable.Expression, @select);

            return queryable.Provider.CreateQuery(call);
        }

        private static IQueryable ApplySorting(IQueryable queryable, Query query)
        {
            bool first = true;
            foreach (var term in query.OrderTerms)
            {
                string methodName = null;
                if (first)
                    methodName = term.Direction == OrderTermDirection.Ascending ? "OrderBy" : "OrderByDescending";
                else
                    methodName = term.Direction == OrderTermDirection.Ascending ? "ThenBy" : "ThenByDescending";
                first = false;

                var parameter = Expression.Parameter(queryable.ElementType);
                var expression = term.AttributeDefinition.CreateExpression(parameter);
                var lambda = Expression.Lambda(expression, parameter);
                var ordercall = Expression.Call(typeof(Queryable), methodName, new [] { queryable.ElementType, lambda.ReturnType }, queryable.Expression, lambda);
                queryable = queryable.Provider.CreateQuery(ordercall);
            }
            return queryable;
        }

        private static IQueryable ApplyFilter(IQueryable queryable, Query query)
        {
            var parameter = Expression.Parameter(queryable.ElementType);
            var expression = CreateFilterExpression(query.Filter, parameter);
            if (expression == null)
                return queryable;
            var lambda = Expression.Lambda(expression, parameter);
            var ordercall = Expression.Call(typeof(Queryable), "Where", new[] { queryable.ElementType }, queryable.Expression, lambda);
            return queryable.Provider.CreateQuery(ordercall);
        }

        private static BinaryExpression CreateFilterExpression(FilterTerm term, Expression parameter)
        {
            if (term.Type == FilterTermType.And)
            {
                BinaryExpression expression = null;
                foreach (var innerTerm in term.Terms)
                {
                    var innerExpression = CreateFilterExpression(innerTerm, parameter);
                    expression = expression == null ? innerExpression : Expression.AndAlso(expression, innerExpression);
                }
                return expression;
            }

            if (term.Type == FilterTermType.Or)
            {
                BinaryExpression expression = null;
                foreach (var innerTerm in term.Terms)
                {
                    var innerExpression = CreateFilterExpression(innerTerm, parameter);
                    expression = expression == null ? innerExpression : Expression.OrElse(expression, innerExpression);
                }
                return expression;
            }

            var fieldFilterTerm = (FieldFilterTerm) term;
            return fieldFilterTerm.AttributeDefinition.CreateFilterExpression(fieldFilterTerm.Operator, fieldFilterTerm.Values, parameter);
        }
    }
}

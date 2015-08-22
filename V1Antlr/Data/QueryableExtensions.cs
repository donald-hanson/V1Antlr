using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace V1Antlr.Data
{
    public static class QueryableExtensions
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

        private static void AddProperty(this TypeBuilder typeBuilder, string name, Type type)
        {
            FieldBuilder field = typeBuilder.DefineField("_" + name, type, FieldAttributes.Private);
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.None, type, null);

            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName;

            MethodBuilder getter = typeBuilder.DefineMethod("get_" + name, getSetAttr, type, Type.EmptyTypes);

            ILGenerator getIL = getter.GetILGenerator();
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, field);
            getIL.Emit(OpCodes.Ret);

            MethodBuilder setter = typeBuilder.DefineMethod("set_" + name, getSetAttr, null, new Type[] { type });

            ILGenerator setIL = setter.GetILGenerator();
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, field);
            setIL.Emit(OpCodes.Ret);


            propertyBuilder.SetGetMethod(getter);
            propertyBuilder.SetSetMethod(setter);
        }


        public static QueryResult ApplyQuery(this IQueryable queryable, Query query)
        {
            queryable = ApplyPaging(queryable, query);
            queryable = ApplySelection(queryable, query);

            return new QueryResult(query, Enumerable.Empty<Asset>(), 0);
        }

        private static IQueryable ApplyPaging(IQueryable queryable, Query query)
        {
            if (query.Skip.HasValue)
            {
                var skip = Expression.Constant(query.Skip.Value);
                var call = Expression.Call(typeof (Queryable), "Skip", new[] {queryable.ElementType}, queryable.Expression, skip);
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

        private static IQueryable ApplySelection(IQueryable queryable, Query query)
        {
            var typeName = $"t{Guid.NewGuid().ToString("N")}";
            var typeBuilder = ModuleBuilder.DefineType(typeName);

            var parameter = Expression.Parameter(queryable.ElementType);

            IDictionary<string, Expression> mapping = new Dictionary<string, Expression>();

            int i = 0;
            foreach (var attributeDefinition in query.Selection)
            {
                var name = $"v{i}";
                i++;

                var expression = attributeDefinition.CreateExpression(parameter);
                mapping.Add(name, expression);

                var propertyType = expression.Type;
                typeBuilder.AddProperty(name, propertyType);
            }

            var generatedType = typeBuilder.CreateType();

            List<MemberAssignment> bindings = new List<MemberAssignment>();

            foreach (var kvp in mapping)
            {
                var propertyName = kvp.Key;
                var rightExpression = kvp.Value;

                var targetProperty = generatedType.GetProperty(propertyName);
                var binding = Expression.Bind(targetProperty, rightExpression);
                bindings.Add(binding);
            }

            var newContext = Expression.New(generatedType);
            var memberInit = Expression.MemberInit(newContext, bindings);

            var select = Expression.Lambda(memberInit, parameter);

            var call = Expression.Call(typeof (Queryable), "Select", new[] {queryable.ElementType, @select.ReturnType}, queryable.Expression, @select);

            return queryable.Provider.CreateQuery(call);
        }
    }

    public class QueryResult
    {
        public readonly Query Query;
        public readonly IEnumerable<Asset> Assets;
        public readonly ulong TotalAvailable;

        public QueryResult(Query query, IEnumerable<Asset> assets, ulong totalAvailable)
        {
            Query = query;
            Assets = assets;
            TotalAvailable = totalAvailable;
        }
    }

    public class Asset
    {
        
    }
}

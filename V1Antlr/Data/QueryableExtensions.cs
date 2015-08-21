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

        private static void AddProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig;

            FieldBuilder field = typeBuilder.DefineField("_" + propertyName, typeof(string), FieldAttributes.Private);
            PropertyBuilder property = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, new[] { propertyType });

            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_value", getSetAttr, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getMethodBuilder.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, field);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_value", getSetAttr, null, new[] { propertyType });
            ILGenerator setIl = setMethodBuilder.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, field);
            setIl.Emit(OpCodes.Ret);

            property.SetGetMethod(getMethodBuilder);
            property.SetSetMethod(setMethodBuilder);
        }


        public static QueryResult ApplyQuery(this IQueryable queryable, Query query)
        {
            //var generatedType = typeof (ExpandoObject);
            

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

            return new QueryResult(query, Enumerable.Empty<Asset>(), 0);
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

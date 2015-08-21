using System;
using System.Linq.Expressions;
using System.Reflection;
using V1Antlr.Extensions;

namespace V1Antlr.Meta
{
    public class PrimitveAttributeDefinition : AttributeDefinition
    {
        private readonly Type _ownedType;
        private readonly PropertyInfo _property;

        public PrimitveAttributeDefinition(Type ownedType, PropertyInfo property, AssetType assetType, bool nullable) 
            :base(assetType, property.Name, nullable, null, false)
        {
            _ownedType = ownedType;
            _property = property;
        }

        internal override bool IsNumeric => _property.PropertyType.IsNumeric();

        internal override Expression CreateExpression(Expression parameter)
        {
            return Expression.Property(parameter, _property);
        }
    }
}
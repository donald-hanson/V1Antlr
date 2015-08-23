using System;
using System.Linq.Expressions;
using System.Reflection;

namespace V1Antlr.Meta
{
    public class SingleRelationAttributeDefinition : AttributeDefinition
    {
        private readonly Type _ownedType;
        private readonly PropertyInfo _property;

        public SingleRelationAttributeDefinition(Type ownedType, PropertyInfo property, AssetType assetType, bool nullable, AssetType relatedAssetType)
            : base(assetType, property.Name, nullable, relatedAssetType, false)
        {
            _ownedType = ownedType;
            _property = property;
        }

        internal override bool IsNumeric => false;

        internal override AttributeDefinition CreateJoinAttributeDefinition(AttributeDefinition relatedAttributeDefinition)
        {
            return new JoinedAttributeDefinition(this, relatedAttributeDefinition);
        }

        internal override Expression CreateExpression(Expression parameter)
        {
            return Expression.Property(parameter, _property);
        }
    }
}
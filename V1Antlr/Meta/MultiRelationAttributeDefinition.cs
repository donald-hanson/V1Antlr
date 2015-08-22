using System;
using System.Linq.Expressions;
using System.Reflection;

namespace V1Antlr.Meta
{
    public class MultiRelationAttributeDefinition : AttributeDefinition
    {
        private readonly Type _type;
        private readonly PropertyInfo _property;
        private readonly Type _relatedType;
        private readonly FilterTerm _filterTerm;

        public MultiRelationAttributeDefinition(Type type, PropertyInfo property, Type relatedType, AssetType assetType, AssetType relatedAssetType) 
            : base(assetType, property.Name, false, relatedAssetType, true)
        {
            _type = type;
            _property = property;
            _relatedType = relatedType;
        }

        public MultiRelationAttributeDefinition(Type type, PropertyInfo property, Type relatedType, AssetType assetType, AssetType relatedAssetType, FilterTerm filterTerm)
            : base(assetType, property.Name+"["+filterTerm+"]", false, relatedAssetType, true)
        {
            _type = type;
            _property = property;
            _relatedType = relatedType;
            _filterTerm = filterTerm;
        }

        internal override bool IsNumeric => false;

        internal override AttributeDefinition CreateJoinAttributeDefinition(AttributeDefinition relatedAttributeDefinition)
        {
            return new JoinedAttributeDefinition(this, relatedAttributeDefinition);
        }

        internal override AttributeDefinition CreateAggregateAttributeDefinition(AggregateType aggregateType)
        {
            if (aggregateType == AggregateType.Count)
                return new AggregateCountAttributeDefinition(this);

            return base.CreateAggregateAttributeDefinition(aggregateType);
        }

        internal override AttributeDefinition CreateFilteredAttributeDefinition(FilterTerm filterTerm)
        {
            return new MultiRelationAttributeDefinition(_type, _property, _relatedType, AssetType, RelatedAssetType, filterTerm);
        }

        internal override Expression CreateExpression(Expression parameter)
        {
            return Expression.Property(parameter, _property);
        }
    }
}
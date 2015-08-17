using System;
using System.Reflection;

namespace V1Antlr.Meta
{
    public class MultiRelationAttributeDefinition : AttributeDefinition
    {
        private readonly Type _type;
        private readonly PropertyInfo _property;
        private readonly Type _relatedType;

        public MultiRelationAttributeDefinition(Type type, PropertyInfo property, Type relatedType, AssetType assetType, AssetType relatedAssetType) 
            : base(assetType, property.Name, false, relatedAssetType, true)
        {
            _type = type;
            _property = property;
            _relatedType = relatedType;
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
    }
}
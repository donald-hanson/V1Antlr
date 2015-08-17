namespace V1Antlr.Meta
{
    public class JoinedAttributeDefinition : AttributeDefinition
    {
        private readonly AttributeDefinition _left;
        private readonly AttributeDefinition _right;

        public JoinedAttributeDefinition(AttributeDefinition left, AttributeDefinition right)
            :base(left.AssetType, left.Name + "." + right.Name, right.IsNullable, right.RelatedAssetType, left.IsMultiValue || right.IsMultiValue)
        {
            _left = left;
            _right = right;
        }

        internal override bool IsNumeric => _right.IsNumeric;

        internal override AttributeDefinition CreateJoinAttributeDefinition(AttributeDefinition relatedAttributeDefinition)
        {
            return new JoinedAttributeDefinition(this, relatedAttributeDefinition);
        }

        internal override AttributeDefinition CreateAggregateAttributeDefinition(AggregateType aggregateType)
        {
            if (IsMultiValue)
            {
                if (aggregateType == AggregateType.Count)
                    return new AggregateCountAttributeDefinition(this);

                if (IsNumeric)
                {
                    if (aggregateType == AggregateType.Sum)
                        return new AggregateSumAttributeDefinition(this);
                    else if (aggregateType == AggregateType.Max)
                        return new AggregateMaxAttributeDefinition(this);
                    else if (aggregateType == AggregateType.Min)
                        return new AggregateMinAttributeDefinition(this);
                }
            }

            return base.CreateAggregateAttributeDefinition(aggregateType);
        }
    }
}
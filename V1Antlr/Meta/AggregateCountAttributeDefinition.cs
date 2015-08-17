namespace V1Antlr.Meta
{
    public class AggregateCountAttributeDefinition : AttributeDefinition
    {
        public AggregateCountAttributeDefinition(AttributeDefinition left)
            :base(left.AssetType, left.Name + ".@Count", false, null, false)
        {           
        }

        internal override bool IsNumeric => true;
    }

    public class AggregateSumAttributeDefinition : AttributeDefinition
    {
        public AggregateSumAttributeDefinition(AttributeDefinition left)
            : base(left.AssetType, left.Name + ".@Sum", false, null, false)
        {    
        }

        internal override bool IsNumeric => true;
    }

    public class AggregateMaxAttributeDefinition : AttributeDefinition
    {
        public AggregateMaxAttributeDefinition(AttributeDefinition left)
            : base(left.AssetType, left.Name + ".@Max", false, null, false)
        {
        }

        internal override bool IsNumeric => true;
    }

    public class AggregateMinAttributeDefinition : AttributeDefinition
    {
        public AggregateMinAttributeDefinition(AttributeDefinition left)
            : base(left.AssetType, left.Name + ".@Min", false, null, false)
        {
        }

        internal override bool IsNumeric => true;
    }
}
namespace V1Antlr.Meta
{
    public enum OrderTermDirection
    {
        Ascending,
        Descending
    }

    public class OrderTerm
    {
        public readonly OrderTermDirection Direction;
        public readonly AttributeDefinition AttributeDefinition;

        public OrderTerm(AttributeDefinition attributeDefinition, OrderTermDirection direction)
        {
            AttributeDefinition = attributeDefinition;
            Direction = direction;
        }

        public override string ToString()
        {
            return $"{AttributeDefinition} {Direction}";
        }
    }
}
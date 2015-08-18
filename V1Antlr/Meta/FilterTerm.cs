using System.Collections.Generic;

namespace V1Antlr.Meta
{
    public enum FilterTermType
    {
        And,
        Or,
        Field
    }

    public abstract class FilterTerm
    {
        public abstract FilterTermType Type { get; }
        public abstract IEnumerable<FilterTerm> Terms { get; }
    }

    public abstract class GroupFilterTerm : FilterTerm
    {
        private readonly List<FilterTerm>  _terms = new List<FilterTerm>();
        public override IEnumerable<FilterTerm> Terms => _terms;
        public void Add(FilterTerm term) => _terms.Add(term);
    }

    public class AndFilterTerm : GroupFilterTerm
    {
        public override FilterTermType Type { get; } = FilterTermType.And;
    }

    public class OrFilterTerm : GroupFilterTerm
    {
        public override FilterTermType Type { get; } = FilterTermType.Or;
    }

    public class FieldFilterTerm : FilterTerm
    {
        public override FilterTermType Type { get; } = FilterTermType.Field;
        public override IEnumerable<FilterTerm> Terms => new[] {this};


        public static FieldFilterTerm Exists(AttributeDefinition attributeDefinition)
        {
            return null;
        }

        public static FieldFilterTerm NotExists(AttributeDefinition attributeDefinition)
        {
            return null;
        }

        public static FieldFilterTerm Equal(AttributeDefinition attributeDefinition, IEnumerable<object> values)
        {
            return null;
        }

        public static FieldFilterTerm NotEqual(AttributeDefinition attributeDefinition, IEnumerable<object> values)
        {
            return null;
        }

        public static FieldFilterTerm Less(AttributeDefinition attributeDefinition, IEnumerable<object> values)
        {
            return null;
        }

        public static FieldFilterTerm LessOrEqual(AttributeDefinition attributeDefinition, IEnumerable<object> values)
        {
            return null;
        }

        public static FieldFilterTerm Greater(AttributeDefinition attributeDefinition, IEnumerable<object> values)
        {
            return null;
        }

        public static FieldFilterTerm GreaterOrEqual(AttributeDefinition attributeDefinition, IEnumerable<object> values)
        {
            return null;
        }
    }
}

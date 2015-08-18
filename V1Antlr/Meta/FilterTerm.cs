using System.Collections.Generic;
using System.Linq;

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
        protected abstract string Separator { get; }
        public override string ToString()
        {
            var inner = string.Join(Separator, _terms.Select(x => x.ToString()));
            if (_terms.Count > 1)
                inner = $"({inner})";
            return inner;
        }
    }

    public class AndFilterTerm : GroupFilterTerm
    {
        public override FilterTermType Type { get; } = FilterTermType.And;
        protected override string Separator { get; } = ";";
    }

    public class OrFilterTerm : GroupFilterTerm
    {
        public override FilterTermType Type { get; } = FilterTermType.Or;
        protected override string Separator { get; } = "|";
    }

    public enum FieldFilterTermOperator
    {
        Exists,
        NotExists,
        Equal,
        NotEqual,
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual
    }

    public class FieldFilterTerm : FilterTerm
    {
        private FieldFilterTerm(AttributeDefinition attributeDefinition, FieldFilterTermOperator op,
            IEnumerable<object> values)
        {
            AttributeDefinition = attributeDefinition;
            Operator = op;
            Values = values;
        }

        public override FilterTermType Type { get; } = FilterTermType.Field;
        public override IEnumerable<FilterTerm> Terms => new[] {this};

        public AttributeDefinition AttributeDefinition { get; }
        public FieldFilterTermOperator Operator { get; }
        public IEnumerable<object> Values { get; }

        public override string ToString()
        {
            var name = AttributeDefinition.Name;
            string op = null;
            switch (Operator)
            {
                case FieldFilterTermOperator.Exists:
                    return $"+{name}";
                case FieldFilterTermOperator.NotExists:
                    return $"-{name}";
                case FieldFilterTermOperator.Equal:
                    op = "=";
                    break;
                case FieldFilterTermOperator.NotEqual:
                    op = "!=";
                    break;
                case FieldFilterTermOperator.Less:
                    op = "<";
                    break;
                case FieldFilterTermOperator.LessOrEqual:
                    op = "<=";
                    break;
                case FieldFilterTermOperator.Greater:
                    op = ">";
                    break;
                case FieldFilterTermOperator.GreaterOrEqual:
                    op = ">=";
                    break;
            }
            string values = string.Join(",", Values.Select(x => $"'{x}'"));
            return $"{name}{op}{values}";
        }


        public static FieldFilterTerm Exists(AttributeDefinition attributeDefinition)
        {
            return new FieldFilterTerm(attributeDefinition, FieldFilterTermOperator.Exists, Enumerable.Empty<object>());
        }

        public static FieldFilterTerm NotExists(AttributeDefinition attributeDefinition)
        {
            return new FieldFilterTerm(attributeDefinition, FieldFilterTermOperator.NotExists, Enumerable.Empty<object>());
        }

        public static FieldFilterTerm Equal(AttributeDefinition attributeDefinition, IEnumerable<object> values)
        {
            return new FieldFilterTerm(attributeDefinition, FieldFilterTermOperator.Equal, values);
        }

        public static FieldFilterTerm NotEqual(AttributeDefinition attributeDefinition, IEnumerable<object> values)
        {
            return new FieldFilterTerm(attributeDefinition, FieldFilterTermOperator.NotEqual, values);
        }

        public static FieldFilterTerm Less(AttributeDefinition attributeDefinition, IEnumerable<object> values)
        {
            return new FieldFilterTerm(attributeDefinition, FieldFilterTermOperator.Less, values);
        }

        public static FieldFilterTerm LessOrEqual(AttributeDefinition attributeDefinition, IEnumerable<object> values)
        {
            return new FieldFilterTerm(attributeDefinition, FieldFilterTermOperator.LessOrEqual, values);
        }

        public static FieldFilterTerm Greater(AttributeDefinition attributeDefinition, IEnumerable<object> values)
        {
            return new FieldFilterTerm(attributeDefinition, FieldFilterTermOperator.Greater, values);
        }

        public static FieldFilterTerm GreaterOrEqual(AttributeDefinition attributeDefinition, IEnumerable<object> values)
        {
            return new FieldFilterTerm(attributeDefinition, FieldFilterTermOperator.GreaterOrEqual, values);
        }
    }
}

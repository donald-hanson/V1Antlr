using System;
using System.Linq;
using System.Linq.Expressions;

namespace V1Antlr.Meta
{
    public abstract class BaseAggregateAttributeDefinition : AttributeDefinition
    {
        private readonly AttributeDefinition _left;

        protected BaseAggregateAttributeDefinition(AttributeDefinition left, string suffix)
            :base(left.AssetType, left.Name + "." + suffix, false, null, false)
        {
            _left = left;
        }

        internal override bool IsNumeric => true;

        internal override Expression CreateExpression(Expression parameter)
        {
            var leftExpression = _left.CreateExpression(parameter);

            var innerType = leftExpression.Type.GetGenericArguments()[0];

            var enumerableCall = Expression.Call(typeof(Enumerable), AggregateMethodName, new[] { innerType }, leftExpression);

            return enumerableCall;
        }

        internal override object Coerce(string value)
        {
            return Convert.ToInt32(value);
        }

        protected abstract string AggregateMethodName { get; }
    }

    public class AggregateCountAttributeDefinition : BaseAggregateAttributeDefinition
    {
        public AggregateCountAttributeDefinition(AttributeDefinition left) :base(left, "@Count")
        {
        }

        protected override string AggregateMethodName { get; } = "Count";
    }

    public class AggregateSumAttributeDefinition : BaseAggregateAttributeDefinition
    {
        public AggregateSumAttributeDefinition(AttributeDefinition left) :base(left, "@Sum")
        {
        }

        protected override string AggregateMethodName { get; } = "Sum";
    }

    public class AggregateMaxAttributeDefinition : BaseAggregateAttributeDefinition
    {
        public AggregateMaxAttributeDefinition(AttributeDefinition left) :base(left, "@Max")
        {
        }

        protected override string AggregateMethodName { get; } = "Max";
    }

    public class AggregateMinAttributeDefinition : BaseAggregateAttributeDefinition
    {
        public AggregateMinAttributeDefinition(AttributeDefinition left) :base(left, "@Min")
        {
        }

        protected override string AggregateMethodName { get; } = "Min";
    }
}
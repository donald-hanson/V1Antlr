using System.Linq;
using System.Linq.Expressions;

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

        internal override Expression CreateExpression(Expression parameter)
        {
            if (_left.IsMultiValue)
            {
                if (_right.IsMultiValue)
                {
                    // left.SelectMany(x=>right)
                    var left = _left.CreateExpression(parameter);
                    var innerType = left.Type.GetGenericArguments()[0];

                    var innerParameter = Expression.Parameter(innerType);
                    var right = _right.CreateExpression(innerParameter);
                    var rightType = right.Type.GetGenericArguments()[0];

                    var lambda = Expression.Lambda(right, innerParameter);

                    return Expression.Call(typeof(Enumerable), "SelectMany", new[] { innerType, rightType }, left, lambda);
                }
                else
                {
                    // left.Select(x=>right)
                    var left = _left.CreateExpression(parameter);
                    var innerType = left.Type.GetGenericArguments()[0];

                    var innerParameter = Expression.Parameter(innerType);
                    var right = _right.CreateExpression(innerParameter);

                    var lambda = Expression.Lambda(right, innerParameter);

                    return Expression.Call(typeof(Enumerable), "Select", new[] { innerType, lambda.ReturnType }, left, lambda);
                }
            }
            else
            {
                // left.right
                var left = _left.CreateExpression(parameter);
                //TODO: what happens if left is null?
                return _right.CreateExpression(left);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace V1Antlr.Meta
{
    public enum AggregateType
    {
        Sum,
        Count,
        Min,
        Max
    }

    public abstract class AttributeDefinition
    {
        protected AttributeDefinition(AssetType assetType, string name, bool nullable, AssetType relatedAssetType, bool isMultiValue)
        {
            AssetType = assetType;
            Token = $"{assetType.Token}.{name}";
            Name = name;
            IsNullable = nullable;
            RelatedAssetType = relatedAssetType;
            IsMultiValue = isMultiValue;
        }

        public string Token { get; }
        public string Name { get; }
        public AssetType AssetType { get; }
        public bool IsNullable { get; }
        public AssetType RelatedAssetType { get; }
        public bool IsMultiValue { get; }

        internal abstract bool IsNumeric { get; }

        internal virtual AttributeDefinition CreateDowncastAttributeDefinition(AssetType downcastAssetType)
        {
            throw new NotSupportedException($"Cannot downcast {Token} to {downcastAssetType.Token}");
        }

        internal virtual AttributeDefinition CreateJoinAttributeDefinition(AttributeDefinition relatedAttributeDefinition)
        {
            throw new NotSupportedException($"Cannot join {Token} to {relatedAttributeDefinition.Token}");
        }

        internal virtual AttributeDefinition CreateAggregateAttributeDefinition(AggregateType aggregateType)
        {
            throw new NotSupportedException($"Cannot aggregate {Token} for {aggregateType}");
        }

        internal virtual AttributeDefinition CreateFilteredAttributeDefinition(FilterTerm filterTerm)
        {
            throw new NotSupportedException($"Cannot filter {Token} for {filterTerm}");
        }

        public override string ToString()
        {
            return Token;
        }

        internal virtual object Coerce(string value)
        {
            return value;
        }

        internal virtual Expression CreateExpression(Expression parameter)
        {
            throw new NotImplementedException($"{GetType().Name}.CreateExpression for {Token}");
        }

        internal virtual Expression CreateExistsFilterExpression(Expression parameter)
        {
            throw new NotImplementedException($"{GetType().Name}.CreateExistsFilterExpression for {Token}");
        }

        internal virtual Expression CreateFilterExpression(FieldFilterTermOperator @operator, IEnumerable<object> values, Expression parameter)
        {
            if (@operator == FieldFilterTermOperator.Exists)
                return CreateExistsFilterExpression(parameter);

            if (@operator == FieldFilterTermOperator.NotExists)
            {
                var existsExpression = CreateExistsFilterExpression(parameter);
                return Expression.Not(existsExpression);
            }

            var method = GetOperatorMethod(@operator);

            BinaryExpression result = null;
            foreach (var value in values)
            {
                var left = CreateExpression(parameter);
                var right = Expression.Constant(value);
                var inner = method(left, right);
                result = result == null ? inner : Expression.OrElse(result, inner);
            }
            return result;
        }

        private Func<Expression, Expression, BinaryExpression> GetOperatorMethod(FieldFilterTermOperator @operator)
        {
            switch (@operator)
            {
                case FieldFilterTermOperator.Equal:
                    return Expression.Equal;
                case FieldFilterTermOperator.NotEqual:
                    return Expression.NotEqual;
                case FieldFilterTermOperator.Greater:
                    return Expression.GreaterThan;
                case FieldFilterTermOperator.GreaterOrEqual:
                    return Expression.GreaterThanOrEqual;
                case FieldFilterTermOperator.Less:
                    return Expression.LessThan;
                case FieldFilterTermOperator.LessOrEqual:
                    return Expression.LessThanOrEqual;
            }

            throw new NotSupportedException($"Unsupported operator {@operator}");
        }
    }
}
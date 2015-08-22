using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using V1Antlr.Extensions;

namespace V1Antlr.Meta
{
    public class PrimitveAttributeDefinition : AttributeDefinition
    {
        private readonly Type _ownedType;
        private readonly PropertyInfo _property;

        public PrimitveAttributeDefinition(Type ownedType, PropertyInfo property, AssetType assetType, bool nullable) 
            :base(assetType, property.Name, nullable, null, false)
        {
            _ownedType = ownedType;
            _property = property;
        }

        internal override bool IsNumeric => _property.PropertyType.IsNumeric();

        internal override object Coerce(string value)
        {
            return Convert.ChangeType(value, _property.PropertyType);
        }

        internal override Expression CreateExpression(Expression parameter)
        {
            return Expression.Property(parameter, _property);
        }

        internal override BinaryExpression CreateFilterExpression(FieldFilterTermOperator @operator, IEnumerable<object> values, Expression parameter)
        {
            if (@operator == FieldFilterTermOperator.Exists)
                throw new NotSupportedException("Exists on Primitve");
            if (@operator == FieldFilterTermOperator.NotExists)
                throw new NotSupportedException("NotExists on Primitve");

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
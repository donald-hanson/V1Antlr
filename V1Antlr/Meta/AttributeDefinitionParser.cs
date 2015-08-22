using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace V1Antlr.Meta
{
    public class AttributeDefinitionParser
    {
        public static AttributeDefinition Parse(string attributeDefinitionToken, MetaModel metaModel)
        {
            ICharStream charStream = new AntlrInputStream(attributeDefinitionToken);
            ITokenSource tokenSource = new V1QueryLexer(charStream);
            ITokenStream tokenStream = new CommonTokenStream(tokenSource);
            var parser = new V1QueryParser(tokenStream);

            var parseTree = parser.attribute_definition_token();
            var assetTypeToken = parseTree.asset_type_token().GetText();
            var assetType = metaModel.GetAssetType(assetTypeToken);

            var attributeName = parseTree.attribute_name().GetText();

            return Parse(attributeName, assetType, metaModel);
        }

        public static AttributeDefinition Parse(string attributeNameToken, AssetType assetType, MetaModel metaModel)
        {
            ICharStream charStream = new AntlrInputStream(attributeNameToken);
            ITokenSource tokenSource = new V1QueryLexer(charStream);
            ITokenStream tokenStream = new CommonTokenStream(tokenSource);
            var parser = new V1QueryParser(tokenStream);
            var attributeName = parser.attribute_name();
            var visitor = new AttributeDefinitionVisitor(assetType, metaModel);
            return visitor.Visit(attributeName);
        }

        public static FilterTerm ParseFilter(string filterToken, AssetType assetType, MetaModel metaModel)
        {
            ICharStream charStream = new AntlrInputStream(filterToken);
            ITokenSource tokenSource = new V1QueryLexer(charStream);
            ITokenStream tokenStream = new CommonTokenStream(tokenSource);
            var parser = new V1QueryParser(tokenStream);
            var expression = parser.filter_expression();
            var visitor = new FilterVisitor(assetType, metaModel);
            return visitor.Visit(expression);
        }

        public static OrderTerm ParseOrderTerm(string filterToken, AssetType assetType, MetaModel metaModel)
        {
            ICharStream charStream = new AntlrInputStream(filterToken);
            ITokenSource tokenSource = new V1QueryLexer(charStream);
            ITokenStream tokenStream = new CommonTokenStream(tokenSource);
            var parser = new V1QueryParser(tokenStream);
            var expression = parser.sort_token_term();
            var visitor = new OrderVisitor(assetType, metaModel);
            return visitor.Visit(expression);
        }
    }

    public class AttributeDefinitionVisitor : V1QueryBaseVisitor<AttributeDefinition>
    {
        private readonly AssetType _rootAssetType;
        private readonly MetaModel _metaModel;

        public AttributeDefinitionVisitor(AssetType rootAssetType, MetaModel metaModel)
        {
            _rootAssetType = rootAssetType;
            _metaModel = metaModel;
        }

        public override AttributeDefinition VisitAttribute_name(V1QueryParser.Attribute_nameContext context)
        {
            var parts = context.attribute_name_part();
            var result = Visit(parts[0]);
            if (parts.Length > 1)
            {
                for (var i = 1; i < parts.Length; i++)
                {
                    var part = parts[i];
                    var subVisitor = new AttributeDefinitionVisitor(result.RelatedAssetType, _metaModel);
                    var relatedAttributeDefinition = subVisitor.Visit(part);

                    result = result.CreateJoinAttributeDefinition(relatedAttributeDefinition);
                }
            }

            var agg = context.aggregation_name();
            if (agg != null)
            {
                var aggregateName = agg.NAME().GetText();
                var aggregateType = (AggregateType)Enum.Parse(typeof (AggregateType), aggregateName);
                result = result.CreateAggregateAttributeDefinition(aggregateType);
            }

            return result;
        }

        public override AttributeDefinition VisitAttribute_name_part(V1QueryParser.Attribute_name_partContext context)
        {
            var attributeName = context.NAME().GetText();
            var downcast = context.downcast()?.asset_type_token().GetText();
            var filter = context.attribute_filter();

            AttributeDefinition result;
            if (!_metaModel.TryGetAttributeDefinition(_rootAssetType, attributeName, out result))
                throw new InvalidOperationException($"Cannot find attribute definition {_rootAssetType.Token}.{attributeName}");

            if (!string.IsNullOrEmpty(downcast))
            {
                var downcastAssetType = _metaModel.GetAssetType(downcast);
                result = result.CreateDowncastAttributeDefinition(downcastAssetType);
            }

            if (filter != null)
            {
                var subVisitor = new FilterVisitor(result.RelatedAssetType, _metaModel);
                var subFilter = subVisitor.Visit(filter);
                result = result.CreateFilteredAttributeDefinition(subFilter);
            }

            return result;
        }
    }

    public class FilterVisitor : V1QueryBaseVisitor<FilterTerm>
    {
        private readonly AssetType _rootAssetType;
        private readonly MetaModel _metaModel;

        public FilterVisitor(AssetType rootAssetType, MetaModel metaModel)
        {
            _rootAssetType = rootAssetType;
            _metaModel = metaModel;
        }

        public override FilterTerm VisitAttribute_filter(V1QueryParser.Attribute_filterContext context)
        {
            var exp = context.filter_expression();
            return Visit(exp);
        }

        public override FilterTerm VisitFilter_expression(V1QueryParser.Filter_expressionContext context)
        {
            var firstGroup = context.grouped_filter_term(0);
            var firstSimple = context.simple_filter_term(0);

            FilterTerm leftTerm = firstGroup != null ? Visit(firstGroup) : Visit(firstSimple);

            int index = 0;
            while (true)
            {
                FilterTermType groupOp;
                if (context.and_operator(index) != null)
                    groupOp = FilterTermType.And;
                else if (context.or_operator(index) != null)
                    groupOp = FilterTermType.Or;
                else
                    break;

                index++;

                var nextGroup = context.grouped_filter_term(index);
                var nextSimple = context.simple_filter_term(index);

                FilterTerm rightTerm = nextGroup != null ? Visit(nextGroup) : Visit(nextSimple);

                if (leftTerm.Type == groupOp)
                {
                    ((GroupFilterTerm) leftTerm).Add(rightTerm);
                }
                else
                {
                    var groupTerm = groupOp == FilterTermType.And ? (GroupFilterTerm) new AndFilterTerm() : new OrFilterTerm();
                    groupTerm.Add(leftTerm);
                    groupTerm.Add(rightTerm);
                    leftTerm = groupTerm;
                }
            }

            return leftTerm;
        }

        public override FilterTerm VisitGrouped_filter_term(V1QueryParser.Grouped_filter_termContext context)
        {
            var exp = context.filter_expression();
            return Visit(exp);
        }

        public override FilterTerm VisitSimple_filter_term(V1QueryParser.Simple_filter_termContext context)
        {
            var attributeName = context.attribute_name();
            var attributeVisitor = new AttributeDefinitionVisitor(_rootAssetType, _metaModel);
            var attributeDefinition = attributeVisitor.Visit(attributeName);

            var binaryOperator = context.binary_operator();
            if (binaryOperator == null)
            {
                var unaryOperator = context.unary_operator();
                if (unaryOperator?.MINUS() != null)
                    return FieldFilterTerm.NotExists(attributeDefinition);
                return FieldFilterTerm.Exists(attributeDefinition);
            }

            List<object> values = new List<object>();

            var valueList = context.filter_value_list();
            if (valueList != null)
            {
                var filterValues = valueList.filter_value();
                foreach (var filterValue in filterValues)
                {
                    var singleQuoteValue = filterValue.SINGLE_QUOTED_STRING();
                    var doubleQuoteValue = filterValue.DOUBLE_QUOTED_STRING();
                    string filterValueToken = singleQuoteValue?.GetText()?? doubleQuoteValue.GetText();
                    filterValueToken = filterValueToken.Substring(1, filterValueToken.Length - 2);
                    values.Add(attributeDefinition.Coerce(filterValueToken));
                }
            }
            else
            {
                var variableContext = context.variable();
                throw new NotImplementedException("Filter Variables");
            }

            
            var binaryOperatorText = binaryOperator.GetText();
            switch (binaryOperatorText)
            {
                case "=":
                    return FieldFilterTerm.Equal(attributeDefinition, values);
                case "!=":
                    return FieldFilterTerm.NotEqual(attributeDefinition, values);
                case "<":
                    return FieldFilterTerm.Less(attributeDefinition, values);
                case "<=":
                    return FieldFilterTerm.LessOrEqual(attributeDefinition, values);
                case ">":
                    return FieldFilterTerm.Greater(attributeDefinition, values);
                case ">=":
                    return FieldFilterTerm.GreaterOrEqual(attributeDefinition, values);
                default:
                    throw new NotSupportedException("Unknown binary operator");
            }
        }
    }

    public class OrderVisitor : V1QueryBaseVisitor<OrderTerm>
    {
        private readonly AssetType _assetType;
        private readonly MetaModel _metaModel;

        public OrderVisitor(AssetType assetType, MetaModel metaModel)
        {
            _assetType = assetType;
            _metaModel = metaModel;
        }

        public override OrderTerm VisitSort_token_term(V1QueryParser.Sort_token_termContext context)
        {
            var asc = context.asc_sort_token_term();
            var desc = context.desc_sort_token_term();
            return asc != null ? Visit(asc) : Visit(desc);
        }

        public override OrderTerm VisitAsc_sort_token_term(V1QueryParser.Asc_sort_token_termContext context)
        {
            var visitor = new AttributeDefinitionVisitor(_assetType, _metaModel);
            var attributeDefinition = visitor.VisitAttribute_name(context.attribute_name());
            return new OrderTerm(attributeDefinition, OrderTermDirection.Ascending);
        }

        public override OrderTerm VisitDesc_sort_token_term(V1QueryParser.Desc_sort_token_termContext context)
        {
            var visitor = new AttributeDefinitionVisitor(_assetType, _metaModel);
            var attributeDefinition = visitor.VisitAttribute_name(context.attribute_name());
            return new OrderTerm(attributeDefinition, OrderTermDirection.Descending);
        }
    }
}
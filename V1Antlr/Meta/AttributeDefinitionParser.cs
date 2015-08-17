using System;
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

        public static AttributeDefinition Parse(string attributeDefinitionToken, AssetType assetType, MetaModel metaModel)
        {
            ICharStream charStream = new AntlrInputStream(attributeDefinitionToken);
            ITokenSource tokenSource = new V1QueryLexer(charStream);
            ITokenStream tokenStream = new CommonTokenStream(tokenSource);
            var parser = new V1QueryParser(tokenStream);
            var attributeName = parser.attribute_name();
            var visitor = new AttributeDefinitionVisitor(assetType, metaModel);
            return visitor.Visit(attributeName);
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
                throw new NotImplementedException("Filtered Attribute Definition");
            }

            return result;
        }
    }
}
using System.Collections.Generic;
using V1Antlr.Meta;

namespace V1Antlr.Data
{
    public class QueryBuilder
    {
        private readonly AssetType _assetType;
        private readonly MetaModel _metaModel;

        private readonly ICollection<AttributeDefinition> _selection = new List<AttributeDefinition>();
        private FilterTerm _filter = new AndFilterTerm();

        private QueryBuilder(AssetType assetType, MetaModel metaModel)
        {
            _assetType = assetType;
            _metaModel = metaModel;
        }

        public static QueryBuilder For(string assetTypeToken, MetaModel metaModel)
        {
            var assetType = metaModel.GetAssetType(assetTypeToken);
            return new QueryBuilder(assetType, metaModel);
        }

        public QueryBuilder Select(params string[] attributeDefinitionNames)
        {
            foreach (var attributeDefintionName in attributeDefinitionNames)
            {
                var attributeDefinition = _assetType.GetAttributeDefinition(attributeDefintionName);
                _selection.Add(attributeDefinition);
            }
            return this;
        }

        public QueryBuilder Where(string filterToken)
        {
            _filter = _assetType.GetFilterTerm(filterToken);
            return this;
        }

        public Query ToQuery()
        {
            return new Query(_assetType, _selection, _filter, _metaModel);
        }
    }

    public class Query
    {
        public readonly AssetType AssetType;
        public readonly IEnumerable<AttributeDefinition> Selection;
        public readonly FilterTerm Filter;
        public readonly MetaModel MetaModel;

        public Query(AssetType assetType, IEnumerable<AttributeDefinition> selection, FilterTerm filter, MetaModel metaModel)
        {
            AssetType = assetType;
            Selection = selection;
            Filter = filter;
            MetaModel = metaModel;
        }
    }
}
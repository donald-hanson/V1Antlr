using System.Collections.Generic;
using System.Runtime.InteropServices;
using V1Antlr.Meta;

namespace V1Antlr.Data
{
    public class QueryBuilder
    {
        private readonly AssetType _assetType;
        private readonly MetaModel _metaModel;

        private readonly ICollection<AttributeDefinition> _selection = new List<AttributeDefinition>();
        private FilterTerm _filter = new AndFilterTerm();
        private int? _skip;
        private int? _take;

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

        public QueryBuilder Skip(int skip)
        {
            _skip = skip;
            return this;
        }

        public QueryBuilder Take(int take)
        {
            _take = take;
            return this;
        }

        public Query ToQuery()
        {
            return new Query(_assetType, _selection, _filter, _skip, _take, _metaModel);
        }
    }

    public class Query
    {
        public readonly AssetType AssetType;
        public readonly IEnumerable<AttributeDefinition> Selection;
        public readonly FilterTerm Filter;
        public readonly int? Skip;
        public readonly int? Take;
        public readonly MetaModel MetaModel;

        public Query(AssetType assetType, IEnumerable<AttributeDefinition> selection, FilterTerm filter, int? skip, int? take, MetaModel metaModel)
        {
            AssetType = assetType;
            Selection = selection;
            Filter = filter;
            Skip = skip;
            Take = take;
            MetaModel = metaModel;
        }
    }
}
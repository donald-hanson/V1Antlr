using System;

namespace V1Antlr.Meta
{
    public class AssetType
    {
        private readonly Type _type;
        private readonly MetaModel _metaModel;

        public AssetType(Type type, MetaModel metaModel)
        {
            _type = type;
            _metaModel = metaModel;
        }

        public string Token => _type.Name;

        public AssetType Base
        {
            get
            {
                if (_type.BaseType != null)
                {
                    AssetType baseType;
                    if (_metaModel.TryGetAssetType(_type.BaseType.Name, out baseType))
                        return baseType;
                }
                return null;
            }
        }

        public AttributeDefinition GetAttributeDefinition(string attributeNameToken)
        {
            return AttributeDefinitionParser.Parse(attributeNameToken, this, _metaModel);
        }
    }
}
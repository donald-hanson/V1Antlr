using System.Collections.Generic;
using V1Antlr.Meta;

namespace V1Antlr.Data
{
    public class Asset
    {
        public readonly AssetType AssetType;

        private readonly IDictionary<string, object> _attributeValues = new Dictionary<string, object>();

        public Asset(AssetType assetType)
        {
            AssetType = assetType;
        }

        public T GetAttributeValue<T>(string attributeNameToken)
        {
            return GetAttributeValue<T>(AssetType.GetAttributeDefinition(attributeNameToken));
        }

        public T GetAttributeValue<T>(AttributeDefinition attributeDefinition)
        {
            return (T) _attributeValues[attributeDefinition.Token];
        }

        internal void LoadAttribute(AttributeDefinition attributeDefinition, object value)
        {
            _attributeValues.Add(attributeDefinition.Token, value);
        }
    }
}
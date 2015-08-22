using System;
using System.Collections.Generic;
using System.Reflection;
using Magnum.Extensions;
using V1Antlr.Extensions;

namespace V1Antlr.Meta
{
    public class MetaModel
    {
        private readonly IDictionary<string, AssetType>  _assetTypes = new Dictionary<string, AssetType>();
        private readonly IDictionary<string, AttributeDefinition> _attributeDefinitions = new Dictionary<string, AttributeDefinition>();

        public void RegisterTypes(params Type[] types)
        {
            foreach (var type in types)
            {
                var assetType = new AssetType(type, this);
                _assetTypes.Add(assetType.Token, assetType);
            }

            foreach (var type in types)
            {
                var assetType = _assetTypes[type.Name];
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    var attributeDefinition = CreateAttributeDefinition(type, property, assetType);
                    _attributeDefinitions.Add(attributeDefinition.Token, attributeDefinition);
                }
            }
        }

        public AssetType GetAssetType(string assetTypeToken)
        {
            return AssetTypeParser.Parse(assetTypeToken, this);
        }

        public AttributeDefinition GetAttributeDefinition(string attributeDefinitionToken)
        {
            return AttributeDefinitionParser.Parse(attributeDefinitionToken, this);
        }

        internal bool TryGetAssetType(string assetTypeToken, out AssetType assetType)
        {
            return _assetTypes.TryGetValue(assetTypeToken, out assetType);
        }

        internal bool TryGetAttributeDefinition(string attributeDefinitionToken, out AttributeDefinition attributeDefinition)
        {
            return _attributeDefinitions.TryGetValue(attributeDefinitionToken, out attributeDefinition);
        }

        internal bool TryGetAttributeDefinition(AssetType assetType, string attributeNameToken, out AttributeDefinition attributeDefinition)
        {
            string key = $"{assetType.Token}.{attributeNameToken}";
            return TryGetAttributeDefinition(key, out attributeDefinition);
        }

        private AttributeDefinition CreateAttributeDefinition(Type type, PropertyInfo property, AssetType assetType)
        {
            var propertyType = property.PropertyType;

            string propertyDebugName = string.Format("{0} {1}.{2}", propertyType.ToShortTypeName(), type.ToShortTypeName(), property.Name);

            if (propertyType == typeof (string))
                return new PrimitveAttributeDefinition(type, property, assetType, true);

            if (propertyType.IsSupportedPrimitiveType())
                return new PrimitveAttributeDefinition(type, property, assetType, false);

            if (propertyType.Implements(typeof(ICollection<>)))
            {
                var innerType = propertyType.GetGenericArguments()[0];
                if (innerType.IsClass && innerType != typeof (string))
                {
                    AssetType relatedAssetType;
                    if (!_assetTypes.TryGetValue(innerType.Name, out relatedAssetType))
                        throw new NotImplementedException($"Could not reslve related asset type: {propertyDebugName}");
                    return new MultiRelationAttributeDefinition(type, property, innerType, assetType, relatedAssetType);
                }

                throw new NotImplementedException($"Unsupported collection type: {propertyDebugName}");
            }

            if (propertyType.Implements(typeof(Nullable<>)))
            {
                var innerType = propertyType.GetGenericArguments()[0];
                if (innerType.IsSupportedPrimitiveType())
                    return new PrimitveAttributeDefinition(type, property, assetType, true);

                throw new NotImplementedException($"Unsupported nullable type: {propertyDebugName}");
            }

            if (propertyType.IsClass)
            {
                AssetType relatedAssetType;
                if (!_assetTypes.TryGetValue(propertyType.Name, out relatedAssetType))
                    throw new NotImplementedException($"Could not resolve related asset type: {propertyDebugName}");
                return new SingleRelationAttributeDefinition(type, property, assetType, true, relatedAssetType);
            }

            throw new NotImplementedException($"Unsupported property type: {propertyDebugName}");
        }
    }
}

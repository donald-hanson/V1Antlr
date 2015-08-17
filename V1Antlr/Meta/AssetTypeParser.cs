using System;

namespace V1Antlr.Meta
{
    public class AssetTypeParser
    {
        public static AssetType Parse(string assetTypeToken, MetaModel metaModel)
        {
            AssetType assetType;
            if (!metaModel.TryGetAssetType(assetTypeToken, out assetType))
                throw new InvalidOperationException($"{assetTypeToken} is not a valid assetType");
            return assetType; 
        }
    }
}
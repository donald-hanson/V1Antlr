using System;

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

        public override string ToString()
        {
            return Token;
        }

        public object Coerce(string value)
        {
            return value;
        }
    }
}
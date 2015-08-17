using System;
using System.Collections.Generic;

namespace V1Antlr.Sample
{
    public class ProductImage
    {
        private ICollection<ProductImageMetaField> _metaFields = new List<ProductImageMetaField>();
        private ICollection<ProductVariant> _productVariants = new List<ProductVariant>();

        public long ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Position { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Source { get; set; }
        public virtual ICollection<ProductImageMetaField> MetaFields { get { return _metaFields; } set { _metaFields = value; } }
        public virtual Product Product { get; set; }
        public virtual ICollection<ProductVariant> ProductVariants { get { return _productVariants; } set { _productVariants = value; } }
    }
}
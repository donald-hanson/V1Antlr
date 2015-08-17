using System;
using System.Collections.Generic;

namespace V1Antlr.Sample
{
    public class Product
    {
        private ICollection<ProductVariant> _variants = new List<ProductVariant>();
        private ICollection<ProductMetaField> _metaFields = new List<ProductMetaField>();
        private ICollection<ProductImage> _images = new List<ProductImage>();

        public long ID { get; set; }
        public string Title { get; set; }
        public string BodyHtml { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Handle { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string Vendor { get; set; }
        public string ProductType { get; set; }
        public string PublishedScope { get; set; }
        public bool Published { get; set; }

        public virtual ICollection<ProductVariant> Variants { get { return _variants; } set { _variants = value; } }
        public virtual ICollection<ProductMetaField> MetaFields { get { return _metaFields; } set { _metaFields = value; } }
        public virtual ICollection<ProductImage> Images { get { return _images; } set { _images = value; } }
    }
}
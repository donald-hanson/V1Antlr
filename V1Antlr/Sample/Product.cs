using System;
using System.Collections.Generic;

namespace V1Antlr.Sample
{
    public class Product
    {
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

        public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public virtual ICollection<ProductMetaField> MetaFields { get; set; } = new List<ProductMetaField>();
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
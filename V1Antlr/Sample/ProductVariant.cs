using System;
using System.Collections.Generic;

namespace V1Antlr.Sample
{
    public class ProductVariant
    {
        private ICollection<ProductVariantOption> _options = new List<ProductVariantOption>();
        private ICollection<ProductVariantMetaField> _metaFields = new List<ProductVariantMetaField>();
        private ICollection<ProductImage> _images = new List<ProductImage>();

        public long ID { get; set; }
        public string Barcode { get; set; }
        public decimal? CompareAtPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Price { get; set; }
        public virtual Product Product { get; set; }
        public string Title { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Taxable { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public string InventoryManagement { get; set; }
        public virtual ICollection<ProductVariantOption> Options { get { return _options; } set { _options = value; } }
        public string FulfillmentService { get; set; }
        public int Grams { get; set; }
        public string InventoryPolicy { get; set; }
        public int Position { get; set; }
        public bool RequiresShipping { get; set; }
        public int Weight { get; set; }
        public string WeightUnit { get; set; }
        public virtual ICollection<ProductVariantMetaField> MetaFields { get { return _metaFields; } set { _metaFields = value; } }
        public virtual ICollection<ProductImage> Images { get { return _images; } set { _images = value; } }

        //public long ImageId { get; set; }
    }
}
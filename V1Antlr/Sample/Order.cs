using System;
using System.Collections.Generic;

namespace V1Antlr.Sample
{
    public class Order
    {
        public long ID { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Address BillingAddress { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string Name { get; set; }

        public virtual ICollection<OrderLineItem> LineItems { get; set; } = new List<OrderLineItem>();
    }

    public class OrderLineItem
    {
        public long ID { get; set; }
        public virtual Order Order { get; set; }
        public virtual int FulfillableQuantity { get; set; }
        public virtual string FulfillmentService { get; set; }
        public virtual string FulfillmentStatus { get; set; }
        public virtual int Grams { get; set; }
        public virtual long Id { get; set; }
        public virtual decimal Price { get; set; }
        public virtual Product Product { get; set; }
        public virtual int Quantity { get; set; }
        public virtual bool RequiresShipping { get; set; }
        public virtual string Sku { get; set; }
        public virtual string Title { get; set; }
        public virtual ProductVariant Variant { get; set; }
        public virtual string VariantTitle { get; set; }
        public virtual string Vendor { get; set; }
        public virtual string Name { get; set; }
        public virtual bool GiftCard { get; set; }
        public virtual bool Taxable { get; set; }
        //IEnumerable<IShopifyOrderTaxLine> TaxLines { get; }
        public virtual decimal TotalDiscount { get; set; }
        public virtual ICollection<OrderLineItemProperty> Properties { get; set; } = new List<OrderLineItemProperty>();
    }

    public class OrderLineItemProperty
    {
        public long ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
    }
}

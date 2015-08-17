namespace V1Antlr.Sample
{
    public class Order
    {
        public long ID { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Address BillingAddress { get; set; }
        public virtual Address ShippingAddress { get; set; }
    }
}

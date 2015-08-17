namespace V1Antlr.Sample
{
    public class ProductVariantOption
    {
        public long ID { get; set; }
        public int Poisition { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
    }
}
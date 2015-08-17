namespace V1Antlr.Sample
{
    public abstract class MetaField
    {
        public long ID { get; set; }
        public string Namespace { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }
}
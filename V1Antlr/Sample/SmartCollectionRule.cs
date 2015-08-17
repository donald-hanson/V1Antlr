namespace V1Antlr.Sample
{
    public class SmartCollectionRule
    {
        public long ID { get; set; }
        public string Column { get; set; }
        public string Relation { get; set; }
        public string Condition { get; set; }
        public virtual SmartCollection SmartCollection { get; set; }
    }
}
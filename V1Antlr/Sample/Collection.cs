using System;
namespace V1Antlr.Sample
{
    public abstract class Collection
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string BodyHtml { get; set; }
        public string SortOrder { get; set; }
        public string Handle { get; set; }
        public bool Published { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string PublishedScope { get; set; }
        public string TemplateSuffix { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual CollectionImage CollectionImage { get; set; }
    }
}
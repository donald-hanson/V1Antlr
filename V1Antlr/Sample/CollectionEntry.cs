using System;

namespace V1Antlr.Sample
{
    public abstract class CollectionEntry
    {
        public long Id { get; set; }
        public virtual Product Product { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Featured { get; set; }
        public int Position { get; set; }
        public string SortValue { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
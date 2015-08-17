using System.Collections.Generic;

namespace V1Antlr.Sample
{
    public class CollectionImage
    {
        private ICollection<CollectionImageMetaField> _metaFields = new List<CollectionImageMetaField>();

        public long ID { get; set; }
        public string Source { get; set; }
        public virtual ICollection<CollectionImageMetaField> MetaFields { get { return _metaFields; } set { _metaFields = value; } }
    }
}
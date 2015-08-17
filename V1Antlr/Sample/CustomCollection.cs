using System.Collections.Generic;

namespace V1Antlr.Sample
{
    public class CustomCollection : Collection
    {
        public virtual ICollection<CustomCollectionMetaField> MetaFields { get; set; } = new List<CustomCollectionMetaField>();
    }
}
using System.Collections.Generic;

namespace V1Antlr.Sample
{
    public class SmartCollection : Collection
    {
        private ICollection<SmartCollectionMetaField> _metaFields = new List<SmartCollectionMetaField>();

        public bool Disjunctive { get; set; }

        public virtual ICollection<SmartCollectionMetaField> MetaFields { get { return _metaFields; } set { _metaFields = value; } }
        public virtual ICollection<SmartCollectionRule> Rules { get; set; }
    }
}
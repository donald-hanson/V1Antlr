using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace V1Antlr.Sample
{
    public class Customer
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool VerifiedEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public string MultipassIdentifier { get; set; }
        public long? LastOrderId { get; set; }
        public string LastOrderName { get; set; }
        public string Note { get; set; }
        public int OrdersCount { get; set; }
        public string State { get; set; }
        public bool TaxExempt { get; set; }
        public decimal? TotalSpent { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Address DefaultAddress { get; set; }
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
        public virtual ICollection<CustomerMetaField> MetaFields { get; set; } = new Collection<CustomerMetaField>();
    }
}
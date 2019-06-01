using SomeBasicEFApp.Web.ValueTypes;
using System.Collections.Generic;

namespace SomeBasicEFApp.Web.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public IList<Order> Orders { get; set; }

        public virtual int Version { get; set; }
        public CustomerId GetId() => new CustomerId(Id);
    }
}

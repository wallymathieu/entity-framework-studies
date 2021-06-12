using SomeBasicEFApp.Web.ValueTypes;
using System.Collections.Generic;

namespace SomeBasicEFApp.Web.Entities
{
    ///
    public class Customer
    {
        ///
        public CustomerId Id { get; set; }
        ///
        public string Firstname { get; set; }
        ///
        public string Lastname { get; set; }
        ///
        public IList<Order> Orders { get; set; }
        ///
        public int Version { get; set; }
    }
}

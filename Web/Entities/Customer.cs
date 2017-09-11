using System.Collections.Generic;
using SomeBasicEFApp.Web;

namespace SomeBasicEFApp.Web.Entities
{
    public class Customer
    {
        public virtual CustomerId Id { get; set; }

        public virtual string Firstname { get; set; }

        public virtual string Lastname { get; set; }

        public virtual IList<Order> Orders { get; set; }

        public virtual int Version { get; set; }

    }
}

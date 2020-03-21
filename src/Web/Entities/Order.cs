using System;
using System.Collections.Generic;
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Web.Entities
{
    public class Order
    {
        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderId Id { get; set; }

        public int Version { get; set; }

        public IList<ProductOrder> ProductOrders { get; set; }
    }
}

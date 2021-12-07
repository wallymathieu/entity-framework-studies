using System;
using System.Collections.Generic;
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Web.Entities
{
    ///
    public class Order
    {
        ///
        public Customer? Customer { get; set; }
        ///
        public DateTime OrderDate { get; init; }
        ///
        public OrderId Id { get; init; }
        ///
        public int Version { get; init; }
        ///
        public IList<ProductOrder> ProductOrders { get; init; } = new List<ProductOrder>();
    }
}

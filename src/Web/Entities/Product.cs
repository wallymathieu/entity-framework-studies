using System.Collections.Generic;
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Web.Entities
{
    public class Product
    {
        public float Cost { get; set; }

        public string Name { get; set; }

        public IList<ProductOrder> ProductOrders { get; set; }

        public int Id { get; set; }

        public int Version { get; set; }

        public ProductId GetId() => new ProductId(Id);
    }
}

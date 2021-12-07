using System.Collections.Generic;
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Web.Entities
{
    ///
    public class Product
    {
        ///
        public float Cost { get; init; }
        ///
        public string? Name { get; init; }
        ///
        public IList<ProductOrder> ProductOrders { get; init; } = new List<ProductOrder>();
        ///
        public ProductId Id { get; init; }
        ///
        public int Version { get; init; }
        ///
        public ProductType Type { get; init; } = new ProductType(null);
    }
}

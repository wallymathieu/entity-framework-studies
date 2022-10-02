using System.Collections.Generic;
using System.Text.Json.Serialization;
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
        [JsonIgnore]
        public IList<ProductOrder> ProductOrders { get; init; } = new List<ProductOrder>();
        ///
        public ProductId Id { get; init; }
        ///
        [JsonIgnore]
        public int Version { get; init; }
        ///
        public ProductType Type { get; init; } = new ProductType(null);
    }
}

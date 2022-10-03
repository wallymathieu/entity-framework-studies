using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Web.Entities;

///
public class Order
{
    ///
    [JsonIgnore]
    public Customer? Customer { get; set; }
    ///
    public DateTime OrderDate { get; init; }
    ///
    public OrderId Id { get; init; }
    ///
    [JsonIgnore]
    public int Version { get; init; }
    ///
    [JsonIgnore]
    public IList<ProductOrder> ProductOrders { get; init; } = new List<ProductOrder>();

    [NotMapped]
    public IEnumerable<Product> Products => ProductOrders.Select(po => po.Product).ToArray();
}

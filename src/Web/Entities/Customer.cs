using SomeBasicEFApp.Web.ValueTypes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SomeBasicEFApp.Web.Entities;

///
public class Customer
{
    ///
    public CustomerId Id { get; init; }
    ///
    public string? Firstname { get; set; }
    ///
    public string? Lastname { get; set; }
    ///
    [JsonIgnore]
    public IList<Order> Orders { get; init; } = new List<Order>();
    ///
    [JsonIgnore]
    public int Version { get; init; }
}

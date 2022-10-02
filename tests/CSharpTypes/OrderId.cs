using System.Text.Json.Serialization;
using SaitheSystemTextJson;

namespace CSharpTypes;

/// <summary>
/// Order identifier, simple wrapper around long value. Since it wraps long we need to use the JsonConverter
/// </summary>
[JsonConverter(typeof(ValueTypeLongSystemTextJsonConverter<OrderId>))]
public struct OrderId : IEquatable<OrderId>
{
    public readonly long Value;
    public OrderId(long value) => Value = value;
    public bool Equals(OrderId other) => Equals(Value, other.Value);
    public override bool Equals(object? obj) => obj is OrderId id && Equals(id);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}

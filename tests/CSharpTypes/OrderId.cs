using System.Text.Json.Serialization;
using SaitheSystemTextJson;

namespace CSharpTypes;

/// <summary>
/// Order identifier, simple wrapper around long value. Since it wraps long we need to use the JsonConverter
/// </summary>
[JsonConverter(typeof(ParseTypeSystemTextJsonConverter<OrderId>))]
public struct OrderId : IEquatable<OrderId>
{
    public readonly long Value;

    public OrderId(long value)
    {
        this.Value = value;
    }

    public readonly static OrderId Empty = new OrderId();

    public bool Equals(OrderId other)
    {
        if (ReferenceEquals(null, other)) return false;
        return Equals(Value, other.Value);
    }
    public override bool Equals(object obj)
    {
        if (obj is OrderId)
            return Equals((CustomerId)obj);
        return false;
    }
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    public override string ToString()
    {
        return Value.ToString();
    }
    public static bool TryParse(string str, out OrderId result)
    {
        result = Empty;
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        long res;
        if (long.TryParse(str, out res))
        {
            result = new OrderId(res);
            return true;
        }
        return false;
    }
    public static OrderId Parse(string str)
    {
        OrderId res;
        if (TryParse(str, out res))
            return res;
        throw new Exception("Could not parse product id");
    }
}

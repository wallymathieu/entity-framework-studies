using System.Text.Json.Serialization;
using SaitheSystemTextJson;

namespace CSharpTypes;

/// <summary>
/// Customer identifier, simple wrapper around long value. Since it wraps long we need to use the JsonConverter
/// </summary>
[JsonConverter(typeof(ValueTypeLongSystemTextJsonConverter<CustomerId>))]
public struct CustomerId : IEquatable<CustomerId>
{
    public readonly long Value;

    public CustomerId(long value)
    {
        this.Value = value;
    }

    public readonly static CustomerId Empty = new CustomerId();

    public bool Equals(CustomerId other)
    {
        if (ReferenceEquals(null, other)) return false;
        return Equals(Value, other.Value);
    }
    public override bool Equals(object obj)
    {
        if (obj is CustomerId)
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
}

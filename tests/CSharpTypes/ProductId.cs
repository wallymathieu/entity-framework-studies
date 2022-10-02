using System.ComponentModel;
using System.Text.Json.Serialization;
using Saithe;
using SaitheSystemTextJson;

namespace CSharpTypes;

[TypeConverter(typeof(ParseTypeConverter<ProductId>))]
[JsonConverter(typeof(ParseTypeSystemTextJsonConverter<ProductId>))]
public struct ProductId: IEquatable<ProductId>
{
    public readonly long Value;

    public ProductId(long value)
    {
        this.Value = value;
    }

    public readonly static ProductId Empty = new ProductId();

    public bool Equals(ProductId other)
    {
        if (ReferenceEquals(null, other)) return false;
        return Equals(Value, other.Value);
    }
    public override bool Equals(object obj)
    {
        if (obj is ProductId)
            return Equals((ProductId)obj);
        return false;
    }
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    public override string ToString()
    {
        return $"ProductId/{Value}";
    }
    public static bool TryParse(string str, out ProductId result) 
    {
        result = Empty;
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        var split = str.Split('/');
        long res;
        if (split.Length == 2 
            && split[0] == "ProductId"
            && long.TryParse(split[1], out res))
        {
            result = new ProductId(res);
            return true;
        }
        return false;
    }
    public static ProductId Parse(string str) 
    {
        ProductId res;
        if (TryParse(str, out res))
            return res;
        throw new Exception("Could not parse product id");
    }
}

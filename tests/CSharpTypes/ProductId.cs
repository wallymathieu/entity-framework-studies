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

    public ProductId(long value) => Value = value;

    public bool Equals(ProductId other) => Equals(Value, other.Value);

    public override bool Equals(object? obj) => obj is ProductId id && Equals(id);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => $"ProductId/{Value}";

    public static bool TryParse(string str, out ProductId result) 
    {
        result = default;
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

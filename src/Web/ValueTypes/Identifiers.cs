using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Saithe;
using Saithe.SystemTextJson;

namespace SomeBasicEFApp.Web.ValueTypes;

    ///
[TypeConverter(typeof(ParseTypeConverter<CustomerId>)),
 JsonConverter(typeof(ParseTypeJsonConverter<CustomerId>))]
public record struct CustomerId (int Value) : IValueType
{
    ///
    public override string ToString()=>$"customer-{Value}";
    ///
    public static CustomerId Parse(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Missing value");
        if (!value.StartsWith("customer-", StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException($"Expected '{value}' to start with prefix 'customer-'");
        return new CustomerId(Int32.TryParse(value.Substring("customer-".Length), out var val)
                ? val
                : throw new ArgumentException());
    }
    ///
    public static implicit operator CustomerId(int d) => new CustomerId(d);
}
    ///
[TypeConverter(typeof(ParseTypeConverter<OrderId>)),
 JsonConverter(typeof(ParseTypeJsonConverter<OrderId>))]
public record struct OrderId (int Value) : IValueType
{
    ///
    public override string ToString()=>$"order-{Value}";
    ///
    public static OrderId Parse(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Missing value");
        if (!value.StartsWith("order-", StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException($"Expected '{value}' to start with prefix 'order-'");
        return new OrderId(Int32.TryParse(value.Substring("order-".Length), out var val)
                ? val
                : throw new ArgumentException());
    }
    ///
    public static implicit operator OrderId(int d) => new OrderId(d);
}
    ///
[TypeConverter(typeof(ParseTypeConverter<ProductId>)),
 JsonConverter(typeof(ParseTypeJsonConverter<ProductId>))]
public record struct ProductId (int Value) : IValueType
{
    ///
    public override string ToString()=>$"product-{Value}";
    ///
    public static ProductId Parse(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Missing value");
        if (!value.StartsWith("product-", StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException($"Expected '{value}' to start with prefix 'product-'");
        return new ProductId(Int32.TryParse(value.Substring("product-".Length), out var val)
                ? val
                : throw new ArgumentException());
    }
    ///
    public static implicit operator ProductId(int d) => new ProductId(d);
}

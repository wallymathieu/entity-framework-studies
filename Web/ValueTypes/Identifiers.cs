
using System;
using System.ComponentModel;
using Saithe;

namespace SomeBasicEFApp.Web
{

    [TypeConverter(typeof(ParseTypeConverter<CustomerId>))]
    public struct CustomerId : IEquatable<CustomerId>, IId
    {
        public int Value { get; }

        public CustomerId(int value)=>Value = value;

        public bool Equals(CustomerId other) => Equals(Value, other.Value);

        public override bool Equals(object obj) => obj is CustomerId id && Equals(id);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => $"C-{Value}";

        public static CustomerId Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Missing value");
            if (!value.StartsWith("C-", StringComparison.InvariantCultureIgnoreCase))
                throw new FormatException($"Expected '{value}' to start with prefix 'C-'");
            if (Int32.TryParse(value.Substring("C-".Length), out var result))
                return new CustomerId(result);
            throw new FormatException($"Expected '{value}' to match 'C--\\d+'");
        }
        public static bool operator ==(CustomerId a, CustomerId b) => a.Equals(b);
        public static bool operator !=(CustomerId a, CustomerId b) => !a.Equals(b);
    }
    [TypeConverter(typeof(ParseTypeConverter<OrderId>))]
    public struct OrderId : IEquatable<OrderId>, IId
    {
        public int Value { get; }

        public OrderId(int value)=>Value = value;

        public bool Equals(OrderId other) => Equals(Value, other.Value);

        public override bool Equals(object obj) => obj is OrderId id && Equals(id);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => $"O-{Value}";

        public static OrderId Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Missing value");
            if (!value.StartsWith("O-", StringComparison.InvariantCultureIgnoreCase))
                throw new FormatException($"Expected '{value}' to start with prefix 'O-'");
            if (Int32.TryParse(value.Substring("O-".Length), out var result))
                return new OrderId(result);
            throw new FormatException($"Expected '{value}' to match 'O--\\d+'");
        }
        public static bool operator ==(OrderId a, OrderId b) => a.Equals(b);
        public static bool operator !=(OrderId a, OrderId b) => !a.Equals(b);
    }
    [TypeConverter(typeof(ParseTypeConverter<ProductId>))]
    public struct ProductId : IEquatable<ProductId>, IId
    {
        public int Value { get; }

        public ProductId(int value)=>Value = value;

        public bool Equals(ProductId other) => Equals(Value, other.Value);

        public override bool Equals(object obj) => obj is ProductId id && Equals(id);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => $"P-{Value}";

        public static ProductId Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Missing value");
            if (!value.StartsWith("P-", StringComparison.InvariantCultureIgnoreCase))
                throw new FormatException($"Expected '{value}' to start with prefix 'P-'");
            if (Int32.TryParse(value.Substring("P-".Length), out var result))
                return new ProductId(result);
            throw new FormatException($"Expected '{value}' to match 'P--\\d+'");
        }
        public static bool operator ==(ProductId a, ProductId b) => a.Equals(b);
        public static bool operator !=(ProductId a, ProductId b) => !a.Equals(b);
    }
}

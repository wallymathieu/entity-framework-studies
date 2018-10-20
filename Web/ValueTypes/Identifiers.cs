﻿using System;
using System.ComponentModel;
using Saithe;

namespace SomeBasicEFApp.Web
{

    [TypeConverter(typeof(ParseTypeConverter<CustomerId>))]
    public struct CustomerId : IEquatable<CustomerId>
    {
        public readonly string Value;

        public CustomerId(string value)=>Value = value;

        public readonly static CustomerId Empty = new CustomerId();

        public bool Equals(CustomerId other) => Equals(Value, other.Value);

        public override bool Equals(object obj) => obj is CustomerId id && Equals(id);
        public override int GetHashCode() => Value?.GetHashCode() ?? 0;
        public override string ToString() => $"C-{Value}";

        public static CustomerId Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Missing value");
            if (!value.StartsWith("C-", StringComparison.InvariantCultureIgnoreCase))
                throw new FormatException($"Expected '{value}' to start with prefix 'C-'");
            return new CustomerId(value.Substring("C-".Length));
        }
        public static bool operator ==(CustomerId a, CustomerId b) => a.Equals(b);
        public static bool operator !=(CustomerId a, CustomerId b) => !a.Equals(b);
    }
    [TypeConverter(typeof(ParseTypeConverter<OrderId>))]
    public struct OrderId : IEquatable<OrderId>
    {
        public readonly string Value;

        public OrderId(string value)=>Value = value;

        public readonly static OrderId Empty = new OrderId();

        public bool Equals(OrderId other) => Equals(Value, other.Value);

        public override bool Equals(object obj) => obj is OrderId id && Equals(id);
        public override int GetHashCode() => Value?.GetHashCode() ?? 0;
        public override string ToString() => $"O-{Value}";

        public static OrderId Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Missing value");
            if (!value.StartsWith("O-", StringComparison.InvariantCultureIgnoreCase))
                throw new FormatException($"Expected '{value}' to start with prefix 'O-'");
            return new OrderId(value.Substring("O-".Length));
        }
        public static bool operator ==(OrderId a, OrderId b) => a.Equals(b);
        public static bool operator !=(OrderId a, OrderId b) => !a.Equals(b);
    }
    [TypeConverter(typeof(ParseTypeConverter<ProductId>))]
    public struct ProductId : IEquatable<ProductId>
    {
        public readonly string Value;

        public ProductId(string value)=>Value = value;

        public readonly static ProductId Empty = new ProductId();

        public bool Equals(ProductId other) => Equals(Value, other.Value);

        public override bool Equals(object obj) => obj is ProductId id && Equals(id);
        public override int GetHashCode() => Value?.GetHashCode() ?? 0;
        public override string ToString() => $"P-{Value}";

        public static ProductId Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Missing value");
            if (!value.StartsWith("P-", StringComparison.InvariantCultureIgnoreCase))
                throw new FormatException($"Expected '{value}' to start with prefix 'P-'");
            return new ProductId(value.Substring("P-".Length));
        }
        public static bool operator ==(ProductId a, ProductId b) => a.Equals(b);
        public static bool operator !=(ProductId a, ProductId b) => !a.Equals(b);
    }
}

﻿using System;
using System.ComponentModel;
using Saithe;

namespace SomeBasicEFApp.Web
{

    [TypeConverter(typeof(ParseTypeConverter<CustomerId>))]
    public struct CustomerId : IEquatable<CustomerId>
    {
        public readonly string Value;

        public CustomerId(string value)
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
            return Value != null
                ? Value.GetHashCode()
                : 0;
        }
        public override string ToString()
        {
            return $"C-{Value}";
        }
        public static CustomerId Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Missing value");
            if (!value.StartsWith("C-", StringComparison.InvariantCultureIgnoreCase))
                throw new FormatException($"Expected '{value}' to start with prefix 'C-'");
            return new CustomerId(value.Substring("C-".Length));
        }
        public static bool operator ==(CustomerId a, CustomerId b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(CustomerId a, CustomerId b)
        {
            return !(a == b);
        }
    }
    [TypeConverter(typeof(ParseTypeConverter<OrderId>))]
    public struct OrderId : IEquatable<OrderId>
    {
        public readonly string Value;

        public OrderId(string value)
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
            if (obj is CustomerId)
                return Equals((OrderId)obj);
            return false;
        }
        public override int GetHashCode()
        {
            return Value != null
                ? Value.GetHashCode()
                : 0;
        }
        public override string ToString()
        {
            return $"O-{Value}";
        }
        public static OrderId Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Missing value");
            if (!value.StartsWith("O-", StringComparison.InvariantCultureIgnoreCase))
                throw new FormatException($"Expected '{value}' to start with prefix 'O-'");
            return new OrderId(value.Substring("O-".Length));
        }
        public static bool operator ==(OrderId a, OrderId b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(OrderId a, OrderId b)
        {
            return !(a == b);
        }
    }
    [TypeConverter(typeof(ParseTypeConverter<ProductId>))]
    public struct ProductId : IEquatable<ProductId>
    {
        public readonly string Value;

        public ProductId(string value)
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
            if (obj is CustomerId)
                return Equals((ProductId)obj);
            return false;
        }
        public override int GetHashCode()
        {
            return Value != null
                ? Value.GetHashCode()
                : 0;
        }
        public override string ToString()
        {
            return $"P-{Value}";
        }
        public static ProductId Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Missing value");
            if (!value.StartsWith("P-", StringComparison.InvariantCultureIgnoreCase))
                throw new FormatException($"Expected '{value}' to start with prefix 'P-'");
            return new ProductId(value.Substring("P-".Length));
        }
        public static bool operator ==(ProductId a, ProductId b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(ProductId a, ProductId b)
        {
            return !(a == b);
        }
    }
}

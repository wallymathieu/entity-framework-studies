using System;
using System.ComponentModel;
using Saithe;

namespace SomeBasicEFApp.Web.ValueTypes
{

    [TypeConverter(typeof(ParseTypeConverter<CustomerId>))]
    public struct CustomerId : IEquatable<CustomerId>, IComparable<CustomerId>
    {
        public readonly int Value;

        public CustomerId(int value)
        {
            this.Value = value;
        }

        public readonly static CustomerId Empty = new CustomerId();

        public bool Equals(CustomerId other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Equals(Value, other.Value);
        }
        public override bool Equals(object obj)=>obj is CustomerId v && Equals(v);
        public override int GetHashCode()=>Value.GetHashCode();
        public override string ToString()=>$"customer-{Value}";
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
        public static bool operator ==(CustomerId a, CustomerId b)=>a.Equals(b);
        public static bool operator !=(CustomerId a, CustomerId b)=>!a.Equals(b);
        public static implicit operator CustomerId(int d) => new CustomerId(d);
        public int CompareTo(CustomerId other) => this.Value.CompareTo(other.Value);
    }
    [TypeConverter(typeof(ParseTypeConverter<OrderId>))]
    public struct OrderId : IEquatable<OrderId>, IComparable<OrderId>
    {
        public readonly int Value;

        public OrderId(int value)
        {
            this.Value = value;
        }

        public readonly static OrderId Empty = new OrderId();

        public bool Equals(OrderId other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Equals(Value, other.Value);
        }
        public override bool Equals(object obj)=>obj is OrderId v && Equals(v);
        public override int GetHashCode()=>Value.GetHashCode();
        public override string ToString()=>$"order-{Value}";
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
        public static bool operator ==(OrderId a, OrderId b)=>a.Equals(b);
        public static bool operator !=(OrderId a, OrderId b)=>!a.Equals(b);
        public static implicit operator OrderId(int d) => new OrderId(d);
        public int CompareTo(OrderId other) => this.Value.CompareTo(other.Value);
    }
    [TypeConverter(typeof(ParseTypeConverter<ProductId>))]
    public struct ProductId : IEquatable<ProductId>, IComparable<ProductId>
    {
        public readonly int Value;

        public ProductId(int value)
        {
            this.Value = value;
        }

        public readonly static ProductId Empty = new ProductId();

        public bool Equals(ProductId other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Equals(Value, other.Value);
        }
        public override bool Equals(object obj)=>obj is ProductId v && Equals(v);
        public override int GetHashCode()=>Value.GetHashCode();
        public override string ToString()=>$"product-{Value}";
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
        public static bool operator ==(ProductId a, ProductId b)=>a.Equals(b);
        public static bool operator !=(ProductId a, ProductId b)=>!a.Equals(b);
        public static implicit operator ProductId(int d) => new ProductId(d);
        public int CompareTo(ProductId other) => this.Value.CompareTo(other.Value);
    }
}

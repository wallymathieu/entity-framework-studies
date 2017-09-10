using System;
using System.ComponentModel;
using Saithe;

namespace SomeBasicEFApp.Web.ValueTypes
{
    [TypeConverter(typeof(ValueTypeConverter<OrderId>))]
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
            if (obj is OrderId)
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
            return Value != null
                ? $"o-{Value}"
                : string.Empty;
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
}
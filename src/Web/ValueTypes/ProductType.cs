using System;

namespace SomeBasicEFApp.Web.ValueTypes
{
    public class ProductType:IEquatable<ProductType>,IValueType
    {
        public string Type { get; }

        public ProductType(string type) => Type = type;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProductType) obj);
        }

        public override int GetHashCode()
        {
            return (Type != null ? Type.GetHashCode() : 0);
        }

        public override string ToString() => Type;

        public bool Equals(ProductType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type;
        }

        public static bool operator ==(ProductType a, ProductType b) => a.Equals(b);

        public static bool operator !=(ProductType a, ProductType b) => !(a == b);
    }

    public interface IValueType
    {
    }
}

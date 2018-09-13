namespace Web.ValueTypes
{
    public struct ProductId
    {
        public readonly int Value;

        public ProductId(int value)
        {
            Value = value;
        }

        public override bool Equals(object obj) => obj is ProductId p && Value == p.Value;

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => "product-" + Value;
    }
}

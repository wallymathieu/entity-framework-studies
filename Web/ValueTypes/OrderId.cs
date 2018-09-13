namespace Web.ValueTypes
{
    public struct OrderId
    {
        public readonly int Value;

        public OrderId(int value)
        {
            Value = value;
        }

        public override bool Equals(object obj) => obj is OrderId p && Value == p.Value;

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => "order-" + Value;
    }
    public struct CustomerId
    {
        public readonly int Value;

        public CustomerId(int value)
        {
            Value = value;
        }

        public override bool Equals(object obj) => obj is CustomerId p && Value == p.Value;

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => "customer-" + Value;
    }
}

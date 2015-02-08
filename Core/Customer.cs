namespace SomeBasicEFApp.Core
{
    public class Customer : IIdentifiableByNumber
    {
        public virtual int Id { get; set; }

        public virtual string Firstname { get; set; }

        public virtual string Lastname { get; set; }

        public virtual System.Collections.Generic.IList<Order> Orders { get; set; }

        public virtual int Version { get; set; }

    }
}

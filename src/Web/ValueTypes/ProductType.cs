using System;

namespace SomeBasicEFApp.Web.ValueTypes
{
    /// <summary>
    /// Product type
    /// </summary>
    public record ProductType(string Type)
    {
        /// <inheritdoc/>
        public override string ToString() => Type;
    }
}

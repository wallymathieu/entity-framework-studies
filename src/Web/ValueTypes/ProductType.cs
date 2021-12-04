namespace SomeBasicEFApp.Web.ValueTypes
{
    /// <summary>
    /// Product type
    /// </summary>
    public record ProductType(string? Type): IValueType
    {
        /// <inheritdoc/>
        public override string? ToString() => Type;
    }
}

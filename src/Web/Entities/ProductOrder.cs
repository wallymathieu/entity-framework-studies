namespace SomeBasicEFApp.Web.Entities;

///
public record ProductOrder
{
    ///
    public virtual int Id { get; init; }
    ///
#pragma warning disable CS8618
    public virtual Order Order { get; init; }
    ///
    public virtual Product Product { get; init; }
#pragma warning restore CS8618
}

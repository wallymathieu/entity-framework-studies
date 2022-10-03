namespace SomeBasicEFApp.Web.Entities;

///
public class ProductOrder
{
    ///
    public virtual int Id { get; init; }
    ///
    public virtual Order? Order { get; init; }
    ///
    public virtual Product? Product { get; init; }
}

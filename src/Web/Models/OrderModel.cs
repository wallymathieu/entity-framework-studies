namespace SomeBasicEFApp.Web.Models
{
    public record OrderModel(string Id, ProductModel[] Products, CustomerModel? Customer);
}

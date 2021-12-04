namespace SomeBasicEFApp.Web.Models
{
    public class OrderModel
    {
        public string Id { get; init; }
        public ProductModel[] Products { get; init; }
        public CustomerModel Customer { get; init; }
    }
}

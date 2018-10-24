namespace SomeBasicEFApp.Web.Models
{
    public class OrderModel
    {
        public string Id { get; set; }
        public ProductModel[] Products { get; set; }
        public CustomerModel Customer { get; set; }
    }
}

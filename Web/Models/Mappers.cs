using System.Linq;
using SomeBasicEFApp.Web.Entities;

namespace SomeBasicEFApp.Web.Models
{
    public class Mappers
    {
        public static ProductModel Map(Product arg) => new ProductModel
        {
            Cost = arg.Cost,
            Name = arg.Name,
            Id =arg.GetId().ToString()
        };

        public static OrderModel Map(Order arg) => new OrderModel
        {
            Id = arg.GetId().ToString(),
            Products = arg.ProductOrders?.Select(po => Map(po.Product)).ToArray(),
            Customer = arg.Customer!=null? Map(arg.Customer):null
        };
        public static CustomerModel Map(Customer arg)=>new CustomerModel
        {
            Id=arg.GetId().ToString(),
            Firstname=arg.Firstname,
            Lastname=arg.Lastname
        };
    }
}

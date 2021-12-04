using System.Linq;
using SomeBasicEFApp.Web.Entities;

namespace SomeBasicEFApp.Web.Models
{
    public class Mappers
    {
        public static ProductModel Map(Product arg) => new(
            Cost: arg.Cost,
            Name: arg.Name,
            Id:arg.Id.ToString()
        );

        public static OrderModel Map(Order arg) => new(
            Products: arg.ProductOrders?.Select(po => Map(po.Product)).ToArray(),
            Customer: arg.Customer!=null? Map(arg.Customer):null,
            Id: arg.Id.ToString()
        );
        public static CustomerModel Map(Customer arg)=>new(
            Firstname:arg.Firstname,
            Lastname:arg.Lastname,
            Id:arg.Id.ToString()
        );
    }
}

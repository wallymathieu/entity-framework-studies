using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SomeBasicEFApp.Web;

namespace SomeBasicEFApp.Web.Entities
{
    public class Product 
    {
        public virtual float Cost { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<ProductOrder> ProductOrders { get; set; }
        [Key]
        public virtual ProductId Id { get; set; }

        public virtual int Version { get; set; }
    }
}

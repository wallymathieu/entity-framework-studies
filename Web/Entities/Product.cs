using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SomeBasicEFApp.Web;

namespace SomeBasicEFApp.Web.Entities
{
    public class Product 
    {
        public virtual float Cost { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<ProductOrder> ProductOrders { get; set; }
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual ProductId Id { get; set; }

        public virtual int Version { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace SomeBasicEFApp.Web.Entities
{
    public class ProductOrder
    {
        public virtual OrderId OrderId { get; set; }
        public virtual ProductId ProductId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}

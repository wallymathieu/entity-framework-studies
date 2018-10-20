using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SomeBasicEFApp.Web.Entities
{
    public class Order
    {
        public virtual CustomerId CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public virtual DateTime OrderDate { get; set; }
        [Key]
        public virtual OrderId Id { get; set; }

        public virtual int Version { get; set; }

        public virtual IList<ProductOrder> ProductOrders { get; set; }

    }
}

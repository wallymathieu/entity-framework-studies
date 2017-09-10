using System;
using System.Collections.Generic;
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Web.Entities
{
    public class Order
    {
        public virtual Customer Customer { get; set; }

        public virtual DateTime OrderDate { get; set; }

        public virtual OrderId Id { get; set; }

        public virtual int Version { get; set; }

		public virtual IList<ProductOrder> ProductOrders { get; set; }

	}
}

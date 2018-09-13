﻿using System;
using System.Collections.Generic;
using Web.ValueTypes;

namespace SomeBasicEFApp.Web.Entities
{
    public class Order : IIdentifiableByNumber
    {
        public virtual Customer Customer { get; set; }

        public virtual DateTime OrderDate { get; set; }

        public virtual int Id { get; set; }

        public virtual int Version { get; set; }

		public virtual IList<ProductOrder> ProductOrders { get; set; }

        public OrderId GetId() => new OrderId(Id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeBasicEFApp.Core
{
	public class ProductOrder : IIdentifiableByNumber
	{
		public virtual int Id { get; set; }

		public virtual Order Order { get; set; }

		public virtual Product Product { get; set; }
	}
}

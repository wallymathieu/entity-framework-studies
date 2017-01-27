namespace SomeBasicEFApp.Web.Entities
{
    public class ProductOrder : IIdentifiableByNumber
	{
		public virtual int Id { get; set; }

		public virtual Order Order { get; set; }

		public virtual Product Product { get; set; }
	}
}

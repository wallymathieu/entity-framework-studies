namespace SomeBasicEFApp.Core
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Data.Entity.ModelConfiguration.Conventions;

	public partial class CoreDbContext : DbContext
	{
		public CoreDbContext(string connString = null)
			: base(connString ?? "name=DataModel")
		{
		}
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}

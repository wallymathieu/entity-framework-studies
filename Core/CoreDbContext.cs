namespace SomeBasicEFApp.Core
{
	using System.Data.Common;
	using System.Data.Entity;
	using System.Data.Entity.ModelConfiguration.Conventions;
	//public class SQLiteConfiguration : DbConfiguration
	//{
	//	public SQLiteConfiguration()
	//	{
	//		SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
	//		SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
	//		Type t = Type.GetType(
	//				   "System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6");
	//		FieldInfo fi = t.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Static);
	//		SetProviderServices("System.Data.SQLite", (DbProviderServices)fi.GetValue(null));
	//	}
	//}
	public partial class CoreDbContext : DbContext
	{

		public CoreDbContext(DbConnection connString)
			: base(connString, true)
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

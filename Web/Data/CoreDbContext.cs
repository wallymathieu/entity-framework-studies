using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.DependencyInjection;
using SomeBasicEFApp.Web.Entities;

namespace SomeBasicEFApp.Web.Data
{
    public class CoreDbCoreConventionSetBuilder : CoreConventionSetBuilder
    {
        public CoreDbCoreConventionSetBuilder(CoreConventionSetBuilderDependencies dependencies) : base(dependencies)
        {
        }
        public override Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet CreateConventionSet()
        {
            var @set = base.CreateConventionSet();
            return @set;
        }
    }

    public partial class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options) {}
        public CoreDbContext() {}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductOrder>().HasKey(b => new { b.OrderId, b.ProductId });
        }


        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }

        public Customer GetCustomer(CustomerId v)
        {
            return Customers.SingleOrDefault(c => c.Id == v);
        }
        public Product GetProduct(ProductId v)
        {
            return Products.SingleOrDefault(p => p.Id == v);
        }
        public Order GetOrder(OrderId v)
        {
            return Orders.SingleOrDefault(o => o.Id == v);
        }
    }
}

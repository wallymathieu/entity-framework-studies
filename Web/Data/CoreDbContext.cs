using System.Linq;
using Microsoft.EntityFrameworkCore;
using SomeBasicEFApp.Web.Entities;
namespace SomeBasicEFApp.Web.Data
{
    public partial class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions options) : base(options)
        {
        }
        public CoreDbContext()
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }

        public Customer GetCustomer(int v)
        {
            return Customers.SingleOrDefault(c => c.Id == v);
        }
        public Product GetProduct(int v)
        {
            return Products.SingleOrDefault(p => p.Id == v);
        }
        public Order GetOrder(int v)
        {
            return Orders.SingleOrDefault(o => o.Id == v);
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SomeBasicEFApp.Web.Entities;

namespace SomeBasicEFApp.Web.Data
{
    public class CoreDbContext : IdentityDbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options) {}
        public CoreDbContext() {}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductOrder>().HasKey(b => new { b.OrderId, b.ProductId });
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }

        public Customer GetCustomer(CustomerId v) => 
            Customers.SingleOrDefault(c => c.Id == v);

        public Product GetProduct(ProductId v) => 
            Products.SingleOrDefault(p => p.Id == v);

        public Order GetOrder(OrderId v) => 
            Orders.SingleOrDefault(o => o.Id == v);
        public Task<Customer> GetCustomerAsync(CustomerId v) => 
            Customers.SingleOrDefaultAsync(c => c.Id == v);

        public Task<Product> GetProductAsync(ProductId v) => 
            Products.SingleOrDefaultAsync(p => p.Id == v);

        public Task<Order> GetOrderAsync(OrderId v) => 
            Orders.SingleOrDefaultAsync(o => o.Id == v);

    }
}

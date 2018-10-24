using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SomeBasicEFApp.Web.Entities;
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Web.Data
{
    public partial class CoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public CoreDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }

        public Customer GetCustomer(CustomerId v) => 
            Customers.SingleOrDefault(c => c.Id == v.Value);

        public Product GetProduct(ProductId v) => 
            Products.SingleOrDefault(p => p.Id == v.Value);

        public Order GetOrder(OrderId v) => 
            Orders.SingleOrDefault(o => o.Id == v.Value);
        public Task<Customer> GetCustomerAsync(CustomerId v) => 
            Customers.SingleOrDefaultAsync(c => c.Id == v.Value);

        public Task<Product> GetProductAsync(ProductId v) => 
            Products.SingleOrDefaultAsync(p => p.Id == v.Value);

        public Task<Order> GetOrderAsync(OrderId v) => 
            Orders.SingleOrDefaultAsync(o => o.Id == v.Value);

    }
}

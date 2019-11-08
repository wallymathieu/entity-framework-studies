using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SomeBasicEFApp.Web.Entities;
using SomeBasicEFApp.Web.ValueTypes;
using FSharpPlusCSharp;
using Microsoft.FSharp.Core;

namespace SomeBasicEFApp.Web.Data
{
    public class CoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public CoreDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
                .Property(o => o.Id).HasConversion(new ValueConverter<ProductId, int>(v => v.Value, v => new ProductId(v)));
            builder.Entity<Customer>()
                .Property(o => o.Id).HasConversion(new ValueConverter<CustomerId, int>(v => v.Value, v => new CustomerId(v)));
            builder.Entity<Order>()
                .Property(o => o.Id).HasConversion(new ValueConverter<OrderId, int>(v => v.Value, v => new OrderId(v)));
            base.OnModelCreating(builder);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }

        public FSharpOption<Customer> GetCustomer(CustomerId v) => 
            Customers.SingleOrDefault(c => c.Id == v).ToOption();

        public FSharpOption<Product> GetProduct(ProductId v) => 
            Products.SingleOrDefault(p => p.Id == v).ToOption();

        public FSharpOption<Order> GetOrder(OrderId v) => 
            Orders.SingleOrDefault(o => o.Id == v).ToOption();
        public async Task<FSharpOption<Customer>> GetCustomerAsync(CustomerId v) => 
            (await Customers.SingleOrDefaultAsync(c => c.Id == v)).ToOption();

        public async Task<FSharpOption<Product>> GetProductAsync(ProductId v) => 
            (await Products.SingleOrDefaultAsync(p => p.Id == v)).ToOption();

        public async Task<FSharpOption<Order>> GetOrderAsync(OrderId v) => 
            (await Orders.SingleOrDefaultAsync(o => o.Id == v)).ToOption();

    }
}

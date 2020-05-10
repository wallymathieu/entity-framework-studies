using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SomeBasicEFApp.Web.Entities;
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Web.Data
{
    public class CoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public CoreDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>(entity =>
            {
                entity.Property(o => o.Id)
                    .HasConversion(new ValueConverter<ProductId, int>(v => v.Value, v => new ProductId(v)));
                entity
                    .OwnsOne(o => o.Type,
                        t=>t.Property(pt=>pt.Type).HasColumnName("ProductType"))
                    .UsePropertyAccessMode(PropertyAccessMode.Field);
                
            });
            builder.Entity<Customer>(entity =>
            {
                entity.Property(o => o.Id)
                    .HasConversion(new ValueConverter<CustomerId, int>(v => v.Value, v => new CustomerId(v)));
            });
            builder.Entity<Order>(entity =>
            {
                entity.Property(o => o.Id)
                    .HasConversion(new ValueConverter<OrderId, int>(v => v.Value, v => new OrderId(v)));
            });
            base.OnModelCreating(builder);
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

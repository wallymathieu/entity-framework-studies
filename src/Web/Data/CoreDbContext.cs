using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SomeBasicEFApp.Web.Entities;
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Web.Data;

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

    public DbSet<Customer> Customers { get; init; }
    public DbSet<Order> Orders { get; init;  }
    public DbSet<Product> Products { get; init; }

    public DbSet<ProductOrder> ProductOrders { get; init; }

    public Customer? GetCustomer(CustomerId customerId) =>
        Customers.SingleOrDefault(customer => customer.Id == customerId);

    public Product? GetProduct(ProductId productId) =>
        Products.SingleOrDefault(product => product.Id == productId);

    public Order? GetOrder(OrderId orderId) =>
        Orders.SingleOrDefault(order => order.Id == orderId);
    public async Task<Customer?> GetCustomerAsync(CustomerId customerId) =>
        await Customers.SingleOrDefaultAsync(customer => customer.Id == customerId);

    public async Task<Product?> GetProductAsync(ProductId productId) =>
        await Products.SingleOrDefaultAsync(product => product.Id == productId);

    public async Task<Order?> GetOrderAsync(OrderId orderId) =>
        await Orders.SingleOrDefaultAsync(order => order.Id == orderId);

}

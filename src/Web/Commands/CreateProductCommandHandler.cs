using System.Threading.Tasks;
using SomeBasicEFApp.Web.Data;
using SomeBasicEFApp.Web.Entities;

namespace SomeBasicEFApp.Web.Commands;

public class CreateProductCommandHandler
{
    private readonly CoreDbContext _context;

    public CreateProductCommandHandler(CoreDbContext context) => _context = context;

    public async Task<Product> Handle(CreateProductCommand product)
    {
        // additional logic around creating products can be added here
        var entity = new Product {Cost = product.Cost, Name = product.Name};
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Used to be compatible with previous controller logic
    /// </summary>
    public Task<Product> Handle(Product product) => Handle(new CreateProductCommand
    (
        Name: product.Name,
        Cost: product.Cost
    ));
}

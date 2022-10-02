using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomeBasicEFApp.Web.Commands;
using SomeBasicEFApp.Web.Data;
using SomeBasicEFApp.Web.Entities;
using SomeBasicEFApp.Web.Models;
using SomeBasicEFApp.Web.ValueTypes;

namespace SomeBasicEFApp.Web.Controllers.v1
{
    [Route("/api/v1/products")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductsController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public ProductsController(CoreDbContext context) => _context = context;

        [HttpGet("")]
        [Produces(typeof(Product[]))]
        public IActionResult Index()
        {// here you normally want filtering based on query parameters (in order to get better perf)
            return Ok(
                _context
                    .Products
                    .ToArray());
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void),StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType()]
        public async Task<IActionResult> Details(ProductId? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost("")]
        [Produces(typeof(Product))]
        public async Task<IActionResult> Post(EditProductModel model)
        {
            var handler = new CreateProductCommandHandler(_context);
            var product = await handler.Handle(new CreateProductCommand(
                Name: model.Name,
                Cost: model.Cost
            ));
            return Ok(product);
        }

    }
}

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SomeBasicEFApp.Web.Commands;
using SomeBasicEFApp.Web.Data;
using SomeBasicEFApp.Web.Entities;
using SomeBasicEFApp.Web.Models;

namespace SomeBasicEFApp.Web.Controllers.Api
{
    [Route("/api/v1/products")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductsController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public ProductsController(CoreDbContext context) => _context = context;

        [HttpGet("")]
        [Produces(typeof(ProductModel[]))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorModel))]
        public IActionResult Index()
        {// here you normally want filtering based on query parameters (in order to get better perf)
            return Ok(
                _context
                    .Products
                    .Select(Mappers.Map)
                    .ToArray());
        }

        [HttpPost("")]
        [Produces(typeof(ProductModel))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorModel))]
        public async Task<IActionResult> Post([FromBody]ProductModel model)
        {// here you normally want filtering based on query parameters (in order to get better perf)
            var handler = new CreateProductCommandHandler(_context);
            var product = await handler.Handle(new CreateProductCommand
            {
                Name = model.Name,
                Cost = model.Cost
            });
            return Ok(product);
        }
        
    }
}

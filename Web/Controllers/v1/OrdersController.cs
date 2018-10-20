using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomeBasicEFApp.Web.Data;
using SomeBasicEFApp.Web.Models;

namespace SomeBasicEFApp.Web.Controllers.Api
{
    [Route("/api/v1/orders")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class OrdersController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public OrdersController(CoreDbContext context) => _context = context;

        [HttpGet("")]
        [Produces(typeof(OrderModel[]))]
        [ProducesResponseType(statusCode: 400, type: typeof(ErrorModel))]
        public IActionResult Index()
        {// here you normally want filtering based on query parameters (in order to get better perf)
            return Ok(
                _context
                    .Orders
                        .Include(o=>o.Customer)
                        .Include(o=>o.ProductOrders)
                        .ThenInclude(po=>po.Product)
                    .Select(Mappers.Map)
                    .ToArray());
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomeBasicEFApp.Web.Data;
using SomeBasicEFApp.Web.Entities;
using SomeBasicEFApp.Web.Models;
using SomeBasicEFApp.Web.ValueTypes;
using Microsoft.AspNetCore.Http;

namespace SomeBasicEFApp.Web.Controllers.v1
{
    [Route("/api/v1/customers")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CustomersController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public CustomersController(CoreDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        [HttpGet]
        [Produces(typeof(CustomerModel[]))]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Customers.Select(c=>Mappers.Map(c)).ToListAsync());
        }

        // GET: Customers/Details/5
        [HttpGet("{id}")]
        [Produces(typeof(CustomerModel))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Details(CustomerId? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var customer = await _context.Customers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(Mappers.Map(customer));
        }

        [HttpPost("")]
        [Produces(typeof(CustomerModel))]
        public async Task<IActionResult> Create([FromBody] EditCustomer model)
        {
            var customer = new Customer {Firstname = model.Firstname, Lastname = model.Lastname};
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return Ok(Mappers.Map(customer));
        }

        [HttpPut("{id}")]
        [Produces(typeof(CustomerModel))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Edit(CustomerId id, [FromBody] EditCustomer model)
        {
            var customer = await _context.GetCustomerAsync(id);
            if (customer == null) return NotFound();
            customer.Firstname = model.Firstname;
            customer.Lastname = model.Lastname;
            await _context.SaveChangesAsync();
            return Ok(Mappers.Map(customer));
        }


        // POST: Customers/Delete/5
        [HttpDelete("{id}")]
        [Produces(typeof(void))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteConfirmed(CustomerId id)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(m => m.Id == id);
            if (customer is null) return NotFound();
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

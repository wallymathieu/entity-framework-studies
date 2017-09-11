using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomeBasicEFApp.Web.Data;
using SomeBasicEFApp.Web.Entities;
using SomeBasicEFApp.Web;

namespace Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly CoreDbContext _context;

        public CustomersController(CoreDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        [HttpGet]
        [Produces(typeof(Customer[]))]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        [HttpGet("{id}")]
        [Produces(typeof(Customer))]
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

            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Firstname,Lastname")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerId id, [Bind("Firstname,Lastname")] Customer customer)
        {
            customer.Id = id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return Ok(customer);
        }


        // POST: Customers/Delete/5
        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(CustomerId id)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(m => m.Id == id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CustomerExists(CustomerId id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}

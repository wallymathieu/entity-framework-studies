using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using SomeBasicEFApp.Core;
using Microsoft.Data.OData;
using SomeBasicNHApp.Code;
using System.Web;
using SomeBasicEFApp.Code;

namespace SomeBasicNHApp.Controllers
{
    public class CustomersController : ODataController, IDisposable
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();
        private CoreDbContext _session;
        private CoreDbContext session { get { return _session ?? (_session = new Session(new WebMapPath()).CreateWebSessionFactory()); } }
        public CustomersController()
        {
        }

        // GET: odata/Customers
        public IHttpActionResult GetCustomers(ODataQueryOptions<Customer> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok<IEnumerable<Customer>>(queryOptions.ApplyTo(session.Customers.AsQueryable()) as IQueryable<Customer>);
        }

        // GET: odata/Customers(5)
        public IHttpActionResult GetCustomer([FromODataUri] int key, ODataQueryOptions<Customer> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok<Customer>(session.Customers.SingleOrDefault(c => c.Id == key));
        }

        // PUT: odata/Customers(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Customer> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Put(customer);

            // TODO: Save the patched entity.

            // return Updated(customer);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // POST: odata/Customers
        public IHttpActionResult Post(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(customer);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/Customers(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Customer> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(customer);

            // TODO: Save the patched entity.

            // return Updated(customer);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/Customers(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        protected override void Dispose(bool disposing)
        {
            if (_session != null)
            {
                _session.Dispose();
                _session = null;
            }
            base.Dispose(disposing);
        }
    }
}

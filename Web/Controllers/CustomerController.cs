using System.Linq;
using System.Web.Mvc;
using SomeBasicEFApp.Code;
using SomeBasicEFApp.Core;

namespace SomeBasicEFApp.Controllers
{
	public class CustomerController : SessionController
	{
		//
		// GET: /Context/

		public ActionResult Index()
		{
			var contexts = from context in Session.Customers
						   select context;
			return View(contexts.ToArray());
		}
		public ActionResult Get(int id)
		{
			return View(Session.Customers.SingleOrDefault(c => c.Id == id));
		}
		public ActionResult Create()
		{
			return View();
		}
		[ActionName("Create"), AcceptVerbs(HttpVerbs.Post)]
		public ActionResult CreatePost(Customer context)
		{
			Session.Customers.Add(context);
			Session.SaveChanges();
			return Redirect("/Customer");
		}
	}
}

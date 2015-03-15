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
			var contexts = from context in EFSession.Customers
						   select context;
			return View(contexts.ToArray());
		}
		public ActionResult Get(int id)
		{
            return View(EFSession.Customers.SingleOrDefault(c => c.Id == id));
		}
		public ActionResult Create()
		{
			return View();
		}
		[ActionName("Create"), AcceptVerbs(HttpVerbs.Post)]
		public ActionResult CreatePost(Customer context)
		{
            EFSession.Customers.Add(context);
            EFSession.SaveChanges();
			return Redirect("/Customer");
		}
	}
}

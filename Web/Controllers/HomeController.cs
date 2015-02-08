using System.Web.Mvc;

namespace SomeBasicEFApp.Controllers
{
	public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}

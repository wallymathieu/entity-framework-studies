using SomeBasicEFApp.Core;
using SomeBasicNHApp.Code;
using System.Web;
using System.Web.Mvc;

namespace SomeBasicEFApp.Code
{
	public class SessionController : Controller
    {
        public SessionController()
        {
        }

        public CoreDbContext EFSession
        {
            get
            {
                return new SessionContext(this.HttpContext).Session;
            }
        }

    }
}

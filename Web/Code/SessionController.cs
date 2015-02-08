using SomeBasicEFApp.Core;
using System.Web;
using System.Web.Mvc;

namespace SomeBasicEFApp.Code
{
	public class SessionController : Controller
    {
        public HttpSessionStateBase HttpSession
        {
            get { return base.Session; }
        }

        public new CoreDbContext Session
        {
            get
            {
                return (CoreDbContext)this.HttpContext.Items[NHibernateActionFilter.Name];
            }
        }
    }
}
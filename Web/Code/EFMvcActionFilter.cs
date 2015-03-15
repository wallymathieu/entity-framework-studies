using System.Web;
using System.Web.Mvc;
using SomeBasicEFApp.Core;

namespace SomeBasicEFApp.Code
{
    public class EFMvcActionFilter : ActionFilterAttribute
    {
        public const string Name = "EFSession";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Items[Name] = new Session(new WebMapPath()).CreateWebSessionFactory();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = (CoreDbContext)filterContext.HttpContext.Items[Name];
            if (session != null)
            {
                session.Dispose();
            }
        }
    }
}

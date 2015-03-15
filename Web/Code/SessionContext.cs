using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SomeBasicEFApp.Core;
using System.Web;
using System.Web.Mvc;
using SomeBasicEFApp.Code;

namespace SomeBasicNHApp.Code
{
    public class SessionContext
    {
        private HttpContextBase HttpContext;

        public SessionContext(HttpContextBase httpContext)
        {
            this.HttpContext = httpContext;
        }

        public CoreDbContext Session
        {
            get
            {
                return (CoreDbContext)this.HttpContext.Items[EFMvcActionFilter.Name];
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using SomeBasicEFApp.Web.Entities;

namespace SomeBasicEFApp.Web.Data
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// If not null, then what customer entity this user is bound to
        /// </summary>
        public Customer Customer { get; set; }   
    }
}

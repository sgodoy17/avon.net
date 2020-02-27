using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Component.Transversal.Extensions
{
    public static class UserManagerExtensions
    {
        public static IdentityResult CreateUser(IdentityUser user, string newPassword)
        {
            if (user == null) { throw new ArgumentNullException("user"); }
            return null;
        }
    }
}

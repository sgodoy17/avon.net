using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Component.Transversal.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid GetGuidUserId(this IIdentity identity)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(identity.GetUserId(), out result);
            return result;
        }

        //public static bool IsInAnyRole(this IPrincipal principal, params string[] roles)
        //{
        //    return roles.Any(principal.IsInRole);
        //}
    }
}

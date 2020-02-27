using IdentiGo.Domain.Enums;
using IdentiGo.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;

namespace IdentiGo.WebManagement.Security
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] allowedRoles)
        {
            Roles = string.Format("{0},{1},{2}", string.Join(",", allowedRoles), RoleName.Role1, RoleName.Role2);
        }
    }
}

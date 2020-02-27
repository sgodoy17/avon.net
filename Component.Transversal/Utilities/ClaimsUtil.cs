using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Component.Transversal.Utilities
{

    public class ClaimsUtils
    {
        /// <Summary >
        /// Gets the user is currently authenticated
        /// </Summary>
        /// < Returns> Authenticated User </returns>
        public static IPrincipal Pricipal
        {
            get
            {
                //if (!UserIsAuthenticated)
                //    throw new UnauthorizedAccessException(string.Format("El usuario {0} no se encuentra autenticado o el tipo de autenticación no es valida para la seguridad federada.", Thread.CurrentPrincipal.Identity.Name));

                var currentPrincipal = Thread.CurrentPrincipal;

                return currentPrincipal;
            }
        }

        /// <summary>
        /// Gets the identity of the authenticated user
        /// </summary>
        /// <returns>Identidad del usuario autenticado</returns>
        public static IIdentity Identity
        {
            get
            {
                return Pricipal.Identity;
            }
        }

        public static ClaimsIdentity ClaimsUserIdentity
        {
            get
            {
                return Identity as ClaimsIdentity;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user is authenticated
        /// </summary>
        /// <returns></returns>
        public static bool UserIsAuthenticated
        {
            get
            {
                // If the user is not authenticated
                bool isAuthenticated = true;
                if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
                    isAuthenticated = false;

                try
                {
                    // If authentication is not type Claims
                    var claims = Thread.CurrentPrincipal as ClaimsPrincipal;
                }
                catch
                {
                    isAuthenticated = false;
                }

                return isAuthenticated;
            }
        }

        public static string GetClaimValue(string type)
        {
            if (ClaimsUserIdentity == null)
                return string.Empty;

            var claim = ClaimsUserIdentity.Claims.FirstOrDefault(c => c.Type == type);
            if (claim == null)
                return string.Empty;
            else
                return claim.Value;
        }
    }
}

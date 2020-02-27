using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

namespace Component.Transversal.Utilities
{
    /// <summary>
    /// Provee operaciones para la simplificación de la autenticación que se tiene actualmente
    /// </summary>
    public class CurrentUserUtils
    {
        public static string AnonimousUserName = "Identigo_AnonimousUser";

        public static Guid AnonimousUserId = new Guid("10c6cbb1-1a85-4af7-841a-1535625e2e05");

        /// <summary>
        /// Obtiene un valor que indica si el usuario se encuentra autenticado
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthenticated
        {
            get
            {
                return ClaimsUtils.UserIsAuthenticated;
            }
        }

        /// <summary>
        /// Obtiene el nombre actual del usuario autenticado
        /// </summary>
        /// <returns></returns>
        public static string UserName
        {
            get
            {
                return string.IsNullOrEmpty(ClaimsUtils.Identity.Name) ? ClaimsUtils.GetClaimValue(CustomClaimTypes.AnonimousUserName) : ClaimsUtils.Identity.Name;
            }
        }

        /// <summary>
        /// Id del usuario atenticado
        /// </summary>
        /// <returns></returns>
        public static Guid UserId
        {
            get
            {
                string key = ClaimsUtils.GetClaimValue(CustomClaimTypes.UserId);

                if (key == string.Empty)
                    return Guid.Empty;
                else
                    return new Guid(key);
            }
        }

        /// <summary>
        /// Nombre completo del usuario logueado
        /// </summary>
        /// <returns></returns>
        public static string UserFullName
        {
            get
            {
                return ClaimsUtils.GetClaimValue(CustomClaimTypes.UserFullName);
            }
        }

        /// <summary>
        /// Firma del usuario logueado
        /// </summary>
        /// <returns></returns>
        public static string UserSignature
        {
            get
            {
                return EncodingUtils.DecodeFrom64(ClaimsUtils.GetClaimValue(CustomClaimTypes.UserSignature));
            }
        }

        /// <summary>
        /// Nombre completo del usuario logueado
        /// </summary>
        /// <returns></returns>
        public static string UserEmail
        {
            get
            {
                return ClaimsUtils.GetClaimValue(CustomClaimTypes.UserEmail);
            }
        }

        /// <summary>
        /// Obtiene la fecha de vencimiento de la contraseña del usuario
        /// </summary>
        /// <returns></returns>
        public static DateTime PasswordExpiration
        {
            get
            {
                string expirationDate = ClaimsUtils.GetClaimValue(CustomClaimTypes.PasswordExpiration);

                if (expirationDate == string.Empty)
                    return DateTime.Now;
                else
                    return DateTime.Parse(expirationDate);
            }
        }

        /// <summary>
        /// Obtiene un valor que indica si la contraseña ha expirado
        /// </summary>
        /// <returns></returns>
        public static bool ExpiredPassword
        {
            get
            {
                string expired = ClaimsUtils.GetClaimValue(CustomClaimTypes.ExpiredPassword);

                if (expired == string.Empty)
                    return false;
                else
                    return bool.Parse(expired);
            }
        }

        /// <summary>
        /// Establece la autenticación con el usuario anonimo de seguridad por defecto
        /// </summary>
        public static void AuthenticateAnonimous()
        {
            ClaimsIdentity identity = new ClaimsIdentity(AuthenticationTypes.Federation, AnonimousUserName, string.Empty);
            List<Claim> claims = new List<Claim>
            {
                new Claim(CustomClaimTypes.UserId, AnonimousUserId.ToString()),
                new Claim(CustomClaimTypes.AnonimousUserName, AnonimousUserName)
            };

            //Se agregan los claims necesarios
            foreach (var claim in claims)
            {
                identity.AddClaim(claim);
            }

            ClaimsPrincipal anonimousPrincipal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = anonimousPrincipal;
            AppDomain.CurrentDomain.SetThreadPrincipal(Thread.CurrentPrincipal);
        }


        /// <summary>
        /// Tipo Usuario dentro de la aplicación
        /// </summary>
        /// <returns></returns>
        public static string TypeUserLogin
        {
            get
            {
                return ClaimsUtils.GetClaimValue(CustomClaimTypes.TypeUserLogin);
            }
        }

        public static AppEnterprise CurrentEnterprise
        {
            get
            {
                return AppEnterprise.Current();
            }
        }

        #region Private



        #endregion
    }
}

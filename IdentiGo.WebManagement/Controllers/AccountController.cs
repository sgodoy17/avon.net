using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IdentiGo.Domain.Security;
using IdentiGo.WebManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Component.Transversal.Utilities;
using IdentiGo.Services.Security;
using IdentiGo.Services.General;
using IdentiGo.Domain.Enums;
using Component.Transversal.WebApi;
using System.Web.Helpers;
using IdentiGo.Services.Master;
using IdentiGo.ExternalServices;
using System.Text.RegularExpressions;

namespace IdentiGo.WebManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IGroupRoleService _groupRoleService;
        private readonly IZoneService _zoneService;
        private readonly IUnitService _unitService;
        private readonly IRoleService _roleService;

        public readonly NombraAvonExternalService NombraAvonService = new NombraAvonExternalService();

        public AccountController(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            IUserService userService,
            ICompanyService companyService,
            IAuthenticationManager authenticationManager,
            IGroupRoleService groupRoleService,
            IZoneService zoneService,
            IUnitService unitService,
            IRoleService roleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _companyService = companyService;
            _authenticationManager = authenticationManager;
            _groupRoleService = groupRoleService;
            _zoneService = zoneService;
            _unitService = unitService;
            _roleService = roleService;
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                string error = null;
                var result = SignInStatus.Success;

                if (_userService.GetByUserName(model.Email) == null)
                    LoginValidate(model, ref error);
                else
                    result = _userService.ValidatePassword(model.Email, model.Password);

                switch (result)
                {
                    case SignInStatus.Success:
                        if (!string.IsNullOrEmpty(_userService.ValidChangePassword(model.Password)))
                            return RedirectToAction("ChangeFirstPassword", new ChangePasswordViewModels { UserName = model.Email });

                        User user = _userService.GetByUserName(model.Email);
                        UserManagerZone(ref user);
                        ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                        _authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = model.RememberMe }, identity);

                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
                    case SignInStatus.Failure:
                        if (error != null)
                            ModelState.AddModelError("", error);
                        else
                            ModelState.AddModelError("", "Intento de inicio de sesión no válido.");

                        return View(model);
                    default:
                        ModelState.AddModelError("", "Intento de inicio de sesión no válido.");

                        return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View(model);
            }
        }

        public void LoginValidate(LoginViewModel model, ref string error)
        {
            var document = model.Email;
            var codeUser = model.Password.PadLeft(11, '0');
            var result = NombraAvonService.ConsultCode(codeUser);

            if (!string.IsNullOrEmpty(result.errorCode) && result.errorCode != "00")
                throw new Exception("Servicio de Avon fuera de línea. Por favor comunicate con servicio a la representante");

            if (result.output6?.Split('.')[0] != document)
                throw new Exception("Intento de inicio de sesión no válido.");

            if (_zoneService.GetByCode(codeUser) == null)
                throw new Exception("Intento de inicio de sesión no válido.");

            var newUser = new User
            {
                IdNumber = document,
                UserName = document,
                Email = document,
                //CodeVerification = int.Parse(model.Password),
                CodeVerification = model.Password,
                Password = codeUser,
                ConfirmPassword = codeUser,
                Address1 = "No Aplica",
                Birthdate = DateTime.Now.AddYears(-18),
                ActivationDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                PhoneNumber = "",
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            newUser.PasswordHash = Crypto.HashPassword(newUser.Password);
            CofigureName(ref newUser, result.output3);
            _userService.AddOrUpdate(newUser);
        }

        public void CofigureName(ref User item, string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            var names = Regex.Replace(name, @"\s+", " ").TrimEnd(' ').Split(' ');

            if (names.Length == 1)
                item.Name1 = names[0];
            else if (names.Length == 2)
            {
                item.Name1 = names[0];
                item.LastName1 = names[1];
            }
            else if (names.Length == 3)
            {
                item.Name1 = names[0];
                item.Name2 = names[1];
                item.LastName1 = names[2];
            }
            else if (names.Length > 3)
            {
                item.Name2 = "";
                item.LastName2 = "";

                for (int i = 0; i < names.Length; i++)
                {
                    if (i < names.Length - 3)
                    {
                        if (i == 0)
                            item.Name1 = $"{names[i]} {item.Name1}";
                        else
                            item.Name2 = $"{names[i]} {item.Name2}";
                    }
                    else
                    {
                        if (i == names.Length - 2)
                            item.LastName1 = $"{names[i]} {item.LastName1}";
                        else
                            item.LastName2 = $"{names[i]} {item.LastName2}";
                    }
                }
            }
        }

        public void UserManagerZone(ref User user)
        {
            var roles = _roleService.GetByName(new string[] { RoleName.Role6 });

            if (_zoneService.GetByCode(user.CodeVerification?.ToString().PadLeft(11, '0')) != null)
            {
                _userService.RemoveRoles(user.Id, roles);
                _userService.AddRoles(user.Id, roles);
            }
            else
            {
                _userService.RemoveRoles(user.Id, roles);
            }

            user = _userService.GetReload(user.Id);
        }

        // POST: /Account/LogOff
        [Authorize]
        public ActionResult LogOff()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Login", new { returnUrl = "", admin = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #region Helper applications
        private const string XsrfKey = "XsrfId";

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };

                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        // GET: /Users/Edit/1
        [Authorize]
        public new ActionResult Profile()
        {
            var user = _userService.GetByUserName(User.Identity.GetUserName());

            if (user == null) return HttpNotFound();

            ViewBag.GenderList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(Gender)).Cast<Gender>().Where(e => e != Gender.Other && e != Gender.Undefined).ToList(), user.Gender);
            ViewBag.IdentificationTypeList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(IdentificationType)).Cast<IdentificationType>().Where(e => e != IdentificationType.Unknown).ToList(), user.IdType);
            var userRoles = _groupRoleService.GetIdByUserId(user.Id).ToList();
            user.RolesList = _groupRoleService.GetAll().Select(x => new SelectListItem() { Selected = userRoles.Contains(x.Id), Text = x.Name, Value = x.Id.ToString() });

            return View(user);
        }

        // POST: /Users/Edit/5
        [HttpPost]
        [Authorize]
        public new ActionResult Profile(User editUser)
        {
            if (ModelState.IsValid)
            {
                editUser.Email = User.Identity.GetUserName();
                var adminresult = _userService.EditUser(editUser);

                if (!adminresult.Succeeded)
                    ModelState.AddModelError("", adminresult.Errors.First());
            }

            if (ModelState.IsValid)
                return RedirectToAction("Index", "Home", new { area = "" });

            ViewBag.GenderList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(Gender)).Cast<Gender>().Where(e => e != Gender.Other && e != Gender.Undefined).ToList(), editUser.Gender);
            ViewBag.IdentificationTypeList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(IdentificationType)).Cast<IdentificationType>().Where(e => e != IdentificationType.Unknown).ToList(), editUser.IdType);

            return View(editUser);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModels());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModels changePassword)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _userService.GetByUserName(User.Identity.GetUserName());
                var resultChange = _userService.ValidChangePassword(changePassword.Password);

                if (!string.IsNullOrEmpty(resultChange))
                    ModelState.AddModelError("", resultChange);
                else
                {
                    currentUser.Password = changePassword.Password;
                    currentUser.ConfirmPassword = changePassword.ConfirmPassword;
                    currentUser.SecurityStamp = Guid.NewGuid().ToString();
                    currentUser.PasswordHash = Crypto.HashPassword(currentUser.Password);
                    _userService.Update(currentUser);

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }

            return View(changePassword);
        }

        public ActionResult ChangeFirstPassword(string userName)
        {
            return View("ChangeFirstPassword", new ChangePasswordViewModels { UserName = userName });
        }

        [HttpPost]
        public ActionResult ChangeFirstPassword(ChangePasswordViewModels changePassword)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _userService.GetByUserName(changePassword.UserName);
                var resultChange = _userService.ValidChangePassword(changePassword.Password);

                if (!string.IsNullOrEmpty(resultChange))
                    ModelState.AddModelError("", resultChange);
                else
                {
                    currentUser.Password = changePassword.Password;
                    currentUser.ConfirmPassword = changePassword.ConfirmPassword;
                    currentUser.SecurityStamp = Guid.NewGuid().ToString();
                    currentUser.PasswordHash = Crypto.HashPassword(currentUser.Password);
                    _userService.Update(currentUser);
                    UserManagerZone(ref currentUser);
                    ClaimsIdentity identity = _userManager.CreateIdentity(currentUser, DefaultAuthenticationTypes.ApplicationCookie);
                    _authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = changePassword.RememberMe }, identity);

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
            return View(changePassword);
        }
        #endregion
    }
}
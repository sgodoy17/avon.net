using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using IdentiGo.Domain.Security;
using Component.Transversal.Extensions;
using IdentiGo.Domain.Enums;
using IdentiGo.Services.General;
using IdentiGo.Services.Security;
using System.Collections.Generic;
using Component.Transversal.Utilities;
using Component.Transversal.WebApi;
using Component.Transversal.Adapters;
using IdentiGo.WebManagement.Security;
using IdentiGo.WebManagement.Models;
using System.Web.Helpers;
using IdentiGo.Services.Master;
using System.Web;
using IdentiGo.Transversal.Utilities;
using System.IO;

namespace IdentiGo.WebManagement.Controllers
{
    [AuthorizeRoles]
    public class UsersAdminController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IGroupRoleService _groupRoleService;
        private readonly IZoneService _zoneService;
        private readonly IUnitService _unitService;

        private readonly ICompanyService _companyService;
        ITypeAdapter _typeAdapter = TypeAdapterFactory.CreateAdapter();

        private const string userAdmin = "admin@enterdev.com.co";

        public UsersAdminController(ApplicationUserManager userManager,
            ApplicationRoleManager roleManager,
            IRoleService roleService,
            IUserService userService,
            ICompanyService companyService,
            IGroupRoleService groupRoleService,
            IZoneService zoneService,
            IUnitService unitService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleService = roleService;
            _userService = userService;
            _groupRoleService = groupRoleService;
            _companyService = companyService;
            _zoneService = zoneService;
            _unitService = unitService;
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index(int? companyId = null)
        {
            var user = await _userManager.FindByIdAsync(User.Identity.GetGuidUserId());

            //Validates that a user is not logging users to another company if not super admin
            var userList = _userService.GetMany(x => x.UserName != userAdmin);

            return View(userList);
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id);

            ViewBag.RoleNames = await _userManager.GetRolesAsync(user.Id);

            return View(user);
        }

        //
        // GET: /Users/Create
        public ActionResult Create()
        {
            ViewBag.GenderList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(Gender)).Cast<Gender>().Where(e => e != Gender.Other && e != Gender.Undefined).ToList(), Gender.Male);
            ViewBag.IdentificationTypeList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(IdentificationType)).Cast<IdentificationType>().Where(e => e != IdentificationType.Unknown).ToList(), IdentificationType.CedulaCiudania);

            return View(new User
            {
                Email = string.Empty
            });
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(User user, params string[] selectedRoles)
        {
            var userValid = _userService.Get(User.Identity.GetGuidUserId());
            if (ModelState.IsValid)
            {
                //Validates that a user is not logging users to another company if not super admin

                var adminresult = _userService.CreateUser(user, user.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await _userManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                            ModelState.AddModelError("", result.Errors.First());
                    }
                }
                else
                    ModelState.AddModelError("", adminresult.Errors.First());
            }
            if (ModelState.IsValid)
                return RedirectToAction("Index");

            ViewBag.GenderList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(Gender)).Cast<Gender>().Where(e => e != Gender.Other && e != Gender.Undefined).ToList(), user.Gender);
            ViewBag.IdentificationTypeList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(IdentificationType)).Cast<IdentificationType>().Where(e => e != IdentificationType.Unknown).ToList(), user.IdType);

            user.RolesList = _roleService.GetByTypeRole(TypeRole.Role, userValid.Roles.Any(x => x.Role.Name == RoleName.Role1)).ToList().Select(x => new SelectListItem() { Selected = false, Text = x.Name, Value = x.Name });
            user.ModuleList = (_roleService.GetByTypeRole(TypeRole.Modulo) ?? new List<Role>()).ToList().Select(x => new SelectListItem { Selected = false, Text = x.Name, Value = x.Name });
            return View(user);
        }

        //
        // GET: /Users/Edit/1
        public ActionResult Edit(Guid id)
        {
            var user = _userService.Get(id);

            if (user == null) return HttpNotFound();

            ViewBag.GenderList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(Gender)).Cast<Gender>().Where(e => e != Gender.Other && e != Gender.Undefined).ToList(), user.Gender);
            ViewBag.IdentificationTypeList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(IdentificationType)).Cast<IdentificationType>().Where(e => e != IdentificationType.Unknown).ToList(), user.IdType);

            var userRoles = _groupRoleService.GetIdByUserId(user.Id).ToList();

            user.RolesList = _groupRoleService.GetAll().Select(x => new SelectListItem() { Selected = userRoles.Contains(x.Id), Text = x.Name, Value = x.Id.ToString() });

            return View(user);
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {

                var adminresult = _userService.EditUser(editUser);

                var userRoles = _groupRoleService.GetIdByUserId(editUser.Id);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    selectedRole = selectedRole ?? new string[] { };
                    var result = _userService.AddGroupRoles(editUser.Id, _groupRoleService.GetById(selectedRole));
                    if (!result)
                        ModelState.AddModelError("", "Failed to relate the roles by user");

                    result = _userService.RemoveGroupRoles(editUser.Id, _groupRoleService.GetById((userRoles.Select(x => x.ToString()).Except(selectedRole).Except(selectedRole.ToList()).ToArray<string>())));
                    if (!result)
                    {
                        ModelState.AddModelError("", "Failed to remove the roles by user");
                    }
                }
                else
                    ModelState.AddModelError("", adminresult.Errors.First());

            }
            if (ModelState.IsValid)
                return RedirectToAction("Index");

            ViewBag.GenderList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(Gender)).Cast<Gender>().Where(e => e != Gender.Other && e != Gender.Undefined).ToList(), editUser.Gender);
            ViewBag.IdentificationTypeList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(IdentificationType)).Cast<IdentificationType>().Where(e => e != IdentificationType.Unknown).ToList(), editUser.IdType);

            var editUserRoles = _groupRoleService.GetIdByUserId(editUser.Id);
            editUser.RolesList = _groupRoleService.GetAll().Select(x => new SelectListItem() { Selected = editUserRoles.Contains(x.Id), Text = x.Name, Value = x.Id.ToString() });
            return View(editUser);
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return HttpNotFound();

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded) return RedirectToAction("Index");

            ModelState.AddModelError("", result.Errors.First());
            return View();
        }


        [Authorize]
        public ActionResult ChangePassword(Guid id)
        {
            return View(new ChangePasswordViewModels { UserId = id });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModels changePassword)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _userService.Get(changePassword.UserId);

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

        #region Zone

        [Authorize]
        public async Task<ActionResult> IndexZone()
        {
            var user = await _userManager.FindByIdAsync(User.Identity.GetGuidUserId());
            if (!_zoneService.GetMany(x => x.Code == user.CodeVerification?.ToString()).Any())
                throw new Exception("El usuario no es gerente de Zona");

            var listCode = _unitService.GetMany(x => x.Zone.Code == user.CodeVerification?.ToString()).Select(x => x.Code);

            //Validates that a user is not logging users to another company if not super admin
            var userList = _userService.GetMany(x => listCode.Contains(x.CodeVerification?.ToString()));

            return View(userList);
        }
        
        //
        // GET: /Users/Create
        public async Task<ActionResult> CreateZone()
        {
            var user = await _userManager.FindByIdAsync(User.Identity.GetGuidUserId());
            if (!_zoneService.GetMany(x => x.Code == user.CodeVerification?.ToString()).Any())
                throw new Exception("El usuario no es gerente de Zona");

            ViewBag.GenderList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(Gender)).Cast<Gender>().Where(e => e != Gender.Other && e != Gender.Undefined).ToList(), Gender.Male);
            ViewBag.IdentificationTypeList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(IdentificationType)).Cast<IdentificationType>().Where(e => e != IdentificationType.Unknown).ToList(), IdentificationType.CedulaCiudania);

            return View(new User
            {
                Email = string.Empty
            });
        }

        //
        // POST: /Users/Create
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateZone(User user, params string[] selectedRoles)
        {
            var userValid = _userService.Get(User.Identity.GetGuidUserId());
            if (!_zoneService.GetMany(x => x.Code == userValid.CodeVerification?.ToString()).Any())
                throw new Exception("El usuario no es gerente de Zona");

            if (ModelState.IsValid)
            {
                //Validates that a user is not logging users to another company if not super admin

                var adminresult = _userService.CreateUser(user, user.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await _userManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                            ModelState.AddModelError("", result.Errors.First());
                    }
                }
                else
                    ModelState.AddModelError("", adminresult.Errors.First());
            }
            if (ModelState.IsValid)
                return RedirectToAction("IndexZone");

            ViewBag.GenderList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(Gender)).Cast<Gender>().Where(e => e != Gender.Other && e != Gender.Undefined).ToList(), user.Gender);
            ViewBag.IdentificationTypeList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(IdentificationType)).Cast<IdentificationType>().Where(e => e != IdentificationType.Unknown).ToList(), user.IdType);

            user.RolesList = _roleService.GetByTypeRole(TypeRole.Role, userValid.Roles.Any(x => x.Role.Name == RoleName.Role1)).ToList().Select(x => new SelectListItem() { Selected = false, Text = x.Name, Value = x.Name });
            user.ModuleList = (_roleService.GetByTypeRole(TypeRole.Modulo) ?? new List<Role>()).ToList().Select(x => new SelectListItem { Selected = false, Text = x.Name, Value = x.Name });
            return View(user);
        }

        //
        // GET: /Users/Edit/1
        [Authorize]
        public ActionResult EditZone(Guid id)
        {
            var userValid = _userService.Get(User.Identity.GetGuidUserId());
            if (!_zoneService.GetMany(x => x.Code == userValid.CodeVerification?.ToString()).Any())
                throw new Exception("El usuario no es gerente de Zona");

            var user = _userService.Get(id);

            if (user == null) return HttpNotFound();

            ViewBag.GenderList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(Gender)).Cast<Gender>().Where(e => e != Gender.Other && e != Gender.Undefined).ToList(), user.Gender);
            ViewBag.IdentificationTypeList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(IdentificationType)).Cast<IdentificationType>().Where(e => e != IdentificationType.Unknown).ToList(), user.IdType);

            var userRoles = _groupRoleService.GetIdByUserId(user.Id).ToList();

            user.RolesList = _groupRoleService.GetAll().Select(x => new SelectListItem() { Selected = userRoles.Contains(x.Id), Text = x.Name, Value = x.Id.ToString() });

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditZone(User editUser, params string[] selectedRole)
        {
            var userValid = _userService.Get(User.Identity.GetGuidUserId());
            if (!_zoneService.GetMany(x => x.Code == userValid.CodeVerification?.ToString()).Any())
                throw new Exception("El usuario no es gerente de Zona");

            if (ModelState.IsValid)
            {

                var adminresult = _userService.EditUser(editUser);

                var userRoles = _groupRoleService.GetIdByUserId(editUser.Id);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    selectedRole = selectedRole ?? new string[] { };
                    var result = _userService.AddGroupRoles(editUser.Id, _groupRoleService.GetById(selectedRole));
                    if (!result)
                        ModelState.AddModelError("", "Failed to relate the roles by user");

                    result = _userService.RemoveGroupRoles(editUser.Id, _groupRoleService.GetById((userRoles.Select(x => x.ToString()).Except(selectedRole).Except(selectedRole.ToList()).ToArray<string>())));
                    if (!result)
                    {
                        ModelState.AddModelError("", "Failed to remove the roles by user");
                    }
                }
                else
                    ModelState.AddModelError("", adminresult.Errors.First());

            }
            if (ModelState.IsValid)
                return RedirectToAction("Index");

            ViewBag.GenderList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(Gender)).Cast<Gender>().Where(e => e != Gender.Other && e != Gender.Undefined).ToList(), editUser.Gender);
            ViewBag.IdentificationTypeList = EnumUtils.GetSelectListEnum(Enum.GetValues(typeof(IdentificationType)).Cast<IdentificationType>().Where(e => e != IdentificationType.Unknown).ToList(), editUser.IdType);

            var editUserRoles = _groupRoleService.GetIdByUserId(editUser.Id);
            editUser.RolesList = _groupRoleService.GetAll().Select(x => new SelectListItem() { Selected = editUserRoles.Contains(x.Id), Text = x.Name, Value = x.Id.ToString() });
            return View(editUser);
        }

        [HttpGet]
        public ActionResult LoadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadFile(HttpPostedFileBase file)
        {
            try
            {
                ReadExcel.InitializeSheet(file.InputStream, Path.GetExtension(file.FileName));

                var list = ReadExcel.ConvertToList<User>();

                _userService.LoadList(list);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}

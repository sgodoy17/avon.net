using System.Linq;
using System.Net;
using System.Web.Mvc;
using IdentiGo.Domain.Security;
using System;
using IdentiGo.Services.Security;
using IdentiGo.WebManagement.Security;
using IdentiGo.Domain.Enums;

namespace IdentiGo.WebManagement.Controllers
{
    [AuthorizeRoles]
    public class GroupRolesAdminController : Controller
    {

        private UserService UserService;
        private RoleService RoleService;
        private GroupRoleService GroupRoleService;

        public GroupRolesAdminController()
        {
        }

        public GroupRolesAdminController(UserService userService,
            RoleService roleService,
            GroupRoleService groupRoleService)
        {
            UserService = userService;
            RoleService = roleService;
            GroupRoleService = groupRoleService;
        }

        //
        // GET: /Roles/
        public ActionResult Index()
        {
            var gruopList = GroupRoleService.GetAll();

            return View(gruopList);
        }

        //
        // GET: /Roles/Details/5
        public ActionResult Details(Guid id)
        {
            var role = GroupRoleService.Get(id);

            return View(role);
        }

        //
        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(GroupRole role)
        {
            try
            {
                var roleresult = GroupRoleService.Add(role);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View();
            }
        }

        //
        // GET: /Roles/Edit/Admin
        public ActionResult Edit(Guid id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var role = GroupRoleService.Get(id);

            if (role == null)
                return HttpNotFound();

            var roleName = role.Role.Select(x => x.Id);

            ViewBag.RoleList = RoleService.GetMany(x => x.Name != RoleName.Role1 && x.Name != RoleName.Role6).ToList().Select(x => new SelectListItem()
            {
                Selected = roleName.Contains(x.Id),
                Text = x.DisplayName,
                Value = x.Id.ToString()
            });

            return View(role);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult Edit(GroupRole groupRole, params string[] selectedRole)
        {
            try
            {
                GroupRoleService.Update(groupRole);

                groupRole = GroupRoleService.Get(groupRole.Id);

                var roles = groupRole.Role.Select(x => x.Id.ToString()).ToList();

                selectedRole = selectedRole ?? new string[] { };

                var result = GroupRoleService.AddRoles(groupRole.Id, RoleService.GetById(selectedRole));

                if (!result) throw new Exception("Failed to relate the roles");

                result = GroupRoleService.RemoveRoles(groupRole.Id, RoleService.GetById(roles.Except(selectedRole).ToArray()));

                if (!result) throw new Exception("Failed to remove the roles");

                result = UserService.AddRolesByGroupId(groupRole.Id, RoleService.GetById(selectedRole));

                if (!result) throw new Exception("Failed to relate the roles by user");

                result = UserService.RemoveRolesByGroupId(groupRole.Id, RoleService.GetById(roles.Except(selectedRole).ToArray()));

                if (!result) throw new Exception("Failed to remove the roles by user");

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        //
        // GET: /Roles/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var role = GroupRoleService.Get(id);

            if (role == null)
                return HttpNotFound();

            return View(role);
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id, string deleteUser)
        {
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                UserService.RemoveRolesByGroupId(id, RoleService.GetByGroupRoleId(id));

                GroupRoleService.Delete(id);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}

using System.Net;
using System.Web.Mvc;
using IdentiGo.Domain.Security;
using System;
using IdentiGo.Domain.Enums;
using IdentiGo.Services.Security;

namespace IdentiGo.WebManagement.Controllers
{
    [Authorize(Roles = RoleName.Role1)]
    public class PermissionsAdminController : Controller
    {
        private RoleService RoleService;
        public PermissionsAdminController()
        {
        }

        public PermissionsAdminController(RoleService roleService)
        {
            RoleService = roleService;
        }
        //
        // GET: /Roles/
        public ActionResult Index()
        {
            return View(RoleService.GetAll());
        }

        //
        // GET: /Roles/Details/5
        public ActionResult Details(Guid id)
        {
            var role = RoleService.Get(id);

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
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                var roleresult = RoleService.Add(role);

                return RedirectToAction("Index");
            }

            return View();
        }

        //
        // GET: /Roles/Edit/Admin
        public ActionResult Edit(Guid id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var role = RoleService.Get(id);

            if (role == null)
                return HttpNotFound();

            return View(role);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult Edit(Role role)
        {
            try
            {
                RoleService.Update(role);

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

            var role = RoleService.Get(id);

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

                RoleService.Delete(id);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}

using IdentiGo.Domain.Entity;
using IdentiGo.Domain.Enums;
using IdentiGo.Domain.Security;
using IdentiGo.Services.General;
using IdentiGo.Services.Security;
using IdentiGo.Transversal.Utilities;
using IdentiGo.WebManagement.Models;
using System;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity.Validation;
using Component.Transversal.Utilities;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web;
using System.IO;
using System.Drawing;
using System.Text;
using System.Web.Configuration;
using IdentiGo.Domain.DTO;
using IdentiGo.Domain.Entity.General;
using IdentiGo.WebManagement.Security;

namespace IdentiGo.WebManagement.Controllers
{
    [AuthorizeRoles("Company")]
    public class CompanyController : Controller
    {
        public readonly ICompanyService CompanyService;
        public readonly IRoleService RoleService;
        public readonly IUserService UserService;
        public readonly ApplicationUserManager UserManager;
        public readonly IAuthenticationManager AuthenticationManager;

        public CompanyController(ICompanyService companyService, IRoleService roleService, IUserService userService, ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
        {
            CompanyService = companyService;
            RoleService = roleService;
            UserService = userService;
            UserManager = userManager;
            AuthenticationManager = authenticationManager;
        }

        //
        // GET: /Company/
        [AuthorizeRoles]
        public ActionResult Index()
        {
            var companyList = CompanyService.GetAll();
            return View(companyList);
        }


        // GET: /Company/Details/5
        public ActionResult Details(int id)
        {
            var company = CompanyService.Get(id);

            ViewBag.RoleNames = company.Role;

            return View(company);
        }

        //
        // GET: /Company/Create
        [AuthorizeRoles]
        public ActionResult Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(RoleService.GetByTypeRole(TypeRole.Modulo), "Name", "Name");
            return View();
        }

        //
        // POST: /Company/Create
        [HttpPost]
        [AuthorizeRoles]
        public ActionResult Create(Company company, HttpPostedFileBase file, params string[] selectedRoles)
        {
            if (file != null)
            {
                var path = Path.Combine(Server.MapPath(WebConfigurationManager.AppSettings["urlCompanyLogo"]), Path.GetFileName(file.FileName));
                var pathImg = Path.Combine(WebConfigurationManager.AppSettings["urlCompanyLogo"], Path.GetFileName(file.FileName));

                if (System.IO.File.Exists(path)) throw new Exception("the file exist");

                file.SaveAs(path);

                company.Image = pathImg;
            }

            if (ModelState.IsValid)
            {
                company.PublicKey = new Guid();

                company = CompanyService.Add(company);

                if (selectedRoles == null) return RedirectToAction("Index");

                var roles = RoleService.GetById(selectedRoles);
                if (CompanyService.AddRoles(company.Id, roles)) return RedirectToAction("Index");

                ModelState.AddModelError("", "Failed to relate the roles");

                ViewBag.RoleId = new SelectList(RoleService.GetByTypeRole(TypeRole.Modulo), "Name", "Name");

                return View();
            }

            ViewBag.RoleId = new SelectList(RoleService.GetByTypeRole(TypeRole.Modulo), "Name", "Name");

            return View();
        }

        //
        // GET: /Company/Edit/1
        public ActionResult Edit(int id)
        {
            var company = CompanyService.Get(id);

            if (company == null)
                return HttpNotFound();

            var companyRoles = company.Role.Select(x => x.Name).ToList();
            company.RolesList = RoleService.GetByTypeRole(TypeRole.Modulo).ToList().Select(x => new SelectListItem()
                {
                    Selected = companyRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                });
            return View(company);
        }

        //
        // POST: /Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company, HttpPostedFileBase file, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                if (company == null) return HttpNotFound();

                if (file != null)
                {
                    var path = Path.Combine(Server.MapPath(WebConfigurationManager.AppSettings["urlCompanyLogo"]), Path.GetFileName(file.FileName));
                    var pathImg = Path.Combine(WebConfigurationManager.AppSettings["urlCompanyLogo"], Path.GetFileName(file.FileName));

                    if (company.Image != pathImg && System.IO.File.Exists(path)) throw new Exception("the file exist");

                    file.SaveAs(path);

                    company.Image = Path.Combine(WebConfigurationManager.AppSettings["urlCompanyLogo"], Path.GetFileName(file.FileName));
                }

                company = CompanyService.UpdateManual(company);

                var companyRoles = company.Role.Select(x => x.Name).ToList();

                selectedRole = selectedRole ?? new string[] { };

                return RedirectToAction("Index");
            }

            throw new Exception("Something failed.");

        }

        //
        // GET: /Company/Delete/5
        [AuthorizeRoles]
        public ActionResult Delete(int id)
        {
            try
            {
                var company = CompanyService.Get(id);
                if (company == null)
                {
                    return HttpNotFound();
                }
                return View(company);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //
        // POST: /Company/Delete/5
        [AuthorizeRoles()]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                CompanyService.Delete(id);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}

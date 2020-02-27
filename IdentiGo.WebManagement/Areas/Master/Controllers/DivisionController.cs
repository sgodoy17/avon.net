using System;
using System.Web;
using System.Web.Mvc;
using IdentiGo.Services.Master;
using IdentiGo.Domain.Entity.Master;
using IdentiGo.Transversal.Services;
using IdentiGo.WebManagement.Security;
using IdentiGo.Domain.Enums;

namespace IdentiGo.WebManagement.Areas.Master.Controllers
{
    [AuthorizeRoles(RoleName.Role3)]
    public class DivisionController : Controller
    {
        public readonly IDivisionService DivisionService;
        private readonly ILoadDataFileService LoadFileService;

        public DivisionController(IDivisionService divisionService, ILoadDataFileService loadFileService)
        {
            DivisionService = divisionService;
            LoadFileService = loadFileService;
        }

        //
        // GET: /division/
        public ActionResult Index()
        {
            var divisionList = DivisionService.GetAll();
            return View(divisionList);
        }

        // GET: /division/Details/5
        public ActionResult Details(Guid id)
        {
            var division = DivisionService.Get(id);
            return View(division);
        }

        //
        // GET: /division/Create
        public ActionResult Create()
        {
            //Get the list of Roles
            return View(new Division());
        }

        //
        // POST: /division/Create
        [HttpPost]
        public ActionResult Create(Division division)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DivisionService.AddOrUpdate(division);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View();
        }

        //
        // GET: /division/Edit/1
        public ActionResult Edit(Guid id)
        {
            var division = DivisionService.Get(id);
            if (division == null) return HttpNotFound();
            return View(division);
        }

        //
        // POST: /division/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Division division)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DivisionService.Update(division);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(division);

        }

        //
        // GET: /division/Delete/5
        public ActionResult Delete(Guid id)
        {
            try
            {
                var division = DivisionService.Get(id);
                if (division == null)
                {
                    return HttpNotFound();
                }
                return View(division);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //
        // POST: /division/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (!ModelState.IsValid) return View();
            DivisionService.Delete(id);
            return RedirectToAction("Index");
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
                LoadFileService.LoadDivision(file);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
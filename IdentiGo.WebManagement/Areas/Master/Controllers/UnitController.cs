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
    public class UnitController : Controller
    {
        public readonly IUnitService UnitService;
        private readonly ILoadDataFileService LoadFileService;
        private readonly IZoneService ZoneService;

        public UnitController(IUnitService unitService, ILoadDataFileService loadFileService, IZoneService zoneService)
        {
            UnitService = unitService;
            LoadFileService = loadFileService;
            ZoneService = zoneService;
        }

        //
        // GET: /unit/
        public ActionResult Index()
        {
            var unitList = UnitService.GetAll();
            return View(unitList);
        }

        // GET: /unit/Details/5
        public ActionResult Details(Guid id)
        {
            var unit = UnitService.Get(id);
            return View(unit);
        }

        //
        // GET: /unit/Create
        public ActionResult Create()
        {
            //Get the list of Roles
            ViewBag.ZoneList = new SelectList(ZoneService.GetAll(), "Id", "Number");

            return View(new Unit());
        }

        //
        // POST: /unit/Create
        [HttpPost]
        public ActionResult Create(Unit unit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UnitService.AddOrUpdate(unit);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.ZoneList = new SelectList(ZoneService.GetAll(), "Id", "Number", unit.ZoneId);

            return View();
        }

        //
        // GET: /unit/Edit/1
        public ActionResult Edit(Guid id)
        {
            var unit = UnitService.Get(id);

            if (unit == null) return HttpNotFound();

            ViewBag.ZoneList = new SelectList(ZoneService.GetAll(), "Id", "Number", unit.ZoneId);

            return View(unit);
        }

        //
        // POST: /unit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Unit unit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UnitService.Update(unit);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.ZoneList = new SelectList(ZoneService.GetAll(), "Id", "Number", unit.ZoneId);

            return View(unit);

        }

        //
        // GET: /unit/Delete/5
        public ActionResult Delete(Guid id)
        {
            try
            {
                var unit = UnitService.Get(id);
                if (unit == null)
                {
                    return HttpNotFound();
                }
                return View(unit);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //
        // POST: /unit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (!ModelState.IsValid) return View();
            UnitService.Delete(id);
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
                LoadFileService.LoadUnit(file);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
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
    public class ZoneController : Controller
    {
        public readonly IZoneService ZoneService;
        private readonly ILoadDataFileService LoadFileService;
        private readonly IDivisionService DivisionService;

        public ZoneController(IZoneService zoneService, ILoadDataFileService loadFileService, IDivisionService divisionService)
        {
            ZoneService = zoneService;
            LoadFileService = loadFileService;
            DivisionService = divisionService;
        }

        //
        // GET: /Zone/
        public ActionResult Index()
        {
            var ZoneList = ZoneService.GetAll();
            return View(ZoneList);
        }

        // GET: /Zone/Details/5
        public ActionResult Details(Guid id)
        {
            var Zone = ZoneService.Get(id);
            return View(Zone);
        }

        //
        // GET: /Zone/Create
        public ActionResult Create()
        {
            //Get the list of Roles
            ViewBag.DivisionList = new SelectList(DivisionService.GetAll(), "Id", "Number");

            return View(new Zone());
        }

        //
        // POST: /Zone/Create
        [HttpPost]
        public ActionResult Create(Zone zone)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ZoneService.AddOrUpdate(zone);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.DivisionList = new SelectList(DivisionService.GetAll(), "Id", "Number", zone.DivisionId);

            return View();
        }

        //
        // GET: /Zone/Edit/1
        public ActionResult Edit(Guid id)
        {
            var zone = ZoneService.Get(id);

            if (zone == null) return HttpNotFound();

            ViewBag.DivisionList = new SelectList(DivisionService.GetAll(), "Id", "Number", zone.DivisionId);

            return View(zone);
        }

        //
        // POST: /Zone/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Zone zone)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ZoneService.Update(zone);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.DivisionList = new SelectList(DivisionService.GetAll(), "Id", "Number", zone.DivisionId);

            return View(zone);

        }

        //
        // GET: /Zone/Delete/5
        public ActionResult Delete(Guid id)
        {
            try
            {
                var zone = ZoneService.Get(id);
                if (zone == null)
                {
                    return HttpNotFound();
                }
                return View(zone);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //
        // POST: /Zone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (!ModelState.IsValid) return View();
            ZoneService.Delete(id);
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
                LoadFileService.LoadZone(file);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
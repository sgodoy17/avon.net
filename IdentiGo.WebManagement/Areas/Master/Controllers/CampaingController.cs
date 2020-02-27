using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using IdentiGo.Transversal.Utilities;
using IdentiGo.Services.Master;
using IdentiGo.Domain.Entity.Master;
using IdentiGo.Transversal.Services;
using IdentiGo.WebManagement.Security;
using IdentiGo.Domain.Enums;

namespace IdentiGo.WebManagement.Areas.Master.Controllers
{
    [AuthorizeRoles(RoleName.Role3)]
    public class CampaingController : Controller
    {
        public readonly ICampaingService CampaingService;
        private readonly ILoadDataFileService LoadFileService;

        public CampaingController(ICampaingService campaingService, ILoadDataFileService loadFileService)
        {
            CampaingService = campaingService;
            LoadFileService = loadFileService;
        }

        //
        // GET: /Campaing/
        public ActionResult Index()
        {
            var campaingList = CampaingService.GetAll();
            return View(campaingList);
        }

        // GET: /Campaing/Details/5
        public ActionResult Details(Guid id)
        {
            var campaing = CampaingService.Get(id);
            return View(campaing);
        }

        //
        // GET: /Campaing/Create
        public ActionResult Create()
        {
            //Get the list of Roles
            return View(new Campaing());
        }

        //
        // POST: /Campaing/Create
        [HttpPost]
        public ActionResult Create(Campaing campaing)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CampaingService.AddOrUpdate(campaing);
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
        // GET: /Campaing/Edit/1
        public ActionResult Edit(Guid id)
        {
            var campaing = CampaingService.Get(id);
            if (campaing == null) return HttpNotFound();
            return View(campaing);
        }

        //
        // POST: /Campaing/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Campaing campaing)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CampaingService.Update(campaing);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(campaing);

        }

        //
        // GET: /Campaing/Delete/5
        public ActionResult Delete(Guid id)
        {
            try
            {
                var campaing = CampaingService.Get(id);
                if (campaing == null)
                {
                    return HttpNotFound();
                }
                return View(campaing);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //
        // POST: /Campaing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (!ModelState.IsValid) return View();
            CampaingService.Delete(id);
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
                ReadExcel.InitializeSheet(file.InputStream, Path.GetExtension(file.FileName));

                var list = ReadExcel.ConvertToList<Campaing>();

                CampaingService.SaveList(list);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
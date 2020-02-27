using System;
using System.Web.Mvc;
using IdentiGo.Domain.Entity.Master;
using IdentiGo.Services.Master;
using IdentiGo.WebManagement.Security;
using IdentiGo.Domain.Enums;

namespace IdentiGo.WebManagement.Areas.Master.Controllers
{
    [AuthorizeRoles(RoleName.Role3)]
    public class QuotaController : Controller
    {
        public readonly IQuotaService QuotaService;

        public QuotaController(IQuotaService quotaService)
        {
            QuotaService = quotaService;
        }

        //
        // GET: /OptionQuota/
        public ActionResult Index()
        {
            var quotaList = QuotaService.GetAll();
            return View(quotaList);
        }


        // GET: /OptionQuota/Details/5
        public ActionResult Details(Guid id)
        {
            var quota = QuotaService.Get(id);
            return View(quota);
        }

        //
        // GET: /OptionQuota/Create
        public ActionResult Create()
        {
            //Get the list of Roles
            return View(new Quota());
        }

        //
        // POST: /OptionQuota/Create
        [HttpPost]
        public ActionResult Create(Quota quota)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    QuotaService.Add(quota);
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
        // GET: /OptionQuota/Edit/1
        public ActionResult Edit(Guid id)
        {
            var quota = QuotaService.Get(id);
            if (quota == null) return HttpNotFound();
            return View(quota);
        }

        //
        // POST: /OptionQuota/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Quota quota)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    QuotaService.Update(quota);
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { ModelState.AddModelError("", ex.Message); }
            }
            return View(quota);

        }

        //
        // GET: /OptionQuota/Delete/5
        public ActionResult Delete(Guid id)
        {
            try
            {
                var quota = QuotaService.Get(id);
                if (quota == null)
                {
                    return HttpNotFound();
                }
                return View(quota);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //
        // POST: /OptionQuota/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (!ModelState.IsValid) return View();
            QuotaService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
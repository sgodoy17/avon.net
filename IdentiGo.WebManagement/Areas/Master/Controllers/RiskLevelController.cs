using System;
using System.Linq;
using System.Web.Mvc;
using IdentiGo.Domain.Entity.Master;
using IdentiGo.Services.Master;
using IdentiGo.WebManagement.Security;
using IdentiGo.Domain.Enums;

namespace IdentiGo.WebManagement.Areas.Master.Controllers
{
    [AuthorizeRoles(RoleName.Role3)]
    public class RiskLevelController : Controller
    {
        public readonly IRiskLevelService RiskLevelService;

        public readonly IQuotaService QuotaService;

        public RiskLevelController(IRiskLevelService riskLevelService, IQuotaService quotaService)
        {
            RiskLevelService = riskLevelService;
            QuotaService = quotaService;
        }

        public ActionResult Index()
        {
            var riskLevelList = RiskLevelService.GetAll();
            return View(riskLevelList);
        }


        public ActionResult Details(Guid id)
        {
            var riskLevel = RiskLevelService.Get(id);

            ViewBag.QuotaNames = riskLevel.Quota;

            return View(riskLevel);
        }

        public ActionResult Create()
        {
            ViewBag.QuotaId = new SelectList(QuotaService.GetAll(), "Name", "Name");

            return View(new RiskLevel());
        }

        [HttpPost]
        public ActionResult Create(RiskLevel riskLevel, params Guid[] SelectedQuota)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        RiskLevelService.Add(riskLevel);

                        if (SelectedQuota == null) return RedirectToAction("Index");

                        var quotas = QuotaService.GetById(SelectedQuota);
                        if (RiskLevelService.AddQuota(riskLevel.Id, quotas)) return RedirectToAction("Index");

                        ModelState.AddModelError("", "Failed to relate the quotas");

                        ViewBag.QuotaId = new SelectList(QuotaService.GetAll(), "Name", "Name");

                        return View();
                    }

                    ViewBag.QuotaId = new SelectList(QuotaService.GetAll(), "Name", "Name");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var riskLevel = RiskLevelService.Get(id);

            if (riskLevel == null) return HttpNotFound();

            var quotaList = riskLevel.Quota.Select(x => x.Id).ToList();
            riskLevel.QuotaList = QuotaService.GetAll().ToList().Select(x => new SelectListItem()
            {
                Selected = quotaList.Contains(x.Id),
                Text = x.Name,

                Value = x.Id.ToString(),
            });

            return View(riskLevel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RiskLevel riskLevel, params Guid[] selectedQuota)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    RiskLevelService.AddOrUpdate(riskLevel);

                    riskLevel = RiskLevelService.Get(riskLevel.Id);

                    var quotaList = riskLevel.Quota.Select(x => x.Id).ToList();

                    selectedQuota = selectedQuota ?? new Guid[] { };

                    var result = RiskLevelService.AddQuota(riskLevel.Id, QuotaService.GetById(selectedQuota));
                    if (!result) throw new Exception("Failed to relate the quota");

                    result = RiskLevelService.RemoveQuota(riskLevel.Id, QuotaService.GetById(quotaList.Except(selectedQuota).ToArray()));
                    if (!result) throw new Exception("Failed to remove the quota");

                    return RedirectToAction("Index");
                }
                catch (Exception ex) { ModelState.AddModelError("", ex.Message); }
            }

            return View(riskLevel);

        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                var riskLevel = RiskLevelService.Get(id);
                if (riskLevel == null)
                {
                    return HttpNotFound();
                }
                return View(riskLevel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (!ModelState.IsValid) return View();
            RiskLevelService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
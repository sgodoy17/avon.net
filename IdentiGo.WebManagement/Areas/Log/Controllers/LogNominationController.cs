using IdentiGo.Domain.Enums;
using IdentiGo.Services.General;
using IdentiGo.WebManagement.Areas.Log.Models;
using IdentiGo.WebManagement.Security;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IdentiGo.WebManagement.Areas.Log.Controllers
{
    [AuthorizeRoles(RoleName.Role4, RoleName.Role8)]
    public class LogNominationController : Controller
    {
        public readonly INominationService NominationService;
        public readonly INominationHistoricService NominationHistoricService;

        public LogNominationController(INominationService nominationService, 
            INominationHistoricService nominationHistoricService)
        {
            NominationService = nominationService;
            NominationHistoricService = nominationHistoricService;
        }

        public ActionResult Index()
        {
            ViewBag.Years = GetYearList();

            return View(new NominationHistoricModel());
        }

        [HttpPost]
        public ActionResult Index(NominationHistoricModel item)
        {
            if (item.DateYear != null)
            {
                item.DateStart = new DateTime(item.DateYear.Value, 1, 1, 1, 0, 0);
                item.DateEnd = new DateTime(item.DateYear.Value, 12, 31, 23, 0, 0);
            }

            item.HictoricNomination = NominationHistoricService.GetByRangeDate(item.DateStart, item.DateEnd);            
            ViewBag.Years = GetYearList();

            return View(item);
        }

        private List<SelectListItem> GetYearList()
        {
            List<SelectListItem> Years = new List<SelectListItem>();
            DateTime startYear = DateTime.Now;
            int endYear = NominationService.GetLastDate();

            while (startYear.Year >= endYear)
            {
                Years.Add(new SelectListItem { Text = startYear.Year.ToString(), Value = startYear.Year.ToString() });
                startYear = startYear.AddYears(-1);
            }

            return Years;
        }
    }
}
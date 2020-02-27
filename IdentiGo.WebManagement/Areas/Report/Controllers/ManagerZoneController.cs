using Component.Transversal.Extensions;
using IdentiGo.Services.General;
using IdentiGo.Services.Master;
using IdentiGo.Services.Security;
using IdentiGo.Transversal.Model;
using IdentiGo.Transversal.Services;
using IdentiGo.WebManagement.Areas.Report.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IdentiGo.WebManagement.Areas.Report.Controllers
{
    [Authorize]
    public class ManagerZoneController : Controller
    {
        public readonly IManagerZoneReportService ManagerZoneReportService;
        public readonly IUserService UserService;
        public readonly ICampaingService CampaingService;
        public readonly INominationService NominationService;
        public readonly IZoneService ZoneService;
        public readonly IUnitService UnitService;
        public readonly INominationHistoricService NominationHistoricService;

        public ManagerZoneController(IManagerZoneReportService managerZoneReportService,
            IUserService userService,
            ICampaingService campaingService,
            INominationService nominationService,
            IZoneService zoneService,
            IUnitService unitService,
            INominationHistoricService nominationHistoricService)
        {
            ManagerZoneReportService = managerZoneReportService;
            UserService = userService;
            CampaingService = campaingService;
            NominationService = nominationService;
            ZoneService = zoneService;
            UnitService = unitService;
            NominationHistoricService = nominationHistoricService;
        }

        // GET: /Nomination/
        public ActionResult Index()
        {
            var user = UserService.Get(User.Identity.GetGuidUserId());
            var zoneId = ZoneService.GetByCode(user?.CodeVerification?.ToString().PadLeft(11, '0'))?.Id;

            if (zoneId == null)
                throw new Exception("Este usuario no es un gerente de Zona");

            ManagerZoneReportModel item = new ManagerZoneReportModel
            {
                ZoneId = (Guid)zoneId
            };

            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number");
            ViewBag.Years = GetYearList();

            return View(item);
        }

        // GET: /Nomination/
        [HttpPost]
        public ActionResult Index(ManagerZoneReportModel item)
        {
            if (item.DateYear != null)
            {
                item.DateStart = new DateTime(item.DateYear.Value, 1, 1, 1, 0, 0);
                item.DateEnd = new DateTime(item.DateYear.Value, 12, 31, 23, 0, 0);
            }

            item = ManagerZoneReportService.ConsulReport(item);
            item.Init = true;
            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number", item.CampaingId);
            ViewBag.Years = GetYearList();

            return View(item);
        }

        // GET: /Nomination/
        public ActionResult IndexCandidate(Guid codeId, TypeConsulManagerZone type, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd, int? year)
        {
            CandidateReport item = new CandidateReport
            {
                UnitId = codeId,
                TypeConsulManagerZone = type,
                CampaingId = campaingId,
                DateStart = dateStart,
                DateEnd = dateEnd,
                DateYear = year
            };

            item = ManagerZoneReportService.ConsulCandidateReport(item);
            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number");
            ViewBag.Years = GetYearList();

            return View("IndexCandidate", item);
        }

        // GET: /Nomination/
        [HttpPost]
        public ActionResult IndexCandidate(CandidateReport item)
        {
            if (item.DateYear != null)
            {
                item.DateStart = new DateTime(item.DateYear.Value, 1, 1, 1, 0, 0);
                item.DateEnd = new DateTime(item.DateYear.Value, 12, 31, 23, 0, 0);
            }

            item = ManagerZoneReportService.ConsulCandidateReport(item);
            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number", item.CampaingId);
            ViewBag.Years = GetYearList();

            return View("IndexCandidate", item);
        }

        public ActionResult EditCandidate(Guid id, Guid codeId, TypeConsulManagerZone type, string campaing)
        {
            EditNomination item = new EditNomination
            {
                CodeId = codeId,
                TypeConsulManagerZone = type,
                CodeCampaing = campaing
            };

            item.Nomination = NominationService.Get(id);
            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number", item.CodeCampaing);
            ViewBag.UnitList = new SelectList(UnitService.GetMany(x => x.Zone.Id == item.Nomination.Zone.Id), "Id", "NumberCodeName", item.Nomination.UnitId);
            ViewBag.CodeUserList = new SelectList(UnitService.GetMany(x => x.Zone.Id == item.Nomination.Zone.Id), "Code", "NumberCodeName", item.Nomination.CodeUser);

            return View(item);
        }

        // GET: /Nomination/
        [HttpPost]
        public ActionResult EditCandidate(EditNomination item)
        {
            try
            {
                if (item.Nomination == null)
                    throw new Exception("No hay ningúna candidata");

                var currentItem = NominationService.GetReload(item.Nomination.Id);
                currentItem.UnitId = item.Nomination.UnitId;
                currentItem.CodeUser = item.Nomination.CodeUser;
                NominationService.AddOrUpdate(currentItem);

                return RedirectToAction("IndexCandidate", new { codeId = item.CodeId, type = item.TypeConsulManagerZone, campaing = item.CodeCampaing });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult DeleteCandidate(Guid id, Guid codeId, TypeConsulManagerZone type, string campaing)
        {
            EditNomination item = new EditNomination
            {
                CodeId = codeId,
                TypeConsulManagerZone = type,
                CodeCampaing = campaing
            };

            item.Nomination = NominationService.Get(id);
            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number", item.CodeCampaing);
            ViewBag.UnitList = new SelectList(UnitService.GetMany(x => x.Zone.Id == item.Nomination.Zone.Id), "Id", "NumberCodeName", item.Nomination.UnitId);
            ViewBag.CodeUserList = new SelectList(UnitService.GetMany(x => x.Zone.Id == item.Nomination.Zone.Id), "Code", "NumberCodeName", item.Nomination.CodeUser);

            return View(item);
        }

        // GET: /Nomination/
        [HttpPost]
        public ActionResult DeleteCandidate(EditNomination item)
        {
            try
            {
                NominationService.Delete(item.Nomination.Id);

                return RedirectToAction("IndexCandidate", new { codeId = item.CodeId, type = item.TypeConsulManagerZone, campaing = item.CodeCampaing });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult HistoricNomination(Guid id)
        {
            NominationHistoricModel nomination = new NominationHistoricModel
            {
                HictoricNomination = NominationHistoricService.GetByNomitationId(id)
            };

            return View(nomination);
        }

        private List<SelectListItem> GetYearList()
        {
            List<SelectListItem> Years = new List<SelectListItem>();
            DateTime startYear = DateTime.Now;
            int endYear = 2017;

            while (startYear.Year >= endYear)
            {
                Years.Add(new SelectListItem { Text = startYear.Year.ToString(), Value = startYear.Year.ToString() });
                startYear = startYear.AddYears(-1);
            }

            return Years;
        }
    }
}
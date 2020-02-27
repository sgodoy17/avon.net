using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Enums;
using IdentiGo.Services.General;
using IdentiGo.Services.Master;
using IdentiGo.Services.Security;
using IdentiGo.Transversal.Model;
using IdentiGo.Transversal.Services;
using IdentiGo.WebManagement.Areas.Report.Models;
using IdentiGo.WebManagement.Security;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Web.Mvc;

namespace IdentiGo.WebManagement.Areas.Report.Controllers
{
    [AuthorizeRoles(RoleName.Role3, RoleName.Role8)]
    public class AdminController : Controller
    {
        public readonly IAdminReportService AdminReportService;
        public readonly IUserService UserService;
        public readonly ICampaingService CampaingService;
        public readonly INominationService NominationService;
        public readonly IZoneService ZoneService;
        public readonly IUnitService UnitService;
        public readonly INominationHistoricService NominationHistoricService;

        public AdminController(IAdminReportService adminReportService,
            IUserService userService,
            ICampaingService campaingService,
            INominationService nominationService,
            IZoneService zoneService,
            IUnitService unitService,
            INominationHistoricService nominationHistoricService)
        {
            AdminReportService = adminReportService;
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
            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number");
            ViewBag.Years = GetYearList();

            return View(new AdminReportModel());
        }

        // GET: /Nomination/
        [HttpPost]
        public ActionResult Index(AdminReportModel item)
        {
            if (item.DateYear != null)
            {
                item.DateStart = new DateTime(item.DateYear.Value, 1, 1, 1, 0, 0);
                item.DateEnd = new DateTime(item.DateYear.Value, 12, 31, 23, 0, 0);
            }

            item = AdminReportService.ConsulReportZone(item);
            item.Init = true;
            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number", item.CampaingId);
            ViewBag.Years = GetYearList();

            return View(item);
        }

        // GET: /Nomination/
        public ActionResult IndexZone(Guid? zoneId = null, Guid? campaingId = null, DateTime? dateStart = null, DateTime? dateEnd = null, TypeConsulManagerZone typeConsult = TypeConsulManagerZone.Unit, int? year = null)
        {
            AdminReportModel item = new AdminReportModel { ZoneId = zoneId, CampaingId = campaingId, DateStart = dateStart, DateEnd = dateEnd, TypeConsulAdmin = typeConsult, DateYear = year };
            item = AdminReportService.ConsulReport(item);
            item.Init = true;
            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number", campaingId);
            ViewBag.ZoneList = new SelectList(ZoneService.GetAll(), "Id", "DisplayValue", zoneId);

            return View(item);
        }

        // GET: /Nomination/
        [HttpPost]
        public ActionResult IndexZone(AdminReportModel item)
        {
            item = AdminReportService.ConsulReport(item);
            item.Init = true;
            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number", item.CampaingId);
            ViewBag.ZoneList = new SelectList(ZoneService.GetAll(), "Id", "DisplayValue", item.ZoneId);

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
                ZoneId = UnitService.Get(codeId)?.ZoneId ?? ZoneService.Get(codeId)?.Id,
                DateStart = dateStart,
                DateEnd = dateEnd,
                DateYear = year
            };

            item = AdminReportService.ConsulCandidateReport(item);
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

            item = AdminReportService.ConsulCandidateReport(item);
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

                return RedirectToAction("IndexCandidate", new { codeId = item.CodeId, type = item.TypeConsulManagerZone,  campaing = item.CodeCampaing });
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

        [HttpPost]
        public FileResult ExportData(ExportDataViewModel item)
        {
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("SalesForce");
            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex);

            row.CreateCell(0).SetCellValue("Zona");
            row.CreateCell(1).SetCellValue("Código Empresaria");
            row.CreateCell(2).SetCellValue("Teléfono Empresaria");
            row.CreateCell(3).SetCellValue("Código de Verificación");
            row.CreateCell(4).SetCellValue("Documento");
            row.CreateCell(5).SetCellValue("Nombre(s)");
            row.CreateCell(6).SetCellValue("Apellidos(s)");
            row.CreateCell(7).SetCellValue("Teléfono Candidata");
            row.CreateCell(8).SetCellValue("Etapa del Proceso");
            row.CreateCell(9).SetCellValue("Estado del Proceso");
            row.CreateCell(10).SetCellValue("Estado del Documento");
            row.CreateCell(11).SetCellValue("Fecha de Creación");
            row.CreateCell(12).SetCellValue("Fecha Finalización");
            row.CreateCell(13).SetCellValue("Tipo de Proceso");
            row.CreateCell(14).SetCellValue("Campaña");

            rowIndex++;

            var nominationList = NominationService.GetByTypeStateDateRange(null, item.TypeState, item.ExportDateStart, item.ExportDateEnd);

            foreach (var nomination in nominationList)
            {
                row = sheet.CreateRow(rowIndex);

                row.CreateCell(0).SetCellValue(nomination.Zone.DisplayValue);
                row.CreateCell(1).SetCellValue(nomination.CodeUser);
                row.CreateCell(2).SetCellValue(nomination.PhoneAnswer);
                row.CreateCell(3).SetCellValue(nomination.CodeVerification == null ? "" : nomination.CodeVerification.ToString());

                if (nomination.Document == null)
                    row.CreateCell(4).SetCellValue("");
                else
                    row.CreateCell(4).SetCellValue(nomination.Document);

                row.CreateCell(5).SetCellValue(nomination.Name);
                row.CreateCell(6).SetCellValue(nomination.LastName);
                row.CreateCell(7).SetCellValue(nomination.PhoneAnswer);
                row.CreateCell(8).SetCellValue(GetDisplayName((new StageProccess()).GetType().GetField(nomination.StageProcess.ToString())));
                row.CreateCell(9).SetCellValue(GetDisplayName((new State()).GetType().GetField(nomination.State.ToString())));
                row.CreateCell(10).SetCellValue(GetDisplayName((new StateDocument()).GetType().GetField(nomination.StateDocument.ToString())));
                row.CreateCell(11).SetCellValue(nomination.DateCreated.ToString("dd/MM/yyyy"));
                row.CreateCell(12).SetCellValue(nomination.DateNomination?.ToString("dd/MM/yyyy"));
                row.CreateCell(13).SetCellValue(GetDisplayName((new TypeProcess()).GetType().GetField(nomination.TypeProcess.ToString())));
                row.CreateCell(14).SetCellValue(nomination.Campaing.Number);

                rowIndex++;
            }

            using (var exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                string saveAsFileName = string.Format("ExportCustomers.xlsx").Replace("/", "-");
                byte[] bytes = exportData.ToArray();

                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", saveAsFileName);
            }
        }

        public string GetDisplayName(FieldInfo field)
        {
            var attribute = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));

            return attribute.Name;
        }
    }
}
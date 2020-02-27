using Component.Transversal.Extensions;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Enums;
using IdentiGo.ExternalServices;
using IdentiGo.Services.General;
using IdentiGo.Services.Log;
using IdentiGo.Services.Master;
using IdentiGo.Services.Security;
using IdentiGo.WebManagement.Areas.General.Models;
using IdentiGo.WebManagement.Security;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace IdentiGo.WebManagement.Areas.General.Controllers
{
    [AuthorizeRoles(RoleName.Role6)]
    public class NominationZoneController : Controller
    {
        public readonly INominationService NominationService;
        public readonly IUserService UserService;
        public readonly IZoneService ZoneService;
        public readonly ICampaingService CampaingService;
        public readonly INominationResponseService NominationResponseService;
        public readonly IIntrasoftService IntrasoftService;
        public readonly ILogSMSService LogSMSService;

        public NominationZoneController(INominationService nominationService,
            IUserService userService,
            IZoneService zoneService,
            ICampaingService campaingService,
            INominationResponseService nominationResponseService,
            IIntrasoftService intrasoftService,
            ILogSMSService logSMSService)
        {
            NominationService = nominationService;
            UserService = userService;
            ZoneService = zoneService;
            CampaingService = campaingService;
            NominationResponseService = nominationResponseService;
            IntrasoftService = intrasoftService;
            LogSMSService = logSMSService;
        }

        // GET: /NominationZone/
        public ActionResult Index()
        {
            var user = UserService.Get(User.Identity.GetGuidUserId());
            var zoneId = ZoneService.GetByCode(user?.CodeVerification?.ToString().PadLeft(11, '0'))?.Id;

            if (zoneId == null)
                throw new Exception("Este usuario no es un gerente de zona");

            NominationZoneModel item = new NominationZoneModel
            {
                ZoneId = (Guid)zoneId
            };

            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number");

            return View(item);
        }

        [HttpPost]
        public ActionResult Index(NominationZoneModel nomination)
        {
            nomination.Nomination = NominationService.GetByCampaingZoneTypeStateDateRange(nomination.Document, nomination.ZoneId, nomination.CampaingId, nomination.TypeState, nomination.DateStart, nomination.DateEnd);
            nomination.Init = true;
            nomination.Total = nomination.Nomination?.Count() ?? 0;
            nomination.Invalid = nomination.TypeState == TypeState.Invalid ? nomination.Nomination.Count() : nomination.TypeState == TypeState.All ? NominationService.GetByCampaingZoneTypeStateDateRange(nomination.Document, nomination.ZoneId, nomination.CampaingId, TypeState.Invalid, nomination.DateStart, nomination.DateEnd).Count() : 0;
            nomination.Success = nomination.TypeState == TypeState.Valid ? nomination.Nomination.Count() : nomination.TypeState == TypeState.All ? NominationService.GetByCampaingZoneTypeStateDateRange(nomination.Document, nomination.ZoneId, nomination.CampaingId, TypeState.Valid, nomination.DateStart, nomination.DateEnd).Count() : 0;
            nomination.Pending = nomination.TypeState == TypeState.Pending ? nomination.Nomination.Count() : nomination.TypeState == TypeState.All ? NominationService.GetByCampaingZoneTypeStateDateRange(nomination.Document, nomination.ZoneId, nomination.CampaingId, TypeState.Pending, nomination.DateStart, nomination.DateEnd).Count() : 0;
            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number", nomination.CampaingId);

            return View(nomination);
        }

        // GET: /Nomination/Details/5
        public ActionResult Details(Guid id)
        {
            var Nomination = NominationService.Get(id);

            return View(Nomination);
        }

        [HttpGet]
        public ActionResult CandidateZoneResponse()
        {
            var user = UserService.Get(User.Identity.GetGuidUserId());

            var zoneId = ZoneService.GetByCode(user?.CodeVerification?.ToString().PadLeft(11, '0'))?.Id;

            if (zoneId == null)
                throw new Exception("Este usuario no es un gerente de zona");

            CandidateZoneResponseModel candidate = new CandidateZoneResponseModel
            {
                ZoneId = (Guid)zoneId
            };

            candidate.CandidateResponse = NominationResponseService.GetByZoneRangeDate(candidate.ZoneId, candidate.DateStart, candidate.DateEnd, candidate.TypeConsult);

            return View(candidate);
        }

        [HttpPost]
        public ActionResult CandidateZoneResponse(CandidateZoneResponseModel candidate)
        {
            candidate.CandidateResponse = NominationResponseService.GetByZoneRangeDate(candidate.ZoneId, candidate.DateStart, candidate.DateEnd, candidate.TypeConsult);

            return View(candidate);
        }

        [HttpGet]
        [AuthorizeRoles]
        public ActionResult Restart(Guid id)
        {
            var nomination = NominationService.GetById(id);

            return View(nomination);
        }

        [HttpPost]
        [AuthorizeRoles]
        public ActionResult Restart(Nomination item)
        {
            var currentNomination = NominationService.GetById(item.Id);

            currentNomination.StageProcess = StageProccess.Init;
            currentNomination.State = State.Default;
            currentNomination.StateDocument = StateDocument.Default;
            currentNomination.CodeUser = "";
            NominationService.AddOrUpdate(currentNomination);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeRoles]
        public ActionResult Message(Guid id)
        {
            var nomination = NominationService.GetById(id);

            return View(nomination);
        }

        [HttpPost]
        [AuthorizeRoles]
        public ActionResult Message(Nomination item)
        {
            var currentNomination = NominationService.GetById(item.Id);
            string phoneNumber = null;
            var scoreUser = currentNomination.Score.Split(',');
            var name = currentNomination.Name.Split(' ');

            string msg = string.Format("{0}, te damos la bienvenida a AVON, el código de verificación es {1}{2}{3}{4}.",
                string.IsNullOrEmpty(name[0]) ? "" : $" {name[0]}",
                currentNomination.CodeVerification,
                scoreUser.Length > 0 && !string.IsNullOrEmpty(scoreUser[0]) ? $", cupo primera campaña ${scoreUser[0]}" : "",
                scoreUser.Length > 1 ? $" segunda ${scoreUser[1]}" : "",
                scoreUser.Length > 2 ? $" tercera ${scoreUser[2]}" : "");

            //SmsService.Send(currentUser.PhoneNumber, msg);
            IntrasoftService.Send(currentNomination.PhoneNumber, msg);
            LogSMSService.Add("", phoneNumber ?? currentNomination.PhoneNumber, msg, currentNomination.Document, false);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ExportData()
        {
            ViewBag.CampaingList = new SelectList(CampaingService.GetAll(), "Id", "Number");

            return View(new ExportDataViewModel());
        }

        [HttpPost]
        public FileResult ExportData(ExportDataViewModel item)
        {
            var user = UserService.Get(User.Identity.GetGuidUserId());
            var zoneId = ZoneService.GetByCode(user?.CodeVerification?.ToString().PadLeft(11, '0'))?.Id;
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("SalesForce");
            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex);

            if (zoneId == null)
                throw new Exception("Este usuario no es un gerente de zona");

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

            var nominationList = NominationService.GetByCampaingZoneTypeStateDateRange(null, (Guid)zoneId, item.CampaingId, item.TypeState, item.ExportDateStart, item.ExportDateEnd);

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
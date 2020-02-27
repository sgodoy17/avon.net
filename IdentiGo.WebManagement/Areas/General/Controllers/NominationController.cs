using IdentiGo.Domain.DTO.App;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Enums;
using IdentiGo.ExternalServices;
using IdentiGo.Services.General;
using IdentiGo.Services.Log;
using IdentiGo.Transversal.IoC;
using IdentiGo.Transversal.Services;
using IdentiGo.Transversal.Utilities;
using IdentiGo.WebManagement.Areas.General.Models;
using IdentiGo.WebManagement.Security;
using Microsoft.Practices.Unity;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using NPOI.XSSF.UserModel;

namespace IdentiGo.WebManagement.Areas.General.Controllers
{
    [AuthorizeRoles(RoleName.Role5, RoleName.Role8)]
    public class NominationController : Controller
    {
        public readonly INominationService NominationService;
        public readonly ILogSMSService LogSMSService;
        public readonly ILogIVRService LogIVRService;
        public readonly INominationResponseService NominationResponseService;
        public readonly IIntrasoftService IntrasoftService;
        public readonly INominationManagerService NominationManagerService;
        public readonly INominationHistoricService NominationHistoricService;

        public NominationController(INominationService nominationService,
            ILogSMSService logSMSService,
            ILogIVRService logIVRService,
            INominationResponseService nominationResponseService,
            IIntrasoftService intrasoftService,
            INominationHistoricService nominationHistoricService)
        {
            IUnityContainer container = IoCFactory.GetUnityContainer();
            NominationManagerService = container.Resolve<INominationManagerService>();
            NominationService = nominationService;
            LogSMSService = logSMSService;
            LogIVRService = logIVRService;
            NominationResponseService = nominationResponseService;
            IntrasoftService = intrasoftService;
            NominationHistoricService = nominationHistoricService;
        }

        public ActionResult Index()
        {
            return View(new NominationModel());
        }

        [HttpPost]
        public ActionResult Index(NominationModel nomination)
        {
            nomination.Nomination = NominationService.GetByTypeStateDateRange(nomination.Document, nomination.TypeState, nomination.DateStart, nomination.DateEnd);
            nomination.Init = true;
            nomination.Total = nomination.Nomination?.Count() ?? 0;
            nomination.Invalid = nomination.TypeState == TypeState.Invalid ? nomination.Nomination.Count() : nomination.TypeState == TypeState.All ? NominationService.GetByTypeStateDateRange(nomination.Document, TypeState.Invalid, nomination.DateStart, nomination.DateEnd).Count() : 0;
            nomination.Success = nomination.TypeState == TypeState.Valid ? nomination.Nomination.Count() : nomination.TypeState == TypeState.All ? NominationService.GetByTypeStateDateRange(nomination.Document, TypeState.Valid, nomination.DateStart, nomination.DateEnd).Count() : 0;
            nomination.Pending = nomination.TypeState == TypeState.Pending ? nomination.Nomination.Count() : nomination.TypeState == TypeState.All ? NominationService.GetByTypeStateDateRange(nomination.Document, TypeState.Pending, nomination.DateStart, nomination.DateEnd).Count() : 0;

            return View(nomination);
        }

        public ActionResult Details(Guid id)
        {
            var Nomination = NominationService.Get(id);

            return View(Nomination);
        }
        
        [Authorize(Roles = RoleName.Role1)]
        public ActionResult Edit(Guid id)
        {
            var Nomination = NominationService.Get(id);
            if (Nomination == null) return HttpNotFound();
            return View(Nomination);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.Role1)]
        public ActionResult Edit(Nomination Nomination)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NominationService.Update(Nomination);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(Nomination);

        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                var Nomination = NominationService.Get(id);

                if (Nomination == null)
                {
                    return HttpNotFound();
                }

                return View(Nomination);
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
            NominationService.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CandidateResponse()
        {
            CandidateResponseModel candidate = new CandidateResponseModel();
            candidate.CandidateResponse = NominationResponseService.GetByRandeDate(candidate.DateStart, candidate.DateEnd, candidate.TypeConsult);

            return View(candidate);
        }

        [HttpPost]
        public ActionResult CandidateResponse(CandidateResponseModel candidate)
        {
            candidate.CandidateResponse = NominationResponseService.GetByRandeDate(candidate.DateStart, candidate.DateEnd, candidate.TypeConsult);

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

            if (currentNomination.CodeVerification != null)
            {
                NominationHistoricService.AddOrUpdate(new NominationHistoric
                {
                    NominationId = currentNomination.Id,
                    Date = DateTime.Now,
                    DateCreated = currentNomination.DateCreated,
                    CodeVerification = currentNomination.CodeVerification,
                    CampaingId = currentNomination.CampaingId,
                    DivisionId = currentNomination.DivisionId,
                    ZoneId = currentNomination.ZoneId,
                    UnitId = currentNomination.UnitId,
                    State = currentNomination.State,
                    StateDocument = currentNomination.StateDocument,
                    StageProcess = currentNomination.StageProcess,
                    PhoneNumber = currentNomination.PhoneNumber,
                    PhoneAnswer = currentNomination.PhoneAnswer,
                    CodeUser = currentNomination.CodeUser
                });
            }

            currentNomination.StageProcess = StageProccess.Init;
            currentNomination.State = State.Default;
            currentNomination.StateDocument = StateDocument.Default;
            currentNomination.CodeUser = "";
            currentNomination.CodeVerification = null;
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
            string msg = "";

            if (currentNomination.StateDocument == StateDocument.PagoAnticipado)
                msg = string.Format("{0}, te damos la bienvenida a AVON en pago contado durante 6 pedidos, disfruta credito en tu pedido 7 si cumples condiciones, el CV es {1}",
                    string.IsNullOrEmpty(name[0]) ? "" : $" {name[0]}",
                    currentNomination.CodeVerification);
            else if (currentNomination.StateDocument == StateDocument.PagoContado)
                msg = string.Format("{0}, te damos la bienvenida a AVON en pago contado durante 2 pedidos, disfruta credito en tu pedido 3 si cumples condiciones, el CV es {1}",
                    string.IsNullOrEmpty(name[0]) ? "" : $" {name[0]}",
                    currentNomination.CodeVerification);
            else
                msg = string.Format("{0}, te damos la bienvenida a AVON, el codigo de verificacion es {1}{2}{3}{4}.",
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
        public ActionResult LoadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadFile(HttpPostedFileBase file, RegisterValidationDto user)
        {
            try
            {
                ReadExcel.InitializeSheet(file.InputStream, Path.GetExtension(file.FileName));

                var list = ReadExcel.ConvertToList<NominationString>();

                foreach (var nomination in list)
                {
                    string message = string.Format("cxmg;{0};{1};{2};{3};{4};{5};",
                        nomination.CodeUser,
                        nomination.Document,
                        nomination.PhoneNumber,
                        nomination.TypePhone,
                        nomination.Campaing,                        
                        nomination.Genre);

                    user.PhoneAnswer = nomination.PhoneAnswer;
                    user.Message = message;
                    user.Message = user.Message.Replace(" ", "");
                    var splitMensaje = user.Message.Split(';');
                    user.CodeValidation = user.Message.Split(';')[0].ToLower();
                    NominationManagerService.NominationProcess(user);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult ExportData()
        {
            return View(new ExportDataViewModel());
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
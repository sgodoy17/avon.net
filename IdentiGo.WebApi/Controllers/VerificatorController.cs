using Component.Transversal.Utilities;
using IdentiGo.Domain.DTO.App;
using IdentiGo.Domain.Entity.General;
using IdentiGo.Domain.Enums;
using IdentiGo.ExternalServices;
using IdentiGo.Services.General;
using IdentiGo.Services.Log;
using IdentiGo.Services.Master;
using IdentiGo.Transversal.IoC;
using IdentiGo.Transversal.Services;
using IdentiGo.Transversal.Utilities;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web.Http;

namespace IdentiGo.WebApi.Controllers
{
    /// <summary>
    /// Class VerificatorController
    /// Provides the entire logic for the nomination process
    /// </summary>
    [Authorize]
    [RoutePrefix("api/verificator")]
    public class VerificatorController : ApiController
    {
        public readonly INominationManagerService NominationManagerService;
        public readonly IConfigService ConfigService;
        public readonly INominationService NominationService;
        public readonly ITestService TestService;
        public readonly IDivisionService DivisionService;
        public readonly IZoneService ZoneService;
        public readonly NombraAvonExternalService NombraAvonService;
        public readonly CifinExternalService CifinService;
        public readonly CifinExternalServiceTest CifinServiceTest;
        public readonly IIVRService IVRService;
        public readonly ILogSMSService LogSMSService;
        public readonly ILogIVRService LogIVRService;
        public readonly IUnitService UnitService;
        private readonly Config config;

        public VerificatorController()
        {
            try
            {
                IUnityContainer container = IoCFactory.GetUnityContainer();
                NominationManagerService = container.Resolve<INominationManagerService>();
                ConfigService = container.Resolve<IConfigService>();
                NominationService = container.Resolve<INominationService>();
                TestService = container.Resolve<ITestService>();
                DivisionService = container.Resolve<IDivisionService>();
                ZoneService = container.Resolve<IZoneService>();
                IVRService = container.Resolve<IIVRService>();
                LogSMSService = container.Resolve<ILogSMSService>();
                LogIVRService = container.Resolve<ILogIVRService>();
                UnitService = container.Resolve<IUnitService>();
                NombraAvonService = new NombraAvonExternalService();
                config = ConfigService.GetAll().FirstOrDefault();
                CifinService = new CifinExternalService(config);
                CifinServiceTest = new CifinExternalServiceTest(config);
            }
            catch (Exception ex)
            {
                EmailUtils.SendEmailError(config, ex.Message);
            }
        }

        [AllowAnonymous]
        [ActionName("GenerateValidation")]
        public IHttpActionResult PostGenerateValidation(RegisterValidationDto user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Message) || !user.Message.StartsWith(Params.codeBase, StringComparison.InvariantCultureIgnoreCase))
                    return Ok(new { Result = 0 });

                user.Message = user.Message.Replace(" ", "");
                var splitMensaje = user.Message.Split(';');
                user.CodeValidation = user.Message.Split(';')[0].ToLower();
                NominationManagerService.NominationProcess(user);

                return Ok(new { Result = "success", Code = user.CodeValidation, user.Message, user.PhoneAnswer });
            }
            catch (Exception ex)
            {
                EmailUtils.SendEmailError(config, ex.Message);

                return Ok(new { Result = 0, ex.Message });
            }
        }

        [AllowAnonymous]
        [ActionName("GenerateValidationIn")]
        public IHttpActionResult PostGenerateValidationIn(RegisterValidationDto user)
        {
            user.Message = user.Results[0].Text;
            user.PhoneAnswer = user.Results[0].From.Substring(user.Results[0].From.Length - 10);

            try
            {
                if (string.IsNullOrEmpty(user.Message) || !user.Message.StartsWith(Params.codeBase, StringComparison.InvariantCultureIgnoreCase))
                    return Ok(new { Result = 0 });

                user.Message = user.Message.Replace(" ", "");
                var splitMensaje = user.Message.Split(';');
                user.CodeValidation = user.Message.Split(';')[0].ToLower();
                NominationManagerService.NominationProcess(user);

                return Ok(new { Result = "success", Code = user.CodeValidation, user.Message, user.PhoneAnswer });
            }
            catch (Exception ex)
            {
                EmailUtils.SendEmailError(config, ex.Message);

                return Ok(new { Result = 0, ex.Message });
            }
        }

        [AllowAnonymous]
        [ActionName("ConfirmationIVR")]
        public IHttpActionResult GetConfirmationIVR(string PhoneAnswer, string CodeIVR, string Message)
        {
            try
            {
                LogIVRService.Add(CodeIVR, PhoneAnswer, Message);

                if (CodeIVR != Params.campidAuthorized.ToString() && CodeIVR != Params.campidConfirmation.ToString())
                    return Ok(new { Result = 0 });

                var user = new RegisterValidationDto
                {
                    PhoneAnswer = PhoneAnswer,
                    Message = Message,
                    CodeValidation = Message
                };

                NominationManagerService.NominationProcess(user);

                return Ok(new { Result = 1 });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = 0, ex.Message });
            }
        }

        [AllowAnonymous]
        [ActionName("IVR")]
        public IHttpActionResult GetIVR(string PhoneAnswer, string CodeIVR, string Message)
        {
            try
            {
                LogIVRService.Add(CodeIVR, PhoneAnswer, Message);

                if (CodeIVR != Params.campidAuthorized.ToString() && CodeIVR != Params.campidConfirmation.ToString())
                    return Ok(new { Result = 0 });

                var user = new RegisterValidationDto
                {
                    PhoneAnswer = PhoneAnswer,
                    Message = Message
                };

                NominationManagerService.NominationProcess(user);

                return Ok(new { Result = 1 });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = 0, ex.Message });
            }
        }

        [AllowAnonymous]
        [ActionName("MicrocolsaFile")]
        public IHttpActionResult PostMicrocolsaFile()
        {
            try
            {
                FileUtil.SendFileFTP();

                return Ok(new { Result = 1, Message = "" });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = 0, ex.Message });
            }
        }

        #region TestService

        [AllowAnonymous]
        [ActionName("IVR")]
        public IHttpActionResult PostIVR(RegisterValidationDto user)
        {
            try
            {
                IVRService.Send(user.PhoneAnswer, int.Parse(user.CodeIVR));

                return Ok(1);
            }
            catch (Exception ex)
            {
                return Ok(new { Result = 0, ex.Message });
            }
        }

        [AllowAnonymous]
        [ActionName("ConfirmationIVR")]
        public IHttpActionResult PostConfirmationIVR(RegisterValidationDto user)
        {
            try
            {
                return Ok(1);
            }
            catch (Exception ex)
            {
                return Ok(new { Result = 0, ex.Message });
            }
        }

        [AllowAnonymous]
        [ActionName("ValidateAvon")]
        public IHttpActionResult GetValidateAvon(string document)
        {
            try
            {
                var result = NombraAvonService.ConsultDocument(document);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new { Result = 0, ex.Message, PhoneAnswer = "" });
            }
        }

        [AllowAnonymous]
        [ActionName("ValidateAvonPago")]
        public IHttpActionResult GetValidateAvonPago(string document)
        {
            try
            {
                var currentUser = NominationService.GetLastByDocument(document);

                if (currentUser != null)
                {
                    int doc = int.Parse(document);
                    var risk = currentUser.AvonRiskLevel.ToString();
                    var genre = currentUser.Sex == Gender.Male ? "M" : currentUser.Sex == Gender.Female ? "F" : "F";
                    var division = int.Parse(DivisionService.Get(currentUser.DivisionId)?.Number);
                    var zone = int.Parse(ZoneService.Get(currentUser.ZoneId)?.Number);
                    var unit = currentUser.UnitId != null ? int.Parse(UnitService.Get(currentUser.UnitId)?.Number) : -1;
                    var today = DateTime.Today.Year;
                    var age = currentUser.InfoCifin.FirstOrDefault().RangoEdad != null ? currentUser.InfoCifin.FirstOrDefault().RangoEdad.Contains("Mas") ? currentUser.InfoCifin.FirstOrDefault().RangoEdad.Split(' ')[1] : currentUser.InfoCifin.FirstOrDefault().RangoEdad.Split('-')[0] : "18";
                    var year = today - int.Parse(age);
                    var birth_date = int.Parse(year.ToString() + "0101");

                    var result = NombraAvonService.PagoContado(doc, genre, risk, birth_date, division, zone, unit);

                    CashPaymentEnum cashPayment = (CashPaymentEnum)result;
                    string stringValue = Enum.GetName(typeof(CashPaymentEnum), result);

                    return Ok(new { Status = cashPayment, Value = stringValue });
                }
                else
                {
                    return Ok(new { Status = -1, Value = "Error, documento no existente." });
                }
                
            } catch(Exception ex)
            {
                return Ok(new { Result = 0, ex.Message, PhoneAnswer = "" });
            }
        }

        [AllowAnonymous]
        [ActionName("ValidateCIFIN")]
        public IHttpActionResult GetValidateCIFIN(string document)
        {
            try
            {
                var result = CifinService.ConsultInformacionService(document);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new { Result = 0, ex.Message, PhoneAnswer = "" });
            }
        }

        [AllowAnonymous]
        [ActionName("ValidateProspecta")]
        public IHttpActionResult GetValidateProspecta(string document)
        {
            try
            {
                var result = CifinService.ConsultProspecta(document);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new { Result = 0, ex.Message, PhoneAnswer = "" });
            }
        }

        [AllowAnonymous]
        [ActionName("ValidateProspectaPlus")]
        public IHttpActionResult GetValidateProspectaPlus(string document)
        {
            try
            {
                var result = CifinServiceTest.ConsultProspecta(document);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new { Result = 0, ex.Message, PhoneAnswer = "" });
            }
        }

        [AllowAnonymous]
        [ActionName("ValidateCodeAvon")]
        public IHttpActionResult GetValidateCodeAvon(string code)
        {
            try
            {
                var result = NombraAvonService.ConsultCode(code);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new { Result = 0, ex.Message, PhoneAnswer = "" });
            }
        }

        [AllowAnonymous]
        [ActionName("ValidateClientAvon")]
        public IHttpActionResult GetValidateClientAvon(string document)
        {
            try
            {
                var result = NombraAvonService.ConsultDocument(document);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new { Result = 0, ex.Message, PhoneAnswer = "" });
            }
        }

        [AllowAnonymous]
        [ActionName("ConfigZoneUnitNomination")]
        public IHttpActionResult PostConfigZoneUnitNomination()
        {
            try
            {
                foreach (var item in NominationService.GetAll())
                {
                    var unitNumber = item.Unit?.Number;
                    var zoneId = item.ZoneId;

                    if (zoneId != null && unitNumber != null)
                    {
                        item.UnitId = UnitService.GetFirsOne(x => x.ZoneId == zoneId && x.Number == unitNumber)?.Id;
                        NominationService.AddOrUpdate(item);
                    }
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(new { Result = 0, ex.Message, PhoneAnswer = "" });
            }
        }

        [AllowAnonymous]
        [ActionName("ConsultDocument")]
        public IHttpActionResult ConsultDocument(string document)
        {
            var result = NominationService.GetLastByDocument(document);

            return Ok(result);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}

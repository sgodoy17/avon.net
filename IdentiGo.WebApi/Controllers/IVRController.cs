using Component.Transversal.Utilities;
using IdentiGo.Domain.DTO.App;
using IdentiGo.Domain.Entity.General;
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
    [RoutePrefix("api/ivr")]
    public class IVRController : ApiController
    {
        public readonly INominationManagerService NominationManagerService;
        public readonly IConfigService ConfigService;
        public readonly INominationService UserValidationService;
        public readonly ITestService TestService;
        public readonly IDivisionService DivisionService;
        public readonly NombraAvonExternalService NombraAvonService;
        public readonly IIVRService IVRService;
        public readonly ILogSMSService LogSMSService;
        public readonly ILogIVRService LogIVRService;

        private Config config;

        public IVRController()
        {
            try
            {
                IUnityContainer container = IoCFactory.GetUnityContainer();
                NominationManagerService = container.Resolve<INominationManagerService>();
                ConfigService = container.Resolve<IConfigService>();
                UserValidationService = container.Resolve<INominationService>();
                TestService = container.Resolve<ITestService>();
                DivisionService = container.Resolve<IDivisionService>();
                IVRService = container.Resolve<IIVRService>();
                LogSMSService = container.Resolve<ILogSMSService>();
                LogIVRService = container.Resolve<ILogIVRService>();

                NombraAvonService = new NombraAvonExternalService();

                config = ConfigService.GetAll().FirstOrDefault();
            }
            catch (Exception ex)
            {
                EmailUtils.SendEmailError(config, ex.Message);
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
                EmailUtils.SendEmailError(config, ex.Message);
                return Ok(new { Result = 0 });
            }
        }
    }
}

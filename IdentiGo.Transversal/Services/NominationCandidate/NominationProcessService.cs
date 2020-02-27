using Component.Transversal.Utilities;
using IdentiGo.Domain.DTO.App;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Enums;
using IdentiGo.ExternalServices;
using IdentiGo.Services.CIFIN;
using IdentiGo.Services.General;
using IdentiGo.Services.Log;
using IdentiGo.Services.Master;
using System;
using System.Text.RegularExpressions;

namespace IdentiGo.Transversal.Services.NominationCandidate
{
    public interface INominationProcessService
    {
        string Login(string[] dataLogin);

        void AppiNomination(RegisterValidationDto item, ref Nomination currentUser);

        void ManualNomination(RegisterValidationDto item, ref Nomination currentUser);
    }

    public class NominationProcessService : INominationProcessService
    {
        public readonly INominationService UserValidationService;
        public readonly IRiskLevelService RiskLevelService;
        public readonly IValidadorPlusService ValidadorPlusService;
        public readonly IProspectaService ProspectaService;
        public readonly ICampaingService CampaingService;
        public readonly IZoneService ZoneService;
        public readonly IUnitService UnitService;
        public readonly IDivisionService DivisionService;
        public readonly ILogSMSService LogSMSService;
        public readonly NombraAvonExternalService NombraAvonService = new NombraAvonExternalService();

        public NominationProcessService(INominationService userValidationService,
            IRiskLevelService riskLevelService,
            IValidadorPlusService validadorPlusService,
            IProspectaService prospectaService,
            ICampaingService campaingService,
            IUnitService unitService,
            IDivisionService divisionService,
            IZoneService zoneService,
            ILogSMSService logSMSService)
        {
            UserValidationService = userValidationService;
            RiskLevelService = riskLevelService;
            ValidadorPlusService = validadorPlusService;
            ProspectaService = prospectaService;
            CampaingService = campaingService;
            UnitService = unitService;
            DivisionService = divisionService;
            ZoneService = zoneService;
            LogSMSService = logSMSService;
        }

        public string Login(string[] dataLogin)
        {
            var codeValidation = dataLogin.Length > 1 ? dataLogin[0] : "";
            var document = dataLogin.Length > 1 ? dataLogin[1] : "";
            var codeUser = dataLogin.Length > 2 ? dataLogin[2].PadLeft(11, '0') : "";

            if (string.IsNullOrEmpty(document))
                return $"{codeValidation};0;El campo documento es requerido";

            if (string.IsNullOrEmpty(codeUser))
                return $"{codeValidation};0;El campo código usuario es requerido";

            var result = NombraAvonService.ConsultCode(codeUser);

            if (!string.IsNullOrEmpty(result.errorCode) && result.errorCode != "00")
                return $"{codeValidation};0;Servicio de Avon fuera de línea. Por favor comunicate con servicio a la representante";

            var documentAvon = result.output6?.Split('.')[0];

            if (documentAvon != document)
                return $"{codeValidation};0;Usuario invalido";

            if (string.IsNullOrWhiteSpace(result.output1.Trim()))
                return $"{codeValidation};0;El usuario debe tener una zona asignada";

            return $"{codeValidation};1;{result.output3}";
        }

        public void AppiNomination(RegisterValidationDto item, ref Nomination currentUser)
        {
            var userValidation = GenerateItem(item);
            ValidationData(ref userValidation);
            currentUser = UserValidationService.GetLastByDocument(userValidation.Document);

            if (currentUser == null)
                currentUser = userValidation;
            else if (currentUser.CodeUser != userValidation.CodeUser)
                throw new Exception("Esta candidata esta siendo gestionada por otra empresaria. Por favor comunicate con tu gerente de zona");

            UpdateData(ref currentUser, userValidation);

            if (!userValidation.Consult || currentUser.StageProcess == StageProccess.Init)
            {
                switch (currentUser.StageProcess)
                {
                    case StageProccess.Init:
                        currentUser.StageProcess = StageProccess.DataAuthorization;
                        currentUser.State = State.Default;
                        currentUser.DateUpdate = DateTime.Now;
                        break;
                    case StageProccess.DataAuthorization:
                        DataAuthorization(ref currentUser, userValidation.State);
                        break;
                    case StageProccess.ValidaAvon:
                        ValidateAvon(ref currentUser);
                        break;
                    case StageProccess.CandidateConfirmation:
                        CandidateConfirmation(ref currentUser, userValidation.State);
                        break;
                    case StageProccess.Score:
                        break;
                    case StageProccess.Finished:
                        break;
                    default:
                        throw new Exception("El campo StageProccess es invalido");
                }

                UserValidationService.AddOrUpdate(currentUser);
            }
        }

        public void ManualNomination(RegisterValidationDto item, ref Nomination currentUser)
        {
            var userValidation = GenerateItemManual(item);
            ValidationData(ref userValidation);
            currentUser = UserValidationService.GetLastByDocument(userValidation.Document);

            if (currentUser == null)
                currentUser = userValidation;
            else if (currentUser.CodeUser != userValidation.CodeUser)
                throw new Exception("Esta candidata esta siendo gestionada por otra empresaria. Por favor comunicate con tu gerente de zona");

            UpdateData(ref currentUser, userValidation);

            switch (currentUser.StageProcess)
            {
                case StageProccess.DataAuthorization:
                    DataAuthorization(ref currentUser, userValidation.State);
                    break;
                case StageProccess.ValidaAvon:
                    ValidateAvon(ref currentUser);
                    break;
                case StageProccess.CandidateConfirmation:
                    CandidateConfirmation(ref currentUser, userValidation.State);
                    break;
                case StageProccess.Score:
                    break;
                case StageProccess.Finished:
                    break;
                default:
                    throw new Exception("El campo StageProccess es invalido");
            }

            UserValidationService.AddOrUpdate(currentUser);
        }

        private Nomination GenerateItem(RegisterValidationDto item)
        {
            var message = item.Message.Split(';');

            return new Nomination
            {
                CodeUser = message.Length > 1 ? message[1].PadLeft(11, '0') : "",
                Document = message.Length > 2 ? message[2] : "",
                CampaingId = message.Length > 3 ? CampaingService.GetByNumber(message[3])?.Id : null,
                PhoneNumber = message.Length > 4 ? Regex.Replace(message[4], @"[^\d]", "") : "",
                TypePhone = message.Length > 5 ? message[5] == "2" ? TypePhone.Fixed : TypePhone.Cell : TypePhone.Cell,
                Consult = message.Length > 6 ? message[6] == "0" : true,
                //State = convertState(message.Length > 7 ? message[7] : ""),
                PhoneAnswer = item.PhoneAnswer,
                TypeProcess = message[0].ToLower() == Params.codeVI  ? TypeProcess.AppiIOS: TypeProcess.Appi,
                StageProcess = StageProccess.Init
            };
        }

        private Nomination GenerateItemManual(RegisterValidationDto item)
        {
            var message = item.Message.Split(';');

            if (message.Length < 6)
                throw new Exception("Estructura de datos invalida, verifique y vuelva a intentar");

            return new Nomination
            {
                CodeUser = message.Length > 1 ? message[1].PadLeft(11, '0') : "",
                Document = message.Length > 2 ? message[2] : "",
                PhoneNumber = message.Length > 3 ? Regex.Replace(message[3], @"[^\d]", "") : "",
                TypePhone = message.Length > 4 ? message[4] == "tel" ? TypePhone.Fixed : TypePhone.Cell : TypePhone.Cell,
                CampaingId = message.Length > 5 ? CampaingService.GetByNumber(message[5])?.Id : null,
                PhoneAnswer = item.PhoneAnswer,
                TypeProcess = TypeProcess.Manual,
                StageProcess = StageProccess.DataAuthorization
            };
        }

        private void ValidationData(ref Nomination user)
        {
            if (string.IsNullOrWhiteSpace(user.PhoneAnswer))
                throw new Exception("El número de teléfono de quien realiza el nombramiento es requerido");

            //if (UserValidationService.ValidatePhoneExist(user.Document, user.PhoneNumber))
            //    throw new Exception($"El numero de teléfono {user.PhoneNumber} se encuentra registrado con otra candidata");

            if (user.CampaingId == null)
                throw new Exception("La campaña ingresada no es valida");

            var result = NombraAvonService.ConsultCode(user.CodeUser);
            user.DivisionId = DivisionService.GetByNumber(result.output5.Trim())?.Id;
            user.ZoneId = ZoneService.GetByNumber(result.output1.Trim())?.Id;
            user.UnitId = UnitService.GetByCodeUnitCodeZone(result.output2.Trim(), result.output1.Trim())?.Id;

            if (user.ZoneId == null)
                throw new Exception("El usuario que realiza el nombramiento debe tener una zona asignada");

            var zone = ZoneService.Get(user.ZoneId);

            if (zone.SelfSend == false)
            {
                if (user.PhoneAnswer == user.PhoneNumber)
                    throw new Exception("Recuerda que no puedes usar tu numero de celular para la validacion de la candidata representante");
            }

            //var number = LogSMSService.GetByPhoneNumber(user.PhoneAnswer);

            //if (zone.NumberTrys == 0)
            //{

            //}
            //else if (number >= zone.NumberTrys)
            //    throw new Exception("Este numero de telefono ha sido utilizado para nombrar a otra representante, intenta con nuevo numero de telefono");

            var number = UserValidationService.GetByPhoneNumber(user.PhoneNumber);

            if (number == 0)
            {
                number = number + 1;
            }
            else if (number > 0)
            {
                var exits = UserValidationService.ValidatePhoneNumber(user.Document, user.PhoneNumber);

                if (exits)
                {
                    number = number - 1;
                }
                else
                {

                }
            }

            if (zone.NumberTrys == 0)
            {

            }
            else if (number < zone.NumberTrys)
            {

            }
            else
            {
                //throw new Exception("Este numero de telefono ha sido utilizado para nombrar a otra representante, intenta con nuevo numero de telefono");
                throw new Exception("Este telefono se uso para nombrar a otra Representante. Ingresa otro numero de telefono de la candidata donde recibira el codigo de verificacion");
            }
        }

        private void UpdateData(ref Nomination currentUser, Nomination user)
        {
            currentUser.TypeProcess = user.TypeProcess;
            currentUser.TypePhone = user.TypePhone;
            currentUser.CampaingId = user.CampaingId;
            currentUser.DivisionId = user.DivisionId;
            currentUser.ZoneId = user.ZoneId;
            currentUser.UnitId = user.UnitId;
            currentUser.PhoneAnswer = user.PhoneAnswer;
            currentUser.PhoneNumber = string.IsNullOrEmpty(user.PhoneNumber) ? currentUser.PhoneNumber : user.PhoneNumber;
            currentUser.Consult = user.Consult;
        }

        private void DataAuthorization(ref Nomination item, State state)
        {
            if (item.State == State.Default || item.State == State.Pending || item.State == State.Invalid)
            {
                item.State = State.Pending;
                item.DateUpdate = DateTime.Now;
            }
            else
                throw new Exception("Estado del proceso invalido");
        }

        private void ValidateAvon(ref Nomination item)
        {
            if (item.State == State.Success)
            {
                item.State = State.Default;
                item.StageProcess = StageProccess.CandidateConfirmation;
                CandidateConfirmation(ref item, item.State);
            }
            else
                return;
        }

        private void CandidateConfirmation(ref Nomination item, State state)
        {
            if (item.State == State.Default || item.State == State.Pending || item.State == State.Invalid)
            {
                item.State = State.Pending;
                item.StageProcess = StageProccess.CandidateConfirmation;
            }
            else
                throw new Exception("Estado del proceso invalido");
        }

        private string ConvertUserValidationCode(string codeUser)
        {
            if (!int.TryParse(codeUser, out int number))
                return codeUser;

            return string.Format("{0:00000000000}", number);
        }
    }
}

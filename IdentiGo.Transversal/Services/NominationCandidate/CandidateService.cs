using IdentiGo.Domain.DTO.App;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Enums;
using IdentiGo.ExternalServices;
using IdentiGo.Services.CIFIN;
using IdentiGo.Services.General;
using IdentiGo.Services.Log;
using IdentiGo.Services.Master;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace IdentiGo.Transversal.Services.NominationCandidate
{
    /// <summary>
    /// Interface ICandidateService
    /// Provides interface for NominationCandidate service
    /// </summary>
    public interface ICandidateService
    {
        void NominationCandidate(RegisterValidationDto item, ref Nomination currentUser);
    }

    /// <summary>
    /// Class CandidateService
    /// </summary>
    public class CandidateService : ICandidateService
    {
        public readonly INominationService NominationService;
        public readonly IRiskLevelService RiskLevelService;
        public readonly IValidadorPlusService ValidadorPlusService;
        public readonly IProspectaService ProspectaService;
        public readonly INominationResponseService NominationResponseService;
        public readonly IConfigService ConfigService;        
        public readonly IIVRService IvrService;
        public readonly ISMSService SmsService;
        public readonly IZoneService ZoneService;
        public readonly ILogSMSService LogSMSService;
        public readonly NombraAvonExternalService NombraAvonService = new NombraAvonExternalService();
        public readonly CifinExternalService CifinService;

        /// <summary>
        /// Constructor CandidateService
        /// Set services references
        /// </summary>
        /// <param name="nominationService">Call the interface of nomination process service</param>
        /// <param name="riskLevelService">Call the interface of risk level service</param>
        /// <param name="validadorPlusService">Call the interface of validator plus service</param>
        /// <param name="prospectaService">Call the interface of prospecta service</param>
        /// <param name="nominationResponseService">Call the interface of nomination response service</param>
        /// <param name="confingService">Call the interface of config service</param>
        /// <param name="zoneService">Call the interface of zone service</param>
        /// <param name="logSMSService">Call the interface of log sms service</param>
        public CandidateService(INominationService nominationService,
            IRiskLevelService riskLevelService,
            IValidadorPlusService validadorPlusService,
            IProspectaService prospectaService,
            INominationResponseService nominationResponseService,
            IConfigService confingService,
            IZoneService zoneService,
            ILogSMSService logSMSService)
        {
            NominationService = nominationService;
            RiskLevelService = riskLevelService;
            ValidadorPlusService = validadorPlusService;
            ProspectaService = prospectaService;
            NominationResponseService = nominationResponseService;
            ConfigService = confingService;
            ZoneService = zoneService;
            LogSMSService = logSMSService;
            var config = ConfigService.GetAll().FirstOrDefault();
            CifinService = new CifinExternalService(config);
        }

        /// <summary>
        /// Method NominationCandidate
        /// Provides the entire logic for nominate the candidate
        /// </summary>
        /// <param name="item">User's information</param>
        /// <param name="currentUser">Current user's nomination information</param>
        public void NominationCandidate(RegisterValidationDto item, ref Nomination currentUser)
        {
            item.State = item.Message.Equals("1") || item.Message.Equals("cx1", StringComparison.InvariantCultureIgnoreCase) ? State.Success : State.Invalid;

            if (string.IsNullOrWhiteSpace(item.PhoneAnswer))
                throw new Exception("campo PhoneAnswer es invalido");

            currentUser = NominationService.GetLastByPhoneNumber(item.PhoneAnswer);

            if (currentUser == null) throw new Exception("el teléfono no se ecuentra registrado");

            if (currentUser.ZoneId == null)
                throw new Exception("El usuario que realiza el nombramiento debe tener una zona asignada");

            var zone = ZoneService.Get(currentUser.ZoneId);

            if (zone.SelfSend == false)
            {
                if (currentUser.PhoneAnswer == item.PhoneAnswer)
                    throw new Exception("Recuerda que no puedes usar tu numero de celular para la validacion de la candidata representante");
            }

            var number = NominationService.GetByPhoneNumber(item.PhoneAnswer);

            if (number == 0)
            {
                number = number + 1;
            }
            else if (number > 0)
            {
                var exits = NominationService.ValidatePhoneNumber(currentUser.Document, item.PhoneAnswer);

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
            else if (number <= zone.NumberTrys)
            {

            }
            else
            {
                //throw new Exception("Este numero de telefono ha sido utilizado para nombrar a otra representante, intenta con nuevo numero de telefono");
                throw new Exception("Este telefono se uso para nombrar a otra Representante. Ingresa otro numero de telefono de la candidata donde recibira el codigo de verificacion");
            }

            switch (currentUser.StageProcess)
            {
                case StageProccess.DataAuthorization:
                    AddNominationResponse(currentUser, item.Message);
                    DataAuthorization(ref currentUser, item.State);
                    break;
                default:
                    throw new Exception("La candidata no se encuentra en ningún proceso de confirmación");
            }

            NominationService.AddOrUpdate(currentUser);
        }

        /// <summary>
        /// Method AddNominationResponse
        /// Save the response of the nomination in an object and then, to database
        /// </summary>
        /// <param name="item">Nomination information</param>
        /// <param name="message">Message of the response</param>
        public void AddNominationResponse(Nomination item, string message)
        {
            NominationResponseService.AddOrUpdate(new NominationResponse { Date = DateTime.Now, NominationId = item.Id, Message = message, Stage = item.StageProcess });
        }

        /// <summary>
        /// Method DataAuthorization
        /// Provides the logic for authorization the data of the candidate
        /// </summary>
        /// <param name="item">Nomination information</param>
        /// <param name="state">State of the nomination</param>
        private void DataAuthorization(ref Nomination item, State state)
        {
            if (item.State != State.Pending)
                return;

            if (state == State.Success)
                ValidateAvon(ref item);
            else
            {
                item.State = State.Invalid;
                item.DateNomination = DateTime.Now;
            }

            item.DateUpdate = DateTime.Now;
        }

        private void ValidateAvon(ref Nomination item)
        {
            var result = NombraAvonService.ConsultDocument(item.Document);

            if (!string.IsNullOrEmpty(result.errorCode?.Trim()) && result.errorCode != "00")
                throw new Exception($"ValidaAvonService Cod:{result.errorCode}");

            var state = result.output1.Trim(' ');

            if (state == "2" || state == "4" || state == "5" || state == "6")
            {
                item.State = State.Success;
                item.StateDocument = state == "2" ? StateDocument.PagoAnticipado : state == "4" ? StateDocument.WriteOff : StateDocument.New;
                item.StageProcess = StageProccess.ValidaAvon;
                ValidateDocument(ref item);
            }
            else
            {
                if (state == "1")
                    item.StateDocument = StateDocument.Collection;
                if (state == "3")
                    item.StateDocument = StateDocument.Activa;
                //if (state == "5")
                //    item.StateDocument = StateDocument.Inactiva;
                if (state == "7")
                    item.StateDocument = StateDocument.ListOFAC;

                item.State = State.Invalid;
                item.StageProcess = StageProccess.ValidaAvon;
                item.DateNomination = DateTime.Now;
            }

            item.DateUpdate = DateTime.Now;
        }

        public void ValidateDocument(ref Nomination item)
        {
            var validadorPlus = CifinService.ConsultInformacionService(item.Document);

            if (validadorPlus == null)
                throw new Exception("La cedula no retorno datos en CFN");

            CofigureName(ref item, validadorPlus.NombreTitular);

            validadorPlus.NominationId = item.Id;
            ValidadorPlusService.AddOrUpdate(validadorPlus);

            if (validadorPlus?.Estado?.Equals("vigente", StringComparison.InvariantCultureIgnoreCase) == false)
            {
                item.StageProcess = StageProccess.ValidaAvon;
                item.State = State.Invalid;
                item.StateDocument = StateDocument.NoVigente;
                item.DateNomination = DateTime.Now;
            }
            else
            {
                item.StageProcess = StageProccess.ValidaAvon;
                item.State = State.Success;
                item.CodeVerification = NominationService.GenerateCodeVerificacion(item.Document);
                CalculateScore(ref item);
            }
        }

        private void CandidateConfirmation(ref Nomination item, State state)
        {
            if (item.State != State.Pending)
                throw new Exception("La candidata no se encuentra en proceso de confirmación");

            if (state == State.Success)
            {
                item.State = State.Default;
                item.StageProcess = StageProccess.Finished;
                item.CodeVerification = NominationService.GenerateCodeVerificacion(item.Document);
                CalculateScore(ref item);
            }
            else
            {
                item.State = State.Invalid;
                item.DateNomination = DateTime.Now;
            }
        }

        public void CofigureName(ref Nomination item, string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            var names = Regex.Replace(name, @"\s+", " ").Split(' ');

            if (names.Length == 1)
                item.Name = names[0];
            else if (names.Length == 2)
            {
                item.LastName = names[0];
                item.Name = names[1];
            }
            else if (names.Length == 3)
            {
                item.LastName = $"{names[0]} {names[1]}";
                item.Name = names[2];
            }
            else if (names.Length > 3)
            {
                item.Name = "";
                item.LastName = "";

                for (int i = names.Length - 1; i >= 0; i--)
                {
                    if (i > 1)
                        item.Name = $"{names[i]} {item.Name}";
                    else
                        item.LastName = $"{names[i]} {item.LastName}";
                }
            }

        }

        public void CalculateScore(ref Nomination item)
        {
            item.Score = "";
            item.StageProcess = StageProccess.Finished;
            item.State = State.Success;
            item.DateNomination = DateTime.Now;

            if (item.StateDocument == StateDocument.PagoAnticipado)
            {
                item.Score = "0";
                item.RiskLevelId = null;

                return;
            }

            var infoProspecta = CifinService.ConsultProspecta(item.Document);

            if (infoProspecta != null)
            {
                infoProspecta.NominationId = item.Id;
                ProspectaService.AddOrUpdate(infoProspecta);
            }

            var riskLevel = RiskLevelService.GetByCategory(infoProspecta?.resultado);

            if (riskLevel?.Id == null) return;

            item.RiskLevelId = riskLevel.Id;

            foreach (var quota in riskLevel.Quota.OrderBy(x => x.Amount))
                item.Score = $"{item.Score},{quota.Amount}";

            item.Score = item.Score.TrimStart(',');
        }
    }
}

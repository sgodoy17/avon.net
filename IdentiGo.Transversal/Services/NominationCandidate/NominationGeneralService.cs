using Component.Transversal.Utilities;
using IdentiGo.Domain.DTO.App;
using IdentiGo.Domain.Entity.AVON;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Enums;
using IdentiGo.ExternalServices;
using IdentiGo.Services.Avon;
using IdentiGo.Services.CIFIN;
using IdentiGo.Services.General;
using IdentiGo.Services.Log;
using IdentiGo.Services.Master;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace IdentiGo.Transversal.Services.NominationCandidate
{
    public interface INominationGeneralService
    {
        string Login(string[] dataLogin);

        void AppiNomination(RegisterValidationDto item, ref Nomination currentUser);

        void ManualNomination(RegisterValidationDto item, ref Nomination currentUser);

        void ManagerNomination(RegisterValidationDto item, ref Nomination currentUser);
    }

    class NominationGeneralService : INominationGeneralService
    {
        public readonly INominationService UserValidationService;
        public readonly IRiskLevelService RiskLevelService;
        public readonly IValidadorPlusService ValidadorPlusService;
        public readonly ICashPaymentService CashPaymentService;
        public readonly IProspectaService ProspectaService;
        public readonly ICampaingService CampaingService;
        public readonly IZoneService ZoneService;
        public readonly IUnitService UnitService;
        public readonly IDivisionService DivisionService;
        public readonly ILogSMSService LogSMSService;
        public readonly INominationResponseService NominationResponseService;
        public readonly INominationHistoricService NominationHistoricService;
        public readonly IConfigService ConfigService;
        public readonly NombraAvonExternalService NombraAvonService = new NombraAvonExternalService();
        public readonly CifinExternalService CifinService;

        public NominationGeneralService(INominationService userValidationService,
            IRiskLevelService riskLevelService,
            IValidadorPlusService validadorPlusService,
            ICashPaymentService cashPaymentService,
            IProspectaService prospectaService,
            ICampaingService campaingService,
            IUnitService unitService,
            IDivisionService divisionService,
            IZoneService zoneService,
            ILogSMSService logSMSService,
            IConfigService confingService,
            INominationResponseService nominationResponseService,
            INominationHistoricService nominationHistoricService)
        {
            UserValidationService = userValidationService;
            RiskLevelService = riskLevelService;
            ValidadorPlusService = validadorPlusService;
            CashPaymentService = cashPaymentService;
            ProspectaService = prospectaService;
            CampaingService = campaingService;
            UnitService = unitService;
            DivisionService = divisionService;
            ZoneService = zoneService;
            LogSMSService = logSMSService;
            ConfigService = confingService;
            NominationResponseService = nominationResponseService;
            NominationHistoricService = nominationHistoricService;
            var config = ConfigService.GetAll().FirstOrDefault();
            CifinService = new CifinExternalService(config);
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

            var date = DateTime.Today.AddDays(-38);

            if (currentUser != null)
            {
                if (currentUser.DateCreated <= date)
                {
                    AddNominationHistoric(currentUser);
                    RestartNomination(ref currentUser);                    
                }
            }

            if (currentUser == null)
                currentUser = userValidation;
            else if (currentUser.CodeUser == null)
                currentUser.CodeUser = userValidation.CodeUser;
            else if (currentUser.CodeUser == "")
                currentUser.CodeUser = userValidation.CodeUser;
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
                        AddNominationResponse(currentUser, item.Message);
                        DataAuthorization(ref currentUser, item.State);
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

        public void AddNominationResponse(Nomination item, string message)
        {
            NominationResponseService.AddOrUpdate(new NominationResponse { Date = DateTime.Now, NominationId = item.Id, Message = message, Stage = item.StageProcess });
        }

        public void AddNominationHistoric(Nomination item)
        {
            NominationHistoricService.AddOrUpdate(new NominationHistoric {
                NominationId = item.Id,
                Date = DateTime.Now,
                DateCreated = item.DateCreated,
                CodeVerification = item.CodeVerification,
                CampaingId = item.CampaingId,
                DivisionId = item.DivisionId,
                ZoneId = item.ZoneId,
                UnitId = item.UnitId,
                State = item.State,
                StateDocument = item.StateDocument,
                StageProcess = item.StageProcess,
                PhoneNumber = item.PhoneNumber,
                PhoneAnswer = item.PhoneAnswer,
                CodeUser = item.CodeUser
            });
        }

        public void ManualNomination(RegisterValidationDto item, ref Nomination currentUser)
        {
            var userValidation = GenerateItemManual(item);
            ValidationData(ref userValidation);
            currentUser = UserValidationService.GetLastByDocument(userValidation.Document);

            var date = DateTime.Today.AddDays(-38);

            if (currentUser != null)
            {
                if (currentUser.DateCreated <= date)
                {
                    AddNominationHistoric(currentUser);
                    RestartNomination(ref currentUser);
                }
            }

            if (currentUser == null)
                currentUser = userValidation;
            else if (currentUser.CodeUser == null)
                currentUser.CodeUser = userValidation.CodeUser;
            else if (currentUser.CodeUser == "")
                currentUser.CodeUser = userValidation.CodeUser;
            else if (currentUser.CodeUser != userValidation.CodeUser)
                throw new Exception("Esta candidata esta siendo gestionada por otra empresaria. Por favor comunicate con tu gerente de zona");

            if (currentUser.StageProcess == StageProccess.Init)
                currentUser.StageProcess = StageProccess.DataAuthorization;

            UpdateData(ref currentUser, userValidation);

            switch (currentUser.StageProcess)
            {
                case StageProccess.DataAuthorization:
                    UserValidationService.AddOrUpdate(currentUser);
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

        public void ManagerNomination(RegisterValidationDto item, ref Nomination currentUser)
        {
            var userValidation = GenerateItemManager(item);
            ValidationData(ref userValidation);
            currentUser = UserValidationService.GetLastByDocument(userValidation.Document);

            var date = DateTime.Today.AddDays(-38);

            if (currentUser != null)
            {
                if (currentUser.DateCreated <= date)
                {
                    AddNominationHistoric(currentUser);
                    RestartNomination(ref currentUser);
                }
            }

            if (currentUser == null)
                currentUser = userValidation;
            else if (currentUser.CodeUser == null)
                currentUser.CodeUser = userValidation.CodeUser;
            else if (currentUser.CodeUser == "")
                currentUser.CodeUser = userValidation.CodeUser;
            else if (currentUser.CodeUser != userValidation.CodeUser)
                throw new Exception("Esta candidata esta siendo gestionada por otra empresaria. Por favor comunicate con tu gerente de zona");

            if (currentUser.StageProcess == StageProccess.Init)
                currentUser.StageProcess = StageProccess.DataAuthorization;

            UpdateData(ref currentUser, userValidation);

            switch (currentUser.StageProcess)
            {
                case StageProccess.DataAuthorization:
                    UserValidationService.AddOrUpdate(currentUser);
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
                Sex = message.Length > 7 ? message[7] == "1" ? Gender.Male : Gender.Female : Gender.Female,
                PhoneAnswer = item.PhoneAnswer,
                TypeProcess = message[0].ToLower() == Params.codeVI ? TypeProcess.AppiIOS : TypeProcess.Appi,
                StageProcess = StageProccess.Init
            };
        }

        private Nomination GenerateItemManual(RegisterValidationDto item)
        {
            var message = item.Message.Split(';');

            if (message.Length < 7)
                throw new Exception("Estructura de datos invalida, verifique y vuelva a intentar");

            if (message[6] == "" || message[6] == null)
                throw new Exception("Por favor definir el genero para realizar el nombramiento");

            return new Nomination
            {
                CodeUser = message.Length > 1 ? message[1].PadLeft(11, '0') : "",
                Document = message.Length > 2 ? message[2] : "",
                PhoneNumber = message.Length > 3 ? Regex.Replace(message[3], @"[^\d]", "") : "",
                TypePhone = message.Length > 4 ? message[4] == "tel" ? TypePhone.Fixed : TypePhone.Cell : TypePhone.Cell,
                CampaingId = message.Length > 5 ? CampaingService.GetByNumber(message[5])?.Id : null,
                Sex = message.Length > 6 ? message[6] == "M" ? Gender.Male : Gender.Female : Gender.Female,
                PhoneAnswer = item.PhoneAnswer,
                TypeProcess = TypeProcess.Manual,
                StageProcess = StageProccess.DataAuthorization
            };
        }

        private Nomination GenerateItemManager(RegisterValidationDto item)
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
                Sex = message.Length > 6 ? message[6] == "M" ? Gender.Male : Gender.Female : Gender.Female,
                PhoneAnswer = item.PhoneAnswer,
                TypeProcess = TypeProcess.Manager,
                StageProcess = StageProccess.DataAuthorization
            };
        }

        private void ValidationData(ref Nomination user)
        {
            if (string.IsNullOrWhiteSpace(user.PhoneAnswer))
                throw new Exception("El número de teléfono de quien realiza el nombramiento es requerido");

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

            var number = 0;

            if (user.PhoneNumber == "" || user.PhoneNumber == null)
                number = 0;
            else
                number = UserValidationService.GetByPhoneNumber(user.PhoneNumber);

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
                    number = number + 1;
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

        private void RestartNomination(ref Nomination currentUser)
        {
            var currentNomination = UserValidationService.GetById(currentUser.Id);
            currentNomination.StageProcess = StageProccess.Init;
            currentNomination.State = State.Default;
            currentNomination.StateDocument = StateDocument.Default;
            currentNomination.DateCreated = DateTime.Now;
            currentNomination.DateLastValidation = DateTime.Now;
            currentNomination.DateNomination = DateTime.Now;
            currentNomination.DateUpdate = DateTime.Now;
            currentNomination.CodeUser = "";
            currentNomination.Score = null;
            currentUser = currentNomination;
        }

        private void DataAuthorization(ref Nomination item, State state)
        {
            if (item.State == State.Default || item.State == State.Pending || item.State == State.Invalid)
            {
                item.State = State.Success;
                item.DateUpdate = DateTime.Now;
            }
            else
                throw new Exception("Estado del proceso invalido");

            if (item.State == State.Success)
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

                if (state == "7")
                    item.StateDocument = StateDocument.ListOFAC;

                if (item.CodeVerification != null)
                    AddNominationHistoric(item);

                item.State = State.Invalid;
                item.StageProcess = StageProccess.ValidaAvon;
                item.CodeVerification = null;
                item.DateNomination = DateTime.Now;
            }

            item.DateUpdate = DateTime.Now;
        }

        public void ValidateDocument(ref Nomination item)
        {
            var validadorPlus = CifinService.ConsultInformacionService(item.Document);

            if (validadorPlus == null)
            {
                validadorPlus.NombreTitular = null;

                CofigureName(ref item, validadorPlus.NombreTitular);
            }
            else
            {
                CofigureName(ref item, validadorPlus.NombreTitular);

                validadorPlus.NominationId = item.Id;
                ValidadorPlusService.AddOrUpdate(validadorPlus);
            }            

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
                item.CodeVerification = UserValidationService.GenerateCodeVerificacion(item.Document);
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
                item.CodeVerification = UserValidationService.GenerateCodeVerificacion(item.Document);
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

            if (infoProspecta?.resultado?.Contains("4") == true || infoProspecta?.resultado?.Contains("5") == true || infoProspecta?.resultado?.Contains("8") == true)
            {
                //infoProspecta.resultado = "Elegible B";
                infoProspecta.resultado = "Sin Información";
            }
            else if (infoProspecta?.resultado?.Contains("6") == true || infoProspecta?.resultado?.Contains("7") == true)
            {
                item.StageProcess = StageProccess.ValidaAvon;
                item.State = State.Invalid;
                item.StateDocument = StateDocument.ListOFAC;
                item.DateNomination = DateTime.Now;

                return;
            }
            else if (infoProspecta?.resultado == null)
            {
                infoProspecta.resultado = "Sin Información";
            }            

            var riskLevel = RiskLevelService.GetByCategory(infoProspecta?.resultado);

            if (riskLevel?.Id == null) return;

            if (item.Zone.CashPayment)
            {
                var document = int.Parse(item.Document);
                var risk = riskLevel.Level.ToString();
                var genre = item.Sex == Gender.Male ? "M" : item.Sex == Gender.Female ? "F" : "F";
                var division = int.Parse(DivisionService.Get(item.DivisionId)?.Number);
                var zone = int.Parse(ZoneService.Get(item.ZoneId)?.Number);
                var unit = item.UnitId != null ? int.Parse(UnitService.Get(item.UnitId)?.Number) : -1;
                var today = DateTime.Today.Year;
                var age = item.InfoCifin.FirstOrDefault().RangoEdad != null ? item.InfoCifin.FirstOrDefault().RangoEdad.Contains("Mas") ? item.InfoCifin.FirstOrDefault().RangoEdad.Split(' ')[1] : item.InfoCifin.FirstOrDefault().RangoEdad.Split('-')[0] : "30";
                var year = today - int.Parse(age);
                var birth_date = int.Parse(year.ToString() + "0101");

                var result = NombraAvonService.PagoContado(document, genre, risk, birth_date, division, zone, unit);
                CashPaymentEnum cashPayment = (CashPaymentEnum)result;

                if (cashPayment == CashPaymentEnum.Apply)
                    item.StateDocument = StateDocument.PagoContado;

                var userPayment = GenerateCashPayment(item.Id, document, genre, risk, birth_date, division, zone, unit, result);
                CashPaymentService.AddOrUpdate(userPayment);
            }

            item.RiskLevelId = riskLevel.Id;

            if (item.StateDocument == StateDocument.PagoContado)
            {
                item.Score = "1";
                item.AvonRiskLevel = 5;

                return;
            }

            item.AvonRiskLevel = riskLevel.Level;

            foreach (var quota in riskLevel.Quota.OrderBy(x => x.Amount))
                item.Score = $"{item.Score},{quota.Amount}";

            item.Score = item.Score.TrimStart(',');
        }

        private CashPayment GenerateCashPayment(Guid id, int document, string genre, string risk, int birth_date, int division, int zone, int unit, int result)
        {
            return new CashPayment
            {
                NominationId = id,
                Document = document,
                Genre = genre,
                Risk = risk,
                BirthDate = birth_date,
                Division = division,
                Zone = zone,
                Unit = unit,
                DateCreated = DateTime.Now,
                Result = result,
                Description = Enum.GetName(typeof(CashPaymentEnum), result),
            };
        }
    }
}

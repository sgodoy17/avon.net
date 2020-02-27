using Component.Transversal.Utilities;
using IdentiGo.Domain.DTO.App;
using IdentiGo.Domain.Entity.General;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Enums;
using IdentiGo.ExternalServices;
using IdentiGo.Services.General;
using IdentiGo.Services.Log;
using IdentiGo.Services.Master;
using IdentiGo.Transversal.Services.NominationCandidate;
using IdentiGo.Transversal.Utilities;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace IdentiGo.Transversal.Services
{
    /// <summary>
    /// Interface INominationManagerService
    /// Provides interface for NominationProcess service
    /// </summary>
    public interface INominationManagerService
    {
        void NominationProcess(RegisterValidationDto item);
    }

    /// <summary>
    /// Class NominationManagerService
    /// Provides the methods and functions to nominate the user
    /// </summary>
    public class NominationManagerService : INominationManagerService
    {
        public readonly INominationProcessService NominationProcessService;
        public readonly INominationGeneralService NominationGeneralService;
        public readonly ICandidateService CandidateService;
        public readonly IIVRService IvrService;
        public readonly ISMSService SmsService;
        public readonly IIntrasoftService IntrasoftService;
        public readonly ILogSMSService LogSMSService;
        public readonly ILogIVRService LogIVRService;
        public readonly INominationService NominationService;
        public readonly IConfigService ConfigService;
        public readonly ICampaingService CampaingService;
        private readonly Config config;

        /// <summary>
        /// Constructor NominationManagerService
        /// Set services references
        /// </summary>
        /// <param name="nominationProcessService">Call the interface of nomination process service</param>
        /// <param name="candidateService">Call the interface of candidate service</param>
        /// <param name="ivrService">Call the interface of ivr service</param>
        /// <param name="smsService">Call the interface of sms service</param>
        /// <param name="intrasoftService">Call the interface of sms service</param>
        /// <param name="logSMSService">Call the interface of log sms service</param>
        /// <param name="logIVRService">Call the interface of log ivr service</param>
        /// <param name="nominationService">Call the interface of nomination</param>
        /// <param name="configService">Call the interface of config service</param>
        /// <param name="campaingService">Call the interface of campaing service</param>
        public NominationManagerService(INominationProcessService nominationProcessService,
            INominationGeneralService nominationGeneralService,
            ICandidateService candidateService,
            IIVRService ivrService,
            ISMSService smsService,
            IIntrasoftService intrasoftService,
            ILogSMSService logSMSService,
            ILogIVRService logIVRService,
            INominationService nominationService,
            IConfigService configService,
            ICampaingService campaingService)
        {
            NominationProcessService = nominationProcessService;
            NominationGeneralService = nominationGeneralService;
            CandidateService = candidateService;
            IvrService = ivrService;
            SmsService = smsService;
            IntrasoftService = intrasoftService;
            LogSMSService = logSMSService;
            LogIVRService = logIVRService;
            NominationService = nominationService;
            ConfigService = configService;
            CampaingService = campaingService;
            config = ConfigService.GetAll().FirstOrDefault();            
        }

        /// <summary>
        /// Method NominationProcess
        /// Provides the top of the nomination process logic
        /// </summary>
        /// <param name="item">User's information</param>
        public void NominationProcess(RegisterValidationDto item)
        {
            Nomination userValidation = null;           

            try
            {
                if (item.CodeValidation == Params.codeVP || item.CodeValidation == Params.codeVI)
                {
                    NominationGeneralService.AppiNomination(item, ref userValidation);
                    NominationGeneralService.AppiNomination(item, ref userValidation);
                }
                else if (item.CodeValidation == Params.codeManual)
                    NominationGeneralService.ManualNomination(item, ref userValidation);
                else if (item.CodeValidation == Params.codeManager)
                    NominationGeneralService.ManagerNomination(item, ref userValidation);
                else if (Array.Exists(Params.codeResponse, x => x.StartsWith(item.CodeValidation)))
                    CandidateService.NominationCandidate(item, ref userValidation);
                else if (item.CodeValidation == Params.codeLogin || item.CodeValidation == Params.codeLoginI)
                {
                    string message = NominationProcessService.Login(item.Message.Split(';'));

                    string document = GenerateDocument(item.Message.Split(';'));

                    if (item.CodeValidation == Params.codeLoginI)
                        message = ($"nombraYa://home?datos={message}").TrimEnd(' ').Replace(' ', '_');

                    message = message + ';' + GenerateRandom();
                    IntrasoftService.Send(item.PhoneAnswer, message);
                    LogSMSService.Add("", item.PhoneAnswer, message, document, false);

                    return;
                }

                LogSMSService.Add(item.CodeValidation, item.PhoneAnswer, item.Message, userValidation.Document);
                SendMessage(userValidation);
            }
            catch (Exception ex)
            {
                if (userValidation != null)
                    userValidation = NominationService.GetLastByDocument(userValidation.Document);
                else
                    userValidation = GenerateItemManual(item);

                if (item.Message.Contains("The request channel timed out while waiting for a reply after"))
                {
                    item.Message = ex.Message;
                    LogSMSService.Add("error", item.PhoneAnswer, ex.Message, userValidation.Document, false);
                    EmailUtils.SendEmailError(config, item.Message);
                }
                else if (item.Message.Contains("Object reference not set to an instance of an object."))
                {
                    item.Message = ex.Message;
                    LogSMSService.Add("error", item.PhoneAnswer, "001-" + ex.Message, userValidation.Document, false);
                    EmailUtils.SendEmailError(config, item.Message);
                }
                else if (item.Message.Contains("ValidaAvonService Cod:03"))
                {
                    item.Message = ex.Message;
                    LogSMSService.Add("error", item.PhoneAnswer, "Avon:" + ex.Message, userValidation.Document, false);
                    EmailUtils.SendEmailError(config, item.Message);
                }
                else if (item.Message.Contains("An error was reported while committing a database transaction but it could not be determined whether the transaction succeeded or failed on the database server."))
                {
                    item.Message = ex.Message;
                    LogSMSService.Add("error", item.PhoneAnswer, ex.Message, userValidation.Document, false);
                    EmailUtils.SendEmailError(config, item.Message);
                }
                else if (item.Message.Contains("java.lang.reflect."))
                {
                    item.Message = ex.Message;
                    LogSMSService.Add("error", item.PhoneAnswer, ex.Message, userValidation.Document, false);
                    EmailUtils.SendEmailError(config, item.Message);
                }
                else
                {
                    item.Message = ex.Message;
                    LogSMSService.Add("error", item.PhoneAnswer, "002-" + ex.Message, userValidation.Document, false);
                    SendMessageError(item, userValidation.Document);
                }
            }
        }

        /// <summary>
        /// Method GenerateDocument
        /// Get the user's document from the message
        /// </summary>
        /// <param name="dataLogin">Message with user's claims</param>
        /// <returns>Document string</returns>
        private string GenerateDocument(string[] dataLogin)
        {
            var document = dataLogin.Length > 1 ? dataLogin[1] : "";

            return document;
        }

        /// <summary>
        /// Method GenerateItemManual
        /// Get all the nomination data from the message
        /// </summary>
        /// <param name="item">Message with user's claims</param>
        /// <returns>Nomnation set of data</returns>
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

        /// <summary>
        /// Method SendMessage
        /// Get user's claim data and send message
        /// </summary>
        /// <param name="currentUser">Nomnation set of data</param>
        private void SendMessage(Nomination currentUser)
        {
            string message = "";
            string msg = "";
            string phoneNumber = null;           

            if (currentUser.Consult)
                message = MessageAppi(currentUser) + ';' + GenerateRandom();
            else if (currentUser.StageProcess == StageProccess.DataAuthorization && currentUser.State == State.Pending)
            {
                if (currentUser.TypePhone == TypePhone.Fixed)
                {
                    IvrService.Send(currentUser.PhoneNumber, Params.campidAuthorized);
                    LogIVRService.Add(Params.campidAuthorized.ToString(), currentUser.PhoneNumber, "", false);
                }
                else
                {
                    message = "Autoriza a Avon realizar el tratamiento y transmision de datos para fines comerciales segun la ley aplicable. Para autorizar envia CX1 de lo contrario envia CX2" + ';' + GenerateRandom();
                    phoneNumber = currentUser.PhoneNumber;
                }
            }
            else if (currentUser.StageProcess == StageProccess.CandidateConfirmation && currentUser.State == State.Pending)
            {
                if (currentUser.TypePhone == TypePhone.Fixed)
                {
                    IvrService.Send(currentUser.PhoneNumber, Params.campidConfirmation);
                    LogIVRService.Add(Params.campidConfirmation.ToString(), currentUser.PhoneNumber, "", false);
                }
                else
                {
                    message = "Autoriza a Avon realizar el tratamiento y transmision de datos para fines comerciales segun la ley aplicable. Para autorizar envia CX1 de lo contrario envia CX2" + ';' + GenerateRandom();
                    phoneNumber = currentUser.PhoneNumber;
                }
            }
            else if (currentUser.TypeProcess == TypeProcess.Manual || currentUser.TypeProcess == TypeProcess.Manager)
            {
                switch (currentUser.StageProcess)
                {
                    case StageProccess.Init:
                        break;
                    case StageProccess.DataAuthorization:
                        if (currentUser.State == State.Invalid)
                            message = $"El usuario {currentUser.Document} no autorizo el tratamiento de datos. No puede continuar el proceso";
                        else if (currentUser.State == State.Success)
                            message = $"Candidate a representante autorizó el tratamiento de datos personales";
                        else if (currentUser.State == State.Invalid)
                            message = $"Candidate a representante no autorizó tratamiento de datos personales, no puede continuar el proceso";
                        break;
                    case StageProccess.ValidaAvon:
                        if (currentUser.State == State.Success)
                            message = "Validacion Avon exitosa. Para continuar reenvie el mensaje anterior";
                        else if (currentUser.StateDocument == StateDocument.Activa)
                            message = $"El documento {currentUser.Document} se encuentra Activa en Avon, no puede continuar el nombramiento";
                        else if (currentUser.StateDocument == StateDocument.Collection)
                            message = $"El documento {currentUser.Document} se encuentra en Collection en Avon, no puede continuar el nombramiento";
                        else if (currentUser.State == State.Invalid)
                            message = $"El documento {currentUser.Document} no puede continuar el nombramiento por incumplimiento de politicas Avon";
                        break;
                    case StageProccess.CandidateConfirmation:
                        if (currentUser.State == State.Invalid)
                            message = "Candidata a Representante no desea continuar el nombramiento";
                        else if (currentUser.State == State.NotContacted)
                            message = "Candidata a Representante no pudo ser contactada, no puede continuar con el nombramiento";
                        break;
                    case StageProccess.Finished:
                        var scoreUser = currentUser.Score.Split(',');

                        if (currentUser.Zone.SendCode == false)
                        {
                            if (currentUser.StateDocument == StateDocument.PagoAnticipado)
                                message = string.Format("Nombramiento exitoso de{0} doc {1}, PAGO ANTICIPADO",
                                    string.IsNullOrEmpty(currentUser.Name) ? "" : $"{currentUser.Name} {currentUser.LastName}",
                                    currentUser.Document);
                            else if (currentUser.StateDocument == StateDocument.PagoContado)
                                message = string.Format("Nombramiento exitoso de{0} doc {1}, PAGO CONTADO",
                                    string.IsNullOrEmpty(currentUser.Name) ? "" : $"{currentUser.Name} {currentUser.LastName}",
                                    currentUser.Document);
                            else
                                message = string.Format("Nombramiento exitoso de{0} doc {1}{2}{3}{4}.",
                                    string.IsNullOrEmpty(currentUser.Name) ? "" : $" {currentUser.Name} {currentUser.LastName}",
                                    currentUser.Document,
                                    scoreUser.Length > 0 && !string.IsNullOrEmpty(scoreUser[0]) ? $", cupo primer pedido ${scoreUser[0]}" : "",
                                    scoreUser.Length > 1 ? $" segundo ${scoreUser[1]}" : "",
                                    scoreUser.Length > 2 ? $" tercer ${scoreUser[2]}" : "");
                        }
                        else
                        {
                            if (currentUser.StateDocument == StateDocument.PagoAnticipado)
                                message = string.Format("Nombramiento exitoso de{0} doc {1} codigo verificacion {2}, PAGO ANTICIPADO",
                                    string.IsNullOrEmpty(currentUser.Name) ? "" : $"{currentUser.Name} {currentUser.LastName}",
                                    currentUser.Document,
                                    currentUser.CodeVerification);
                            else if (currentUser.StateDocument == StateDocument.PagoContado)
                                message = string.Format("Nombramiento exitoso de{0} doc {1} codigo verificacion {2}, PAGO CONTADO",
                                    string.IsNullOrEmpty(currentUser.Name) ? "" : $"{currentUser.Name} {currentUser.LastName}",
                                    currentUser.Document,
                                    currentUser.CodeVerification);
                            else
                                message = string.Format("Nombramiento exitoso de{0} doc {1} codigo verificacion {2}{3}{4}{5}.",
                                    string.IsNullOrEmpty(currentUser.Name) ? "" : $" {currentUser.Name} {currentUser.LastName}",
                                    currentUser.Document,
                                    currentUser.CodeVerification,
                                    scoreUser.Length > 0 && !string.IsNullOrEmpty(scoreUser[0]) ? $", cupo primer pedido ${scoreUser[0]}" : "",
                                    scoreUser.Length > 1 ? $" segundo ${scoreUser[1]}" : "",
                                    scoreUser.Length > 2 ? $" tercer ${scoreUser[2]}" : "");
                        }
                            
                        break;
                    default:
                        throw new Exception("El campo StageProccess es invalido");
                }
            }
            else
                message = MessageAppi(currentUser) + ';' + GenerateRandom();

            if (string.IsNullOrEmpty(message)) return;            

            if (currentUser.StageProcess == StageProccess.Finished && currentUser.State == State.Success)
            {
                currentUser.State = State.Pass;
                NominationService.AddOrUpdate(currentUser);

                var scoreUser = currentUser.Score.Split(',');
                var name_string = currentUser.Name ?? "CANDIDATA AVON";
                var name = name_string.Split(' ');

                if (currentUser.StateDocument == StateDocument.PagoAnticipado)
                    msg = string.Format("{0}, te damos la bienvenida a AVON en pago contado durante 6 pedidos, disfruta credito en tu pedido 7 si cumples condiciones, el CV es {1}",
                        string.IsNullOrEmpty(name[0]) ? "" : $" {name[0]}",
                        currentUser.CodeVerification);
                else if (currentUser.StateDocument == StateDocument.PagoContado)
                    msg = string.Format("{0}, te damos la bienvenida a AVON en pago contado durante 2 pedidos, disfruta credito en tu pedido 3 si cumples condiciones, el CV es {1}",
                        string.IsNullOrEmpty(name[0]) ? "" : $" {name[0]}",
                        currentUser.CodeVerification);
                else
                    msg = string.Format("{0}, te damos la bienvenida a AVON, el codigo de verificacion es {1}{2}{3}{4}.",
                        string.IsNullOrEmpty(name[0]) ? "" : $" {name[0]}",
                        currentUser.CodeVerification,
                        scoreUser.Length > 0 && !string.IsNullOrEmpty(scoreUser[0]) ? $", cupo primera campaña ${scoreUser[0]}" : "",
                        scoreUser.Length > 1 ? $" segunda ${scoreUser[1]}" : "",
                        scoreUser.Length > 2 ? $" tercera ${scoreUser[2]}" : "");


             
                IntrasoftService.Send(currentUser.PhoneNumber, msg);
                LogSMSService.Add("", phoneNumber ?? currentUser.PhoneNumber, msg, currentUser.Document, false);
            }

            IntrasoftService.Send(phoneNumber ?? currentUser.PhoneAnswer, message);
            LogSMSService.Add("", phoneNumber ?? currentUser.PhoneAnswer, message, currentUser.Document, false);
        }

        /// <summary>
        /// Method MessageAppi
        /// Set message from the api
        /// </summary>
        /// <param name="currentUser">Nomnation set of data</param>
        /// <returns>String parse message</returns>
        private string MessageAppi(Nomination currentUser)
        {
            var state = 0;
            string score;

            switch (currentUser.StageProcess)
            {
                case StageProccess.ValidaAvon:
                    if (currentUser.State == State.Invalid)
                        state = (int)currentUser.StateDocument;
                    else
                        state = (int)currentUser.State;
                    break;
                case StageProccess.Finished:
                    state = (int)currentUser.State;

                    if (state == 14)
                        state = 1;

                    break;
                default:
                    state = (int)currentUser.State;
                    break;
            }

            if (currentUser.Zone.SendCode == false)
                score = currentUser.CodeVerification == null && string.IsNullOrEmpty(currentUser.Score) ? "" : $"{null},{currentUser.Score}";
            else
                score = currentUser.CodeVerification == null && string.IsNullOrEmpty(currentUser.Score) ? "" : $"{currentUser.CodeVerification},{currentUser.Score}";

            if (currentUser.TypeProcess == TypeProcess.AppiIOS)
                return ($"nombraYa://h?p={Params.codeVI};1;{currentUser.Document};{currentUser.Name}{(string.IsNullOrEmpty(currentUser.LastName) ? "" : $" {currentUser.LastName}")};{(int)currentUser.StageProcess};{score};{state}").Replace(" ", "_");
            else
                return $"{Params.codeVP};1;{currentUser.Document};{currentUser.Name}{(string.IsNullOrEmpty(currentUser.LastName) ? "" : $" {currentUser.LastName}")};{(int)currentUser.StageProcess};{score};{state}";
        }

        /// <summary>
        /// Method GenerateRandom
        /// Get a random text
        /// </summary>

        private String GenerateRandom()
        {
            Random obj = new Random();
            string sCadena = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int longitud = sCadena.Length;
            char cletra;
            int nlongitud = 3;
            string sNuevacadena = string.Empty;

            for (int i = 0; i < nlongitud; i++)
            {
                cletra = sCadena[obj.Next(sCadena.Length)];
                sNuevacadena += cletra.ToString();
            }

            return sNuevacadena;
        }         

        /// <summary>
        /// Method SendMessageError
        /// Set the message in case of error
        /// </summary>
        /// <param name="item">Message with user's claims</param>
        private void SendMessageError(RegisterValidationDto item, string document)
        {
            string message = "";           

            if (item.CodeValidation == Params.codeVP)
                message = $"{Params.codeVP};0;{document};{item.Message}" + ';' + GenerateRandom();
            if (item.CodeValidation == Params.codeVI)
                message = ($"{"nombraYa://h?p="}{Params.codeVI};0;{document};{item.Message}").Replace(" ", "_") + ';' + GenerateRandom();
            else if (item.CodeValidation == Params.codeManual)
                message = $"{item.Message}";
            else if (Array.Exists(Params.codeResponse, x => x.StartsWith(item.CodeValidation)))
            {
                var user = NominationService.GetLastByPhoneNumber(item.PhoneAnswer);
                item.PhoneAnswer = user?.PhoneAnswer;
                message = user?.TypeProcess == TypeProcess.Manual ? item.Message : $"{Params.codeVPError};0;{item.Message}" + ';' + GenerateRandom();
            }
            else if (item.CodeValidation == Params.codeLogin)
                message = $"{item.CodeValidation};0;{ item.Message}" + ';' + GenerateRandom();
            else if (item.CodeValidation == Params.codeLoginI)
                message = $"nombraYa://home?datos={item.CodeValidation};0;{item.Message}" + ';' + GenerateRandom();

            if (string.IsNullOrEmpty(item.PhoneAnswer) || string.IsNullOrEmpty(message))
                return;

            IntrasoftService.Send(item.PhoneAnswer, message);
            EmailUtils.SendEmailError(config, message);
        }
    }
}
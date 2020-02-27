using IdentiGo.Transversal.Exceptions;
using IdentiGo.Transversal.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Transactions;

namespace IdentiGo.Transversal.Error
{
    public class AppErrorManager
    {
        public static AppErrorDetail GetErrorDetail(Exception exception)
        {
            if (exception == null)
                throw new AppBusinessException("No fue posible extraer la excepción interna.");

            //Primero se verifica si es una excepción del EF
            if ((exception.GetType() == typeof(EntityException) || exception.GetType() == typeof(DataException)) && exception.InnerException != null)
                return GetErrorDetail(exception.InnerException);

            string errorCode = "500";
            string errorTitle = "Falla interna del servidor";
            string detailErrorType = "Falla interna";
            string simpleMessage = "Se ha presentado una falla interna en la aplicación";
            string detailClientMessage = simpleMessage;
            string shortExceptionMessage = exception.Message;
            string detailCompleteExceptionMessage = exception.ToString();
            bool retryNotify = false;
            bool sendMail = true;

            #region Switch Error Type

            var @switch = new Dictionary<Type, Action> 
            {
                { typeof(AppBusinessException), 
                    () =>
                    {
                        var businessEx = ((AppBusinessException)exception);
                        errorCode = "200";
                        errorTitle = "Acción no permitida";
                        detailErrorType = "Falla de negocio";
                        simpleMessage = businessEx.BusinessMessage;
                        detailClientMessage = simpleMessage;
                        shortExceptionMessage = businessEx.ToShortString();
                        detailCompleteExceptionMessage = businessEx.ToString();
                        sendMail = false;
                    }
                },
                { typeof(InvalidOperationException), 
                    () =>
                    {
                        var invalidEx = ((InvalidOperationException)exception);
                        errorCode = "201";
                        errorTitle = "Operación no válida para el sistema";
                        detailErrorType = "Operación no valida";
                        simpleMessage = "No es posible realizar la operación en el sistema";
                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, invalidEx.Message);
                        detailCompleteExceptionMessage += Environment.NewLine + string.Format("HResult: {0} {1} Help Link: {2} {1} Data: {3}", invalidEx.HResult, Environment.NewLine, invalidEx.HelpLink, invalidEx.Data.ToDebugString());
                    }
                },
                { typeof(UnauthorizedAccessException), 
                    () =>
                    {
                        var unautorizedEx = ((UnauthorizedAccessException)exception);
                        errorCode = "401"; 
                        errorTitle = "Acceso no autorizado";
                        detailErrorType = "Falla de autorización";
                        simpleMessage = "No autorizado";
                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, unautorizedEx.Message);
                        detailCompleteExceptionMessage += Environment.NewLine + string.Format("HResult: {0} {1} Help Link: {2} {1} Data: {3}", unautorizedEx.HResult, Environment.NewLine, unautorizedEx.HelpLink, unautorizedEx.Data.ToDebugString());
                        
                    }
                },
                { typeof(FaultException),
                    () =>
                    {
                        var faultEx = ((FaultException)exception);
                        errorCode = "300";
                        errorTitle = "Falla en el servicio de integración";
                        detailErrorType = "Falla general de integracion";
                        simpleMessage = "Falla al llamar al servicio de integración";
                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, faultEx.Reason.ToString());
                        shortExceptionMessage = string.Format("Reason: {0} {1} Message: {2}", faultEx.Reason, Environment.NewLine, faultEx.Message);
                        detailCompleteExceptionMessage = shortExceptionMessage + Environment.NewLine + string.Format("HResult: {0} {1} Help Link: {2} {1} Action: {3} {1} Code: {4} {1} Reason: {5} {1} Data: {6}", faultEx.HResult, Environment.NewLine, faultEx.HelpLink, faultEx.Action, faultEx.Code, faultEx.Reason, faultEx.Data.ToDebugString());
                        retryNotify = true;
                    }
                },
                { typeof(FaultException<>),
                    () =>
                    {
                        var faultEx = ((FaultException)exception);
                        errorCode = "301";
                        errorTitle = "Falla en el servicio de integración";
                        detailErrorType = "Falla no tipificada de integracion";
                        simpleMessage = "Falla al llamar al servicio de integración";
                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, faultEx.Reason.ToString());
                        shortExceptionMessage = string.Format("Reason: {0} {1} Message: {2}", faultEx.Reason, Environment.NewLine, faultEx.Message);
                        detailCompleteExceptionMessage = shortExceptionMessage + Environment.NewLine + string.Format("HResult: {0} {1} Help Link: {2} {1} Action: {3} {1} Code: {4} {1} Reason: {5} {1} Data: {6}", faultEx.HResult, Environment.NewLine, faultEx.HelpLink, faultEx.Action, faultEx.Code, faultEx.Reason, faultEx.Data.ToDebugString());
                        retryNotify = true;
                    }
                },
                { typeof(DbEntityValidationException),
                    () =>
                    {
                        errorCode = "100";
                        errorTitle = "Existen problemas en la información a ingresar a la base de datos";
                        detailErrorType = "Falla de validacion del proveedor de datos";
                        var entityValidationEx = ((DbEntityValidationException)exception);
                        string validationErrors = 
                            string.Join(" <> ", 
                                entityValidationEx.EntityValidationErrors.SelectMany(ev => 
                                        ev.ValidationErrors.Select(ve => 
                                            string.Format("{{{0}}}=>{{{1}}}", ve.PropertyName, ve.ErrorMessage)).ToArray()));
                        simpleMessage = "Problemas de validación en los datos a ingresar en el sistema, por favor corrija la información e intentelo de nuevo";
                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, validationErrors);
                        shortExceptionMessage = "Message: " + entityValidationEx.Message + Environment.NewLine + "Data Errors: " + validationErrors;
                        detailCompleteExceptionMessage = string.Format("{0} {2} HResult: {1} {2} Help Link: {3} {2} Data: {4}", shortExceptionMessage, entityValidationEx.HResult, Environment.NewLine, entityValidationEx.HelpLink, entityValidationEx.Data.ToDebugString());
                    }
                },
                { typeof(DbUpdateException),
                    () =>
                    {
                        errorCode = "101";
                        errorTitle = "Falla al intentar ingresar la información";
                        detailErrorType = "Falla de actualización de la base de datos";
                        var updateEx = ((DbUpdateException)exception);
                        string sqlMessage = string.Empty;
                        string exMessage = updateEx.ToString();
                        simpleMessage = "Problemas de validación en los datos a ingresar en el sistema";

                        if (updateEx.InnerException != null)
                        {
                            var ex = updateEx.InnerException.InnerException as SqlException;

                            if (ex != null)
                            {
                                SqlException sqlEx = ex;
                                sqlMessage = sqlEx.ToString();

                                switch (sqlEx.Number)
                                {
                                    case -1:
                                    case 2:
                                    case 53:
                                        simpleMessage =
                                            "No fue posible conectarse al servidor de base de datos. Por favor contacte al administrador del sistema";
                                        retryNotify = true;
                                        break;
                                    case -2:
                                        simpleMessage =
                                            "Se agoto el tiempo de espera en el servidor de base de datos. Por favor intente nuevamente la operación";
                                        retryNotify = true;
                                        break;
                                    case 547:
                                        simpleMessage = "Error de restricción de claves foraneas en la base de datos";
                                        break;
                                    case 2601:
                                    case 2627:
                                        simpleMessage =
                                            "Se esta intentando ingresar información duplicada en la base de datos";
                                        break;
                                }
                            }
                        }

                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, shortExceptionMessage);
                        shortExceptionMessage = string.Format("Exception Message: {0} => SQL Message: {1}", exMessage, sqlMessage);
                        detailCompleteExceptionMessage = string.Format("{0} {2} HResult: {1} {2} Help Link: {3} {2} Data: {4}", shortExceptionMessage, updateEx.HResult, Environment.NewLine, updateEx.HelpLink, updateEx.Data.ToDebugString());
                    }
                },
                { typeof(DbUpdateConcurrencyException),
                    () =>
                    {
                        errorCode = "102";
                        errorTitle = "Falla de inconsistencia en la información a ingresar";
                        detailErrorType = "Falla de concurrencia del proveedor de datos";
                        simpleMessage = "Problemas de consistencia con los datos a ingresar contra los que se encuentran en la base de datos";
                        var concurrencyEx = ((DbUpdateConcurrencyException)exception);
                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, concurrencyEx.ToString());
                        string entries =
                            string.Join(" <> ",
                                concurrencyEx.Entries.Select(en =>
                                    string.Format("{{{0}}}=>{{{1}}}", en.Entity.GetType(), string.Join(",", en.CurrentValues.PropertyNames))));
                        shortExceptionMessage = string.Format("Exception Message: {0} => Entries: {1}", concurrencyEx.Message, entries);
                        detailCompleteExceptionMessage = string.Format("{0} {2} HResult: {1} {2} Help Link: {3} {2} Data: {4}", shortExceptionMessage, concurrencyEx.HResult, Environment.NewLine, concurrencyEx.HelpLink, concurrencyEx.Data.ToDebugString());
                    }
                },
                { typeof(TimeoutException),
                    () =>
                    {
                        errorCode = "600";
                        errorTitle = "Tiempo de espera excedido";
                        detailErrorType = "Tiempo de espera agotado";
                        simpleMessage = "Tiempo de espera agotado mientras se esperaba una respuesta del servidor. Se excedio el tiempo de espera en el servidor. Por favor intentelo nuevamente";
                        detailClientMessage = simpleMessage;
                        var timeOutEx = ((TimeoutException)exception);
                        detailCompleteExceptionMessage = string.Format("{0} {2} HResult: {1} {2} Help Link: {3} {2} Data: {4}", shortExceptionMessage, timeOutEx.HResult, Environment.NewLine, timeOutEx.HelpLink, timeOutEx.Data.ToDebugString());
                    }
                },
                { typeof(NullReferenceException), 
                    () =>
                    {
                        detailErrorType = "Objeto nulo instanciado";
                        var nullRef = (NullReferenceException) exception;
                        simpleMessage = "Error de objeto nulo no instanciado";
                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, exception.ToString());                        
                        shortExceptionMessage = "Source: "+  nullRef.Source + Environment.NewLine + "Message: " + nullRef.Message;
                        detailCompleteExceptionMessage = string.Format("{0} {2} HResult: {1} {2} Help Link: {3} {2} Data: {4}", shortExceptionMessage, nullRef.HResult, Environment.NewLine, nullRef.HelpLink, nullRef.Data.ToDebugString());
                    }
                },
                { typeof(TransactionException),
                    () =>
                    {
                        errorCode = "800";
                        errorTitle = "Error general de la transacción";
                        detailErrorType = "Error general de la transacción";
                        simpleMessage = "No fue posible completar la transacción debido a un error general";
                        var tranEx = ((TransactionException)exception);
                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, tranEx.ToString());
                        detailCompleteExceptionMessage = string.Format("{0} {2} HResult: {1} {2} Help Link: {3} {2} Data: {4}", shortExceptionMessage, tranEx.HResult, Environment.NewLine, tranEx.HelpLink, tranEx.Data.ToDebugString());
                    }
                },
                { typeof(TransactionAbortedException),
                    () =>
                    {
                        errorCode = "801";
                        errorTitle = "Transacción abortada";
                        detailErrorType = "Transacción abortada";
                        simpleMessage = "Transacción abortada";
                        var tranEx = ((TransactionAbortedException)exception);
                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, tranEx.ToString());
                        detailCompleteExceptionMessage = string.Format("{0} {2} HResult: {1} {2} Help Link: {3} {2} Data: {4}", shortExceptionMessage, tranEx.HResult, Environment.NewLine, tranEx.HelpLink, tranEx.Data.ToDebugString());
                    }
                },
                { typeof(TransactionManagerCommunicationException),
                    () =>
                    {
                        errorCode = "802";
                        errorTitle = "Transacción abortada";
                        detailErrorType = "Transacción abortada";
                        simpleMessage = "Transacción abortada por problemas de comunicación";
                        var tranEx = ((TransactionManagerCommunicationException)exception);
                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, tranEx.ToString());
                        detailCompleteExceptionMessage = string.Format("{0} {2} HResult: {1} {2} Help Link: {3} {2} Data: {4}", shortExceptionMessage, tranEx.HResult, Environment.NewLine, tranEx.HelpLink, tranEx.Data.ToDebugString());
                    }
                },
                {typeof(EntityException),
                    () =>
                    {
                        errorCode = "1000";
                        errorTitle = "Error del proveedor de datos";
                        detailErrorType = "Error del proveedor de datos";
                        simpleMessage = "Error con el proveedor de datos";
                        var entityEx = ((EntityException)exception);
                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, entityEx.ToString());
                        detailCompleteExceptionMessage = string.Format("{0} {2} HResult: {1} {2} Help Link: {3} {2} Data: {4}", shortExceptionMessage, entityEx.HResult, Environment.NewLine, entityEx.HelpLink, entityEx.Data.ToDebugString());                
                    }
                },
                { typeof(Exception), 
                    () =>
                    {
                        detailClientMessage = MoreInfoDetailMessage(simpleMessage, exception.ToString());
                        detailCompleteExceptionMessage = string.Format("{0} {2} HResult: {1} {2} Help Link: {3} {2} Data: {4}", shortExceptionMessage, exception.HResult, Environment.NewLine, exception.HelpLink, exception.Data.ToDebugString());
                    }
                }
            };

            try
            {
                @switch[exception.GetType()]();
            }
            catch (Exception)
            {
                @switch[typeof(Exception)]();
            }

            #endregion

            AppErrorDetail detail = new AppErrorDetail(exception, errorTitle, errorCode, detailErrorType, simpleMessage, detailClientMessage, shortExceptionMessage, detailCompleteExceptionMessage, retryNotify, sendMail);

            return detail;
        }

        //public static void LogError(Exception exception)
        //{
        //    var errorDetail = GetErrorDetail(exception);
        //    LogFactory.CreateLog().Error(errorDetail.CompleteExceptionString(), errorDetail.Exception);
        //}

        //public static void LogError(AppErrorDetail errorDetail)
        //{
        //    LogFactory.CreateLog().Error(errorDetail.CompleteExceptionString(), errorDetail.Exception);
        //}

        //public static void SendMailError(AppErrorDetail errorDetail, string from, string to, bool async)
        //{
        //    var config = AppMailConfig.Default;

        //    AppMail mail = new AppMail(from, string.Format(":: Notificación de Fallas del SIM :: {0} :: {1}", config.AppName, errorDetail));

        //    if (string.IsNullOrEmpty(to))
        //        to = SettingsReader.GetKey("MailErrores.Destinatarios");

        //    string body = "<div style='padding: 0.7em; font-size: 1.2em;'>";
        //    body += "<p>";
        //    body += "<h2>";
        //    body += string.Format("Falla en la aplicación {0} del tipo: {1}", config.AppName, errorDetail.DetailErrorType);
        //    body += "</h2>";
        //    body += "</p>";
        //    body += "<p>";
        //    body += "<b>Detalles: </b><br />";
        //    body += "<b>" + errorDetail.ErrorTitle + "</b><br />";
        //    body += "<b>" + errorDetail + "</b><br /><br />";
        //    body += "<b>Error para mostrar: </b><br />";
        //    body += errorDetail.DetailClientMessage.Replace(Environment.NewLine, "<br />") + "<br /><br />";
        //    body += "<b>Resumen del error: </b><br />";
        //    body += errorDetail.ResumeErrorMessage.Replace(Environment.NewLine, "<br />") + "<br /><br />";
        //    body += "<b>Error detallado: </b><br />";
        //    body += errorDetail.CompleteExceptionMessage.Replace(Environment.NewLine, "<br />");
        //    body += "</p>";
        //    body += "<p><b>Exeption: </b><br />";
        //    body += errorDetail.Exception.ToString().Replace(Environment.NewLine, "<br />") + "<br /><br />";
        //    body += "<b>Complete Nested Exeption: </b><br />";
        //    body += errorDetail.CompleteExceptionString().Replace(Environment.NewLine, "<br />") + "<br /><br />";
        //    body += "<b>Trace: </b><br />";
        //    body += (errorDetail.Exception.StackTrace ?? string.Empty).Replace(Environment.NewLine, "<br />") + "</p>";
        //    body += "</div>";

        //    mail.Body = body;
        //    mail.To = AppMail.ConfigAdresses(to);

        //    AppMailManager.SendMail(config, mail, async);
        //}

        //public static void SendMailError(AppErrorDetail errorDetail, string from, string to)
        //{
        //    SendMailError(errorDetail, from, to, true);
        //}

        //public static void SendMailError(Exception error, string from, string to, bool async)
        //{
        //    var errorDetail = GetErrorDetail(error);
        //    SendMailError(errorDetail, from, to, async);
        //}

        //public static void SendMailError(Exception error, string from, string to)
        //{
        //    SendMailError(error, from, to, true);
        //}

        //public static void LogErrorAndSendMail(AppErrorDetail errorDetail)
        //{
        //    LogErrorAndSendMail(errorDetail, string.Empty, string.Empty, true);
        //}

        //public static void LogErrorAndSendMail(AppErrorDetail errorDetail, string from, string to)
        //{
        //    LogErrorAndSendMail(errorDetail, from, to, true);
        //}

        //public static void LogErrorAndSendMail(AppErrorDetail errorDetail, string from, string to, bool async)
        //{
        //    LogError(errorDetail);
        //    if (errorDetail.SendMail)
        //        SendMailError(errorDetail, from, to, async);
        //}

        //public static void LogErrorAndSendMail(Exception error)
        //{
        //    LogErrorAndSendMail(error, string.Empty, string.Empty, true);
        //}

        //public static void LogErrorAndSendMail(Exception error, string from, string to)
        //{
        //    LogErrorAndSendMail(error, from, to, true);
        //}

        //public static void LogErrorAndSendMail(Exception error, string from, string to, bool async)
        //{
        //    var errorDetail = GetErrorDetail(error);
        //    LogError(errorDetail);
        //    if (errorDetail.SendMail)
        //        SendMailError(errorDetail, from, to, async);
        //}

        static string MoreInfoDetailMessage(string detailMessage, string additionalInfo)
        {
            return detailMessage + "|" + additionalInfo.Replace("\"", "").Replace("\'", "").Replace(Environment.NewLine, "");
        }

        //public static void DirectLog(Exception error, bool sendMail)
        //{
        //    var filesDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\log";
        //    var fileName = "1-" + Guid.NewGuid() + ".direct-log";

        //    var filePath = Path.Combine(filesDirectory, fileName);

        //    var errorDetail = GetErrorDetail(error);

        //    File.WriteAllText(filePath, errorDetail.CompleteExceptionString());

        //    if (!sendMail)
        //        return;

        //    try
        //    {
        //        SendMailError(errorDetail, string.Empty, string.Empty, false);
        //    }
        //    catch (Exception ex)
        //    {
        //        DirectLog(ex, false);
        //    }
        //}
    }
}

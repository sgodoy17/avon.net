using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdentiGo.Transversal.Error
{
    public class MvcExceptionHandler
    {
        public static void HandleException(ControllerContext context, Exception exception)
        {
            AppErrorDetail errorDetail = null;
            MvcErrorModel errorModel = GetExceptionModel(context, exception, out errorDetail);

            //AppErrorManager.LogErrorAndSendMail(errorDetail,
            //    ConfigurationManager.AppSettings["Mail.Remitente"],
            //    ConfigurationManager.AppSettings["MailErrores.Destinatarios"]);

            context.RouteData.Values["controller"] = "Error";
            string actionToCall = "Index";

            context.RouteData.Values["action"] = actionToCall;
            context.RouteData.DataTokens["area"] = string.Empty;

            Controller errorController = new ErrorControllerBase();

            context.Controller = errorController;
            context.Controller.ViewData = new ViewDataDictionary(errorModel);

            errorController.ActionInvoker.InvokeAction(context, actionToCall);
        }

        public static MvcErrorModel GetExceptionModel(ControllerContext context, Exception exception, out AppErrorDetail errorDetail)
        {
            errorDetail = AppErrorManager.GetErrorDetail(exception);

            try
            {
                context.HttpContext.Response.StatusCode = 500;

            }
            catch
            {
                // ignored
            }

            MvcErrorModel errorModel =
                new MvcErrorModel(exception,
                    errorDetail.Id,
                    context.HttpContext.Request.IsAjaxRequest(),
                    context.RouteData,
                    errorDetail.ErrorTitle,
                    errorDetail.ErrorCode,
                    errorDetail.DetailClientMessage,
                    errorDetail.RetryNotify);

            return errorModel;
        }
    }
}

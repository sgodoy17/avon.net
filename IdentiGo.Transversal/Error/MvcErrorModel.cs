using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace IdentiGo.Transversal.Error
{
    public class MvcErrorModel
    {
        public const string RetryMessage = "Puede intentarlo de nuevo en unos minutos.";

        public Guid Id { get; set; }

        public bool IsAjaxRequest { get; set; }

        public RouteData CallingRoute { get; set; }

        public string ErrorTitle { get; set; }

        public string ErrorCode { get; set; }

        public string DetailMessage { get; set; }

        public string UserErrorMessage { get; private set; }

        public string CallingController { get; private set; }

        public string CallingAction { get; private set; }

        public string CallingArea { get; private set; }

        public bool RetryNotify { get; private set; }

        public HandleErrorInfo ErrorInfo { get; private set; }

        public MvcErrorModel()
        {

        }

        public MvcErrorModel(Exception exception,
            Guid id,
            bool isAjaxRequest,
            RouteData callingRoute,
            string errorTitle,
            string errorCode,
            string detailMessage,
            bool retryNotify)
        {
            this.Id = id;
            this.IsAjaxRequest = isAjaxRequest;
            this.DetailMessage = detailMessage;
            this.CallingRoute = callingRoute;
            this.ErrorCode = errorCode;
            this.ErrorTitle = errorTitle;
            this.RetryNotify = retryNotify;

            //Cambiado para que no muestre el ID del mensaje al usuario
            //string userMessageFormat = "<b>Id:</b> {2} \n<b>Mensaje:</b> {0} \n<b>Detalle:</b> {1} \n{3}";
            string userMessageFormat = "<b>Mensaje:</b> {0} \n<b>Detalle:</b> {1} \n{2}";

            this.UserErrorMessage =
                string.Format(userMessageFormat,
                this.ErrorTitle,
                this.DetailMessage,
                this.RetryNotify ? RetryMessage : string.Empty);

            this.CallingController = (string)CallingRoute.Values["controller"];
            this.CallingAction = (string)CallingRoute.Values["action"];
            this.CallingArea = CallingRoute.DataTokens["area"] == null ? string.Empty : (string)CallingRoute.DataTokens["area"];

            this.ErrorInfo = new HandleErrorInfo(exception, this.CallingController, this.CallingAction);

        }

    }
}

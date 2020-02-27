using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Transversal.Error
{
    public class AppErrorDetail
    {
        public Guid Id { get; private set; }
        public string ErrorTitle { get; set; }
        public string ErrorCode { get; set; }
        public string SimpleMessage { get; set; }
        public string DetailClientMessage { get; set; }
        public string ResumeErrorMessage { get; set; }
        public string CompleteExceptionMessage { get; set; }
        public string DetailErrorType { get; set; }
        public bool RetryNotify { get; set; }
        public bool SendMail { get; set; }

        public Exception Exception { get; set; }

        public AppErrorDetail(Exception exception, string errorTitle, string errorCode, string detailErrorType, string simpleMessage, string detailClientMessage, string resumeErrorMessage, string completeExceptionMessage, bool retryNotify, bool sendMail)
        {
            this.Id = Guid.NewGuid();

            this.ErrorTitle = errorTitle;
            this.ErrorCode = errorCode;
            this.CompleteExceptionMessage = completeExceptionMessage;
            this.DetailClientMessage = detailClientMessage;
            this.ResumeErrorMessage = resumeErrorMessage;
            this.Exception = exception;
            this.DetailErrorType = detailErrorType;
            this.RetryNotify = retryNotify;
            this.SendMail = sendMail;
            this.SimpleMessage = simpleMessage;
        }

        public override string ToString()
        {
            return string.Format("[{0}] [{1}] [{2}]", this.ErrorCode, this.DetailErrorType, this.Id);
        }

        public string CompleteExceptionString()
        {
            Exception ex = this.Exception;

            var completeException = "[ [DETAILS]: " + this.CompleteExceptionMessage + " ]" + Environment.NewLine + Environment.NewLine;
            completeException += "[ [CLIENT DETAILS]: " + this.DetailClientMessage + " ]" + Environment.NewLine + Environment.NewLine;

            completeException += "[ [RESUME]: " + this.ResumeErrorMessage + " ]" + Environment.NewLine + Environment.NewLine;

            while (ex != null)
            {
                completeException += "=>[NESTED EXEPTION]: " + ex + "" + Environment.NewLine;
                ex = ex.InnerException;
            }

            return completeException;
        }
    }
}

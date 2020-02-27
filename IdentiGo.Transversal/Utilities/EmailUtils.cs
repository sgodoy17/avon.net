using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Net.Mail;
using IdentiGo.Domain.Entity.General;
using System.Web.Configuration;
using System.Net;

namespace IdentiGo.Transversal.Utilities
{

    public static class EmailUtils
    {
        public static void SendEmailError(Config config, string message)
        {
            MailMessage mail = new MailMessage(WebConfigurationManager.AppSettings["EmailSend"], (config?.EmailTo ?? WebConfigurationManager.AppSettings["EmailTo"]));
            SmtpClient client = new SmtpClient();
            client.Port = int.Parse(WebConfigurationManager.AppSettings["EmailPort"]);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(WebConfigurationManager.AppSettings["EmailSend"], WebConfigurationManager.AppSettings["EmailPassword"]);
            client.Host = WebConfigurationManager.AppSettings["EmailHost"];
            client.EnableSsl = true;
            mail.Subject = config?.Subject ?? WebConfigurationManager.AppSettings["EmailSubject"];
            mail.Body = message;
            client.Send(mail);
        }
    }
}

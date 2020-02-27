using System;
using System.Web.Mvc;
using IdentiGo.WebManagement.Security;
using IdentiGo.Services.Log;
using IdentiGo.Domain.Enums;
using IdentiGo.WebManagement.Areas.Log.Models;
using System.Linq;

namespace IdentiGo.WebManagement.Areas.Log.Controllers
{
    [AuthorizeRoles(RoleName.Role4, RoleName.Role8)]
    public class LogSMSController : Controller
    {
        public readonly ILogSMSService LogSMSService;

        public LogSMSController(ILogSMSService logSMSService)
        {
            LogSMSService = logSMSService;
        }

        // GET: /BlackList/
        public ActionResult Index()
        {
            return View(new SMSModel());
        }

        [HttpPost]
        public ActionResult Index(SMSModel sms)
        {
            sms.LogSMS = LogSMSService.GetByTypeRandeDate(sms.DateStart, sms.DateEnd, sms.StateSMS);
            sms.Total = sms.LogSMS?.Count() ?? 0;
            sms.Accept = sms.StateSMS == TypeConsultCandidateResponse.Accept ? sms.LogSMS.Count() : sms.StateSMS == TypeConsultCandidateResponse.All ? LogSMSService.GetByTypeRandeDate(sms.DateStart, sms.DateEnd, TypeConsultCandidateResponse.Accept).Count() : 0;
            sms.NotAccept = sms.StateSMS == TypeConsultCandidateResponse.NoAccept ? sms.LogSMS.Count() : sms.StateSMS == TypeConsultCandidateResponse.All ? LogSMSService.GetByTypeRandeDate(sms.DateStart, sms.DateEnd, TypeConsultCandidateResponse.NoAccept).Count() : 0;

            return View(sms);
        }
        
        // GET: /BlackList/Delete/5
        public ActionResult Delete(Guid id)
        {
            try
            {
                var blackList = LogSMSService.Get(id);

                if (blackList == null)
                {
                    return HttpNotFound();
                }

                return View(blackList);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: /BlackList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (!ModelState.IsValid) return View();
            LogSMSService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
using System;
using System.Web.Mvc;
using IdentiGo.WebManagement.Security;
using IdentiGo.Services.Log;
using IdentiGo.Domain.Enums;

namespace IdentiGo.WebManagement.Areas.Log.Controllers
{
    [AuthorizeRoles(RoleName.Role4, RoleName.Role8)]
    public class LogIVRController : Controller
    {
        public readonly ILogIVRService LogIVRService;

        public LogIVRController(ILogIVRService logIVRService)
        {
            LogIVRService = logIVRService;
        }

        // GET: /BlackList/
        public ActionResult Index()
        {
            var blackListList = LogIVRService.GetAll();

            return View(blackListList);
        }
        
        // GET: /BlackList/Delete/5
        public ActionResult Delete(Guid id)
        {
            try
            {
                var blackList = LogIVRService.Get(id);

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
            LogIVRService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
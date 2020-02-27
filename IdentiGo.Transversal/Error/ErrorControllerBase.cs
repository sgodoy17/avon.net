using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdentiGo.Transversal.Error
{
    public class ErrorControllerBase : Controller
    {
        public ActionResult Index()
        {
            MvcErrorModel errorModel = (MvcErrorModel)ViewData.Model;

            ViewBag.ErrorTitle = errorModel.ErrorTitle;
            ViewBag.ErrorCode = errorModel.ErrorCode;

            return View(errorModel);
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            ViewBag.ErrorCode = 404;
            ViewBag.Title = "Recurso no encontrado";

            ViewBag.ErrorTitle = ViewBag.Title;

            return View();
        }
    }
}

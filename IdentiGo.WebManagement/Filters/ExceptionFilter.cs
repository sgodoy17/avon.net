using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentiGo.Transversal.Error;

namespace IdentiGo.WebManagement.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.IsCustomErrorEnabled )
            {
                HandleException(filterContext);
            }
        }

        public virtual void HandleException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            MvcExceptionHandler.HandleException(filterContext, filterContext.Exception);
        }

    }
}
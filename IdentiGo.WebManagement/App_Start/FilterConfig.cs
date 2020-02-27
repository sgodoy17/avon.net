using System.Web.Mvc;
using IdentiGo.WebManagement.Filters;

namespace IdentiGo.WebManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionFilter());
            filters.Add(new HandleExceptionAttribute());
            //filters.Add(new RequireRouteValues());
        }
    }
}

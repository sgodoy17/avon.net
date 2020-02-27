using IdentiGo.Transversal.IoC;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Owin.Security;

namespace IdentiGo.WebManagement
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IUnityContainer container = IoCFactory.GetUnityContainer();

            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<ApplicationSignInManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationSignInManager>()));
            container.RegisterType<ApplicationUserManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()));
            container.RegisterType<ApplicationRoleManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationRoleManager>()));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Mvc;
using System.Web.Http;
using Microsoft.Practices.Unity;
using System.Web.Routing;
using System.Web.Optimization;
using IdentiGo.Transversal.IoC;

[assembly: OwinStartup(typeof(IdentiGo.WebManagement.Startup))]
namespace IdentiGo.WebManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

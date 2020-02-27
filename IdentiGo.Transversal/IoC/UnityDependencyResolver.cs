using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;
using IDependencyResolver = System.Web.Mvc.IDependencyResolver;

namespace IdentiGo.Transversal.IoC
{
    public class UnityDependencyResolver : IDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver
    {
        protected IUnityContainer Container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.Container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return Container.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return Container.ResolveAll(serviceType);
            }
            catch
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
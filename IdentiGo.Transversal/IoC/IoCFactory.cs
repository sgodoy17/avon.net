using Component.Transversal.Adapters;
using IdentiGo.Data.Repository;
using IdentiGo.Data.UnitOfWork;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Services.General;
using IdentiGo.Transversal.IoC.Registrations;
using Microsoft.Practices.Unity;

namespace IdentiGo.Transversal.IoC
{
    public static class IoCFactory
    {
        public static IUnityContainer GetUnityContainer()
        {
            //Create UnityContainer          
            IUnityContainer container = new UnityContainer()
                .RegisterType<IDbFactory, DbFactory>(new HttpContextLifetimeManager<IDbFactory>())
                .RegisterType<IUnitOfWork, UnitOfWork>(new HttpContextLifetimeManager<IUnitOfWork>());

            TypeAdapterFactory.SetCurrent(() => new AutomapperTypeAdapterFactory(DtoMapReference.GetProfiles()));
            container = AppServicesConfiguration.RegisterTypes(container);
            container = RepositorysConfiguration.RegisterTypes(container);

            return container;
        }

        public static IUnityContainer GetUnityContainerW()
        {
            //Create UnityContainer          
            IUnityContainer container = new UnityContainer()
                .RegisterType<IDbFactory, DbFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IUnitOfWork, UnitOfWork>(new ContainerControlledLifetimeManager());

            container.RegisterType<IRepository<Nomination>, Repository<Nomination>>(new ContainerControlledLifetimeManager());
            container.RegisterType<INominationService, NominationService>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}

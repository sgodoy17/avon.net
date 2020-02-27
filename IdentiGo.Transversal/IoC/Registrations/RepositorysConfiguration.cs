using IdentiGo.Data.Repository;
using IdentiGo.Domain.Entity.AVON;
using IdentiGo.Domain.Entity.CIFIN;
using IdentiGo.Domain.Entity.General;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Entity.Log;
using IdentiGo.Domain.Entity.Master;
using IdentiGo.Domain.Security;
using Microsoft.Practices.Unity;

namespace IdentiGo.Transversal.IoC.Registrations
{
    public static class RepositorysConfiguration
    {
        public static IUnityContainer RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IRepository<User>, Repository<User>>(new HttpContextLifetimeManager<IRepository<User>>());
            container.RegisterType<IRepository<Company>, Repository<Company>>(new HttpContextLifetimeManager<IRepository<Company>>());
            container.RegisterType<IRepository<Role>, Repository<Role>>(new HttpContextLifetimeManager<IRepository<Role>>());
            container.RegisterType<IRepository<Config>, Repository<Config>>(new HttpContextLifetimeManager<IRepository<Config>>());
            container.RegisterType<IRepository<UserAffiliation>, Repository<UserAffiliation>>(new HttpContextLifetimeManager<IRepository<UserAffiliation>>());
            container.RegisterType<IRepository<AffiliationType>, Repository<AffiliationType>>(new HttpContextLifetimeManager<IRepository<AffiliationType>>());
            container.RegisterType<IRepository<Country>, Repository<Country>>(new HttpContextLifetimeManager<IRepository<Country>>());
            container.RegisterType<IRepository<Department>, Repository<Department>>(new HttpContextLifetimeManager<IRepository<Department>>());
            container.RegisterType<IRepository<City>, Repository<City>>(new HttpContextLifetimeManager<IRepository<City>>());
            container.RegisterType<IRepository<VoteSite>, Repository<VoteSite>>(new HttpContextLifetimeManager<IRepository<VoteSite>>());
            container.RegisterType<IRepository<SecretaryTransit>, Repository<SecretaryTransit>>(new HttpContextLifetimeManager<IRepository<SecretaryTransit>>());
            container.RegisterType<IRepository<Nomination>, Repository<Nomination>>(new HttpContextLifetimeManager<IRepository<Nomination>>());
            container.RegisterType<IRepository<BlackList>, Repository<BlackList>>(new HttpContextLifetimeManager<IRepository<BlackList>>());
            container.RegisterType<IRepository<RiskLevel>, Repository<RiskLevel>>(new HttpContextLifetimeManager<IRepository<RiskLevel>>());
            container.RegisterType<IRepository<Quota>, Repository<Quota>>(new HttpContextLifetimeManager<IRepository<Quota>>());
            container.RegisterType<IRepository<Zone>, Repository<Zone>>(new HttpContextLifetimeManager<IRepository<Zone>>());
            container.RegisterType<IRepository<Prospecta>, Repository<Prospecta>>(new HttpContextLifetimeManager<IRepository<Prospecta>>());
            container.RegisterType<IRepository<ValidadorPlus>, Repository<ValidadorPlus>>(new HttpContextLifetimeManager<IRepository<ValidadorPlus>>());
            container.RegisterType<IRepository<CashPayment>, Repository<CashPayment>>(new HttpContextLifetimeManager<IRepository<CashPayment>>());
            container.RegisterType<IRepository<Division>, Repository<Division>>(new HttpContextLifetimeManager<IRepository<Division>>());
            container.RegisterType<IRepository<Unit>, Repository<Unit>>(new HttpContextLifetimeManager<IRepository<Unit>>());
            container.RegisterType<IRepository<Campaing>, Repository<Campaing>>(new HttpContextLifetimeManager<IRepository<Campaing>>());
            container.RegisterType<IRepository<LogIVR>, Repository<LogIVR>>(new HttpContextLifetimeManager<IRepository<LogIVR>>());
            container.RegisterType<IRepository<LogSMS>, Repository<LogSMS>>(new HttpContextLifetimeManager<IRepository<LogSMS>>());
            container.RegisterType<IRepository<GroupRole>, Repository<GroupRole>>(new HttpContextLifetimeManager<IRepository<GroupRole>>());
            container.RegisterType<IRepository<NominationResponse>, Repository<NominationResponse>>(new HttpContextLifetimeManager<IRepository<NominationResponse>>());
            container.RegisterType<IRepository<NominationHistoric>, Repository<NominationHistoric>>(new HttpContextLifetimeManager<IRepository<NominationHistoric>>());

            return container;
        }
    }
}

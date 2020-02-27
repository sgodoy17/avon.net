using IdentiGo.Services.General;
using IdentiGo.Services.Security;
using Microsoft.Practices.Unity;
using IdentiGo.ExternalServices;
using IdentiGo.Services.Master;
using IdentiGo.Transversal.Services;
using IdentiGo.Services.CIFIN;
using IdentiGo.Services.Log;
using IdentiGo.Transversal.Services.NominationCandidate;
using IdentiGo.Services.Avon;

namespace IdentiGo.Transversal.IoC.Registrations
{
    public static class AppServicesConfiguration
    {
        public static IUnityContainer RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IUserService, UserService>(new HttpContextLifetimeManager<IUserService>());
            container.RegisterType<ICompanyService, CompanyService>(new HttpContextLifetimeManager<ICompanyService>());
            container.RegisterType<IRoleService, RoleService>(new HttpContextLifetimeManager<IRoleService>());
            container.RegisterType<IConfigService, ConfigService>(new HttpContextLifetimeManager<IConfigService>());
            container.RegisterType<IUserAffiliationService, UserAffiliationService>(new HttpContextLifetimeManager<IUserAffiliationService>());
            container.RegisterType<IAffiliationTypeService, AffiliationTypeService>(new HttpContextLifetimeManager<IAffiliationTypeService>());
            container.RegisterType<ICountryService, CountryService>(new HttpContextLifetimeManager<ICountryService>());
            container.RegisterType<IDepartmentService, DepartmentService>(new HttpContextLifetimeManager<IDepartmentService>());
            container.RegisterType<ICityService, CityService>(new HttpContextLifetimeManager<ICityService>());
            container.RegisterType<IVoteSiteService, VoteSiteService>(new HttpContextLifetimeManager<IVoteSiteService>());
            container.RegisterType<ISecretaryTransitService, SecretaryTransitService>(new HttpContextLifetimeManager<ISecretaryTransitService>());
            container.RegisterType<INominationService, NominationService>(new HttpContextLifetimeManager<INominationService>());
            container.RegisterType<IBlackListService, BlackListService>(new HttpContextLifetimeManager<IBlackListService>());
            container.RegisterType<ITestService, TestService>(new HttpContextLifetimeManager<ITestService>());
            container.RegisterType<IRiskLevelService, RiskLevelService>(new HttpContextLifetimeManager<IRiskLevelService>());
            container.RegisterType<IQuotaService, QuotaService>(new HttpContextLifetimeManager<IQuotaService>());
            container.RegisterType<IZoneService, ZoneService>(new HttpContextLifetimeManager<IZoneService>());
            container.RegisterType<IDivisionService, DivisionService>(new HttpContextLifetimeManager<IDivisionService>());
            container.RegisterType<IUnitService, UnitService>(new HttpContextLifetimeManager<IUnitService>());
            container.RegisterType<ICampaingService, CampaingService>(new HttpContextLifetimeManager<ICampaingService>());
            container.RegisterType<ILogIVRService, LogIVRService>(new HttpContextLifetimeManager<ILogIVRService>());
            container.RegisterType<ILogSMSService, LogSMSService>(new HttpContextLifetimeManager<ILogSMSService>());
            container.RegisterType<IGroupRoleService, GroupRoleService>(new HttpContextLifetimeManager<IGroupRoleService>());
            container.RegisterType<INominationResponseService, NominationResponseService>(new HttpContextLifetimeManager<INominationResponseService>());
            container.RegisterType<INominationHistoricService, NominationHistoricService>(new HttpContextLifetimeManager<INominationHistoricService>());
            container.RegisterType<INominationManagerService, NominationManagerService>(new HttpContextLifetimeManager<INominationManagerService>());
            container.RegisterType<INominationProcessService, NominationProcessService>(new HttpContextLifetimeManager<INominationProcessService>());
            container.RegisterType<INominationGeneralService, NominationGeneralService>(new HttpContextLifetimeManager<INominationGeneralService>());
            container.RegisterType<ICandidateService, CandidateService>(new HttpContextLifetimeManager<ICandidateService>());
            container.RegisterType<IReportService, ReportService>(new HttpContextLifetimeManager<IReportService>());
            container.RegisterType<IValidadorPlusService, ValidadorPlusService>(new HttpContextLifetimeManager<IValidadorPlusService>());
            container.RegisterType<ICashPaymentService, CashPaymentService>(new HttpContextLifetimeManager<ICashPaymentService>());
            container.RegisterType<IProspectaService, ProspectaService>(new HttpContextLifetimeManager<IProspectaService>());
            container.RegisterType<ILoadDataFileService, LoadDataFileService>(new HttpContextLifetimeManager<ILoadDataFileService>());
            container.RegisterType<IIVRService, IVRService>(new HttpContextLifetimeManager<IIVRService>());
            container.RegisterType<ISMSService, SMSService>(new HttpContextLifetimeManager<ISMSService>());
            container.RegisterType<IIntrasoftService, IntrasoftService>(new HttpContextLifetimeManager<IIntrasoftService>());
            container.RegisterType<IManagerZoneReportService, ManagerZoneReportService>(new HttpContextLifetimeManager<IManagerZoneReportService>());
            container.RegisterType<IAdminReportService, AdminReportService>(new HttpContextLifetimeManager<IAdminReportService>());

            return container;
        }
    }
}

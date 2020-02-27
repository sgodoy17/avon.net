using System;
using System.IO;
using System.Linq;
using System.Text;
using Component.Transversal.Adapters;
using Component.Transversal.Utilities;
using IdentiGo.Domain.Entity.IdentiGo.Report;
using IdentiGo.Domain.Enums;
using IdentiGo.Services.General;
using IdentiGo.Services.Master;

namespace IdentiGo.Transversal.Services
{
    public interface IReportService
    {
        DetailReport DetailReport(int companyId, string document);

        ManagementReport ManagementReport(ManagementReport management);

        Stream DetailReportDownload(DetailReport item);
    }

    public class ReportService : IReportService
    {
        public readonly IUserAffiliationService UserAffiliationService;
        public readonly ICityService CityService;
        public readonly IDepartmentService DepartmentService;
        public readonly IConfigService ConfigService;
        public readonly IVoteSiteService VoteSiteService;
        public readonly ISecretaryTransitService SecretaryTransitService;
        public readonly IBlackListService BlackListService;
        public readonly INominationService UserValidationService;

        public ITypeAdapter TypeAdapter = TypeAdapterFactory.CreateAdapter();

        public ReportService(IUserAffiliationService userAffiliationService,
            ICityService cityService,
            IDepartmentService departmentService,
            IConfigService configService,
            IVoteSiteService voteSiteService,
            ISecretaryTransitService secretaryTransitService,
            IBlackListService blackListService,
            INominationService userValidationService)
        {
            UserAffiliationService = userAffiliationService;
            CityService = cityService;
            DepartmentService = departmentService;
            ConfigService = configService;
            VoteSiteService = voteSiteService;
            SecretaryTransitService = secretaryTransitService;
            BlackListService = blackListService;
            UserValidationService = userValidationService;
        }

        public DetailReport DetailReport(int companyId, string document)
        {
            var user = UserValidationService.GetLastByDocument(document);

            if (user == null)
                throw new Exception("El no se encontro ningún registro con este documento");

            var detailReport = new DetailReport
            {
                NumberDocument = document
            };

            return detailReport;
        }

        public Stream DetailReportDownload(DetailReport item)
        {

            var byteArray = Encoding.ASCII.GetBytes("");
            var stream = new MemoryStream(byteArray);
            stream.Flush();
            return stream;
        }

        public ManagementReport ManagementReport(ManagementReport management)
        {
            var userList = UserValidationService.GetByDateRange(management.StartDate, management.EndDate);

            if (!userList.Any())
                return management;

            foreach (var user in userList)
            {
                management.NumberPerson += 1;

                management.NumberPersonSuccess += user.State == State.Success ? 1 : 0;

                management.NumberPersonFailed += user.State != State.Success ? 1 : 0;

            }

            management.NumberTriedPerson = Math.Round(management.NumberTriedPerson / management.NumberPersonAnswer, 2);

            return management;
        }
    }
}

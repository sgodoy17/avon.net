using Component.Transversal.Adapters;
using IdentiGo.Data;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Services.General;
using IdentiGo.Services.Master;
using IdentiGo.Services.Security;
using IdentiGo.Transversal.Model;
using System.Data.Entity;
using System.Linq;

namespace IdentiGo.Transversal.Services
{
    public interface IAdminReportService
    {
        AdminReportModel ConsulReport(AdminReportModel item);

        CandidateReport ConsulCandidateReport(CandidateReport item);

        AdminReportModel ConsulReportZone(AdminReportModel item);
    }

    public class AdminReportService : IAdminReportService
    {
        public readonly IUserService UserService;
        public readonly IZoneService ZoneService;
        public readonly IUnitService UnitService;
        public readonly INominationService NominationService;
        public ITypeAdapter TypeAdapter = TypeAdapterFactory.CreateAdapter();

        public AdminReportService(IUserService userService,
            IZoneService zoneService,
            IUnitService unitService,
            INominationService nominationService)
        {
            UserService = userService;
            ZoneService = zoneService;
            UnitService = unitService;
            NominationService = nominationService;
        }

        public AdminReportModel ConsulReport(AdminReportModel item)
        {
            item.ListIimpresario = new System.Collections.Generic.List<ListIimpresarioReportModel>();
            var zone = ZoneService.Get(item.ZoneId);
            item.Success = NominationService.GetCountValidByZone(item.ZoneId, item.CampaingId, item.DateStart, item.DateEnd);
            item.Invalid = NominationService.GetCountInValidByZone(item.ZoneId, item.CampaingId, item.DateStart, item.DateEnd);
            item.Pending = NominationService.GetCountPendingByZone(item.ZoneId, item.CampaingId, item.DateStart, item.DateEnd);
            item.Total = NominationService.GetCountTotalByZone(item.ZoneId, item.CampaingId, item.DateStart, item.DateEnd);

            foreach (var zoneItem in ZoneService.GetMany(x => item.ZoneId == null || x.Id == item.ZoneId))
            {
                if (item.TypeConsulAdmin == TypeConsulManagerZone.Impresario)
                {
                    var listItem = new ListIimpresarioReportModel
                    {
                        Code = $"{zoneItem.Number} - {zoneItem.Code} - {zoneItem.Name} - Gerente de Zona",
                        CodeId = zoneItem.Id
                    };

                    listItem.Success = NominationService.GetCountValidByCodeUser(zoneItem.Code, item.CampaingId, item.DateStart, item.DateEnd);
                    listItem.Invalid = NominationService.GetCountInValidByCodeUser(zoneItem.Code, item.CampaingId, item.DateStart, item.DateEnd);
                    listItem.Pending = NominationService.GetCountPendingByCodeUser(zoneItem.Code, item.CampaingId, item.DateStart, item.DateEnd);
                    listItem.Total = NominationService.GetCountTotalByCodeUser(zoneItem.Code, item.CampaingId, item.DateStart, item.DateEnd);
                    item.ListIimpresario.Add(listItem);
                }
                else
                {
                    var listItem = new ListIimpresarioReportModel
                    {
                        Code = $"{zoneItem.Number} - {zoneItem.Code} - {zoneItem.Name} - Sin Unidad",
                        CodeId = zoneItem.Id
                    };

                    listItem.Success = NominationService.GetCountValidByCodeUnit(null, item.CampaingId, zoneItem.Id, item.DateStart, item.DateEnd);
                    listItem.Invalid = NominationService.GetCountInValidByCodeUnit(null, item.CampaingId, zoneItem.Id, item.DateStart, item.DateEnd);
                    listItem.Pending = NominationService.GetCountPendingByCodeUnit(null, item.CampaingId, zoneItem.Id, item.DateStart, item.DateEnd);
                    listItem.Total = NominationService.GetCountTotalByCodeUnit(null, item.CampaingId, zoneItem.Id, item.DateStart, item.DateEnd);
                    item.ListIimpresario.Add(listItem);
                }
            }

            var unitList = UnitService.GetMany(x => item.ZoneId != null || x.ZoneId == null);
            foreach (var unit in unitList)
            {
                var impresario = new ListIimpresarioReportModel
                {
                    Code = $"{unit.Number} - {unit.Code} - {unit.Name}",
                    CodeId = unit.Id
                };

                if (item.TypeConsulAdmin == TypeConsulManagerZone.Impresario)
                {
                    impresario.Success = NominationService.GetCountValidByCodeUser(unit.Code, item.CampaingId, item.DateStart, item.DateEnd);
                    impresario.Invalid = NominationService.GetCountInValidByCodeUser(unit.Code, item.CampaingId, item.DateStart, item.DateEnd);
                    impresario.Pending = NominationService.GetCountPendingByCodeUser(unit.Code, item.CampaingId, item.DateStart, item.DateEnd);
                    impresario.Total = NominationService.GetCountTotalByCodeUser(unit.Code, item.CampaingId, item.DateStart, item.DateEnd);
                }
                else
                {
                    impresario.Success = NominationService.GetCountValidByCodeUnit(unit.Id, item.CampaingId, unit.Zone.Id, item.DateStart, item.DateEnd);
                    impresario.Invalid = NominationService.GetCountInValidByCodeUnit(unit.Id, item.CampaingId, unit.Zone.Id, item.DateStart, item.DateEnd);
                    impresario.Pending = NominationService.GetCountPendingByCodeUnit(unit.Id, item.CampaingId, unit.Zone.Id, item.DateStart, item.DateEnd);
                    impresario.Total = NominationService.GetCountTotalByCodeUnit(unit.Id, item.CampaingId, unit.Zone.Id, item.DateStart, item.DateEnd);
                }
                item.ListIimpresario.Add(impresario);
            }

            return item;
        }

        public AdminReportModel ConsulReportZone(AdminReportModel item)
        {
            item.ListIimpresario = new System.Collections.Generic.List<ListIimpresarioReportModel>();
            item.Success = NominationService.GetCountValidByZone(null, item.CampaingId, item.DateStart, item.DateEnd);
            item.Invalid = NominationService.GetCountInValidByZone(null, item.CampaingId, item.DateStart, item.DateEnd);
            item.Pending = NominationService.GetCountPendingByZone(null, item.CampaingId, item.DateStart, item.DateEnd);
            item.Total = NominationService.GetCountTotalByZone(null, item.CampaingId, item.DateStart, item.DateEnd);

            foreach (var zoneItem in ZoneService.GetAll())
            {
                    var listItem = new ListIimpresarioReportModel
                    {
                        Code = $"{zoneItem.Number} - {zoneItem.Code} - {zoneItem.Name}",
                        CodeId = zoneItem.Id
                    };

                    listItem.Success = NominationService.GetCountValidByZone(zoneItem.Id, item.CampaingId, item.DateStart, item.DateEnd);
                    listItem.Invalid = NominationService.GetCountInValidByZone(zoneItem.Id, item.CampaingId, item.DateStart, item.DateEnd);
                    listItem.Pending = NominationService.GetCountPendingByZone(zoneItem.Id, item.CampaingId, item.DateStart, item.DateEnd);
                    listItem.Total = NominationService.GetCountTotalByZone(zoneItem.Id, item.CampaingId, item.DateStart, item.DateEnd);
                    item.ListIimpresario.Add(listItem);
            }

            return item;
        }

        public CandidateReport ConsulCandidateReport(CandidateReport item)
        {
            item.ListNomination = new System.Collections.Generic.List<Nomination>();
            var unit = UnitService.Get(item.UnitId);
            var zone = ZoneService.Get(item.UnitId) ?? unit?.Zone;

            if (item.TypeConsulManagerZone == TypeConsulManagerZone.Impresario)
            {
                var code = unit?.Code ?? zone.Code;
                item.Name = unit?.Name ?? zone.Name;
                item.Success = NominationService.GetCountValidByCodeUser(code, item.CampaingId, item.DateStart, item.DateEnd);
                item.Invalid = NominationService.GetCountInValidByCodeUser(code, item.CampaingId, item.DateStart, item.DateEnd);
                item.Pending = NominationService.GetCountPendingByCodeUser(code, item.CampaingId, item.DateStart, item.DateEnd);
                item.Total = NominationService.GetCountTotalByCodeUser(code, item.CampaingId, item.DateStart, item.DateEnd);
                item.ListNomination = MainContext.Create().UserValidation.Where(x => x.CodeUser == code && (item.CampaingId == null || x.CampaingId == item.CampaingId) && (item.DateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= item.DateStart) && (item.DateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= item.DateEnd)).ToList();
                //item.ListNomination = NominationService.GetMany(x => x.CodeUser == code && (item.DateStart == null || x.DateCreated.Date >= item.DateStart.Value) && (item.DateEnd == null || x.DateCreated.Date <= item.DateEnd.Value)).ToList();
            }
            else
            {
                item.Name = unit?.Name ?? zone.Name;
                item.Success = NominationService.GetCountValidByCodeUnit(unit?.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                item.Invalid = NominationService.GetCountInValidByCodeUnit(unit?.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                item.Pending = NominationService.GetCountPendingByCodeUnit(unit?.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                item.Total = NominationService.GetCountTotalByCodeUnit(unit?.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                item.Total = NominationService.GetCountTotalByCodeUnit(unit?.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                //item.ListNomination = MainContext.Create().UserValidation.Where(x => x.ZoneId == zone.Id && (unit.Id == null || x.UnitId == unit.Id) && (item.DateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= item.DateStart) && (item.DateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= item.DateEnd)).ToList();
                item.ListNomination = NominationService.GetMany(x => x.ZoneId == zone.Id && x.UnitId == unit?.Id && (item.CampaingId == null || x.CampaingId == item.CampaingId) && (item.DateStart == null || x.DateCreated.Date >= item.DateStart.Value) && (item.DateEnd == null || x.DateCreated.Date <= item.DateEnd.Value)).ToList();
            }

            return item;
        }
    }
}

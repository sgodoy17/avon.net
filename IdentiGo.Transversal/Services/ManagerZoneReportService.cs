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
    public interface IManagerZoneReportService
    {
        ManagerZoneReportModel ConsulReport(ManagerZoneReportModel item);

        CandidateReport ConsulCandidateReport(CandidateReport item);
    }

    public class ManagerZoneReportService : IManagerZoneReportService
    {
        public readonly IUserService UserService;
        public readonly IZoneService ZoneService;
        public readonly IUnitService UnitService;
        public readonly INominationService NominationService;


        public ITypeAdapter TypeAdapter = TypeAdapterFactory.CreateAdapter();

        public ManagerZoneReportService(IUserService userService,
            IZoneService zoneService,
            IUnitService unitService,
            INominationService nominationService)
        {
            UserService = userService;
            ZoneService = zoneService;
            UnitService = unitService;
            NominationService = nominationService;
        }

        public ManagerZoneReportModel ConsulReport(ManagerZoneReportModel item)
        {
            item.ListIimpresario = new System.Collections.Generic.List<ListIimpresarioReportModel>();
            var zone = ZoneService.Get(item.ZoneId);
            item.Success = NominationService.GetCountValidByZone(zone.Id, item.CampaingId, item.DateStart, item.DateEnd);
            item.Invalid = NominationService.GetCountInValidByZone(zone.Id, item.CampaingId, item.DateStart, item.DateEnd);
            item.Pending = NominationService.GetCountPendingByZone(zone.Id, item.CampaingId, item.DateStart, item.DateEnd);
            item.Total = NominationService.GetCountTotalByZone(zone.Id, item.CampaingId, item.DateStart, item.DateEnd);

            if (item.TypeConsulManagerZone == TypeConsulManagerZone.Impresario)
            {
                var listItem = new ListIimpresarioReportModel
                {
                    Code = $"{zone.Number} - {zone.Code} - {zone.Name} - Gerente de Zona",
                    CodeId = zone.Id
                };

                listItem.Success = NominationService.GetCountValidByCodeUser(zone.Code, item.CampaingId, item.DateStart, item.DateEnd);
                listItem.Invalid = NominationService.GetCountInValidByCodeUser(zone.Code, item.CampaingId, item.DateStart, item.DateEnd);
                listItem.Pending = NominationService.GetCountPendingByCodeUser(zone.Code, item.CampaingId, item.DateStart, item.DateEnd);
                listItem.Total = NominationService.GetCountTotalByCodeUser(zone.Code, item.CampaingId, item.DateStart, item.DateEnd);
                item.ListIimpresario.Add(listItem);
            }
            else
            {
                var listItem = new ListIimpresarioReportModel
                {
                    Code = $"{zone.Number} - {zone.Code} - {zone.Name} - Sin Unidad",
                    CodeId = zone.Id
                };

                listItem.Success = NominationService.GetCountValidByCodeUnit(null, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                listItem.Invalid = NominationService.GetCountInValidByCodeUnit(null, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                listItem.Pending = NominationService.GetCountPendingByCodeUnit(null, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                listItem.Total = NominationService.GetCountTotalByCodeUnit(null, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                item.ListIimpresario.Add(listItem);
            }

            var unitList = UnitService.GetMany(x => x.ZoneId == zone.Id);

            foreach (var unit in unitList)
            {
                var impresario = new ListIimpresarioReportModel
                {
                    Code = $"{unit.Number} - {unit.Code} - {unit.Name}",
                    CodeId = unit.Id
                };

                if (item.TypeConsulManagerZone == TypeConsulManagerZone.Impresario)
                {
                    impresario.Success = NominationService.GetCountValidByCodeUser(unit.Code, item.CampaingId, item.DateStart, item.DateEnd);
                    impresario.Invalid = NominationService.GetCountInValidByCodeUser(unit.Code, item.CampaingId, item.DateStart, item.DateEnd);
                    impresario.Pending = NominationService.GetCountPendingByCodeUser(unit.Code, item.CampaingId, item.DateStart, item.DateEnd);
                    impresario.Total = NominationService.GetCountTotalByCodeUser(unit.Code, item.CampaingId, item.DateStart, item.DateEnd);
                }
                else
                {
                    impresario.Success = NominationService.GetCountValidByCodeUnit(unit.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                    impresario.Invalid = NominationService.GetCountInValidByCodeUnit(unit.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                    impresario.Pending = NominationService.GetCountPendingByCodeUnit(unit.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                    impresario.Total = NominationService.GetCountTotalByCodeUnit(unit.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                }

                item.ListIimpresario.Add(impresario);
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
                item.ListNomination = MainContext.Create().UserValidation.Where(x => x.CodeUser == code && (item.DateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= item.DateStart) && (item.DateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= item.DateEnd)).ToList();
                //item.ListNomination = NominationService.GetMany(x => x.CodeUser == code && (item.DateStart == null || x.DateCreated.Date >= item.DateStart.Value) && (item.DateEnd == null || x.DateCreated.Date <= item.DateEnd.Value)).ToList();
            }
            else
            {
                item.Name = unit?.Name ?? zone.Name;
                item.Success = NominationService.GetCountValidByCodeUnit(unit?.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                item.Invalid = NominationService.GetCountInValidByCodeUnit(unit?.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                item.Pending = NominationService.GetCountPendingByCodeUnit(unit?.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                item.Total = NominationService.GetCountTotalByCodeUnit(unit?.Id, item.CampaingId, zone.Id, item.DateStart, item.DateEnd);
                //item.ListNomination = MainContext.Create().UserValidation.Where(x => x.ZoneId == zone.Id && (item.UnitId == null || x.UnitId == unit.Id) && (item.DateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= item.DateStart) && (item.DateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= item.DateEnd)).ToList();
                item.ListNomination = NominationService.GetMany(x => x.ZoneId == zone.Id && x.UnitId == unit?.Id && (item.DateStart == null || x.DateCreated.Date >= item.DateStart.Value) && (item.DateEnd == null || x.DateCreated.Date <= item.DateEnd.Value)).ToList();
            }

            return item;
        }
    }
}

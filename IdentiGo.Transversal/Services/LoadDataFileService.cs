using System;
using System.IO;
using Component.Transversal.Adapters;
using IdentiGo.Services.Master;
using IdentiGo.Transversal.Utilities;
using System.Web;
using IdentiGo.Domain.Entity.Master;

namespace IdentiGo.Transversal.Services
{
    public interface ILoadDataFileService
    {
        void LoadZone(HttpPostedFileBase file);
        void LoadUnit(HttpPostedFileBase file);
        void LoadDivision(HttpPostedFileBase file);
    }

    public class LoadDataFileService : ILoadDataFileService
    {
        public readonly IZoneService ZoneService;
        public readonly IDivisionService DivisionService;
        public readonly IUnitService UnitService;

        public ITypeAdapter TypeAdapter = TypeAdapterFactory.CreateAdapter();

        public LoadDataFileService(IZoneService zoneService,
            IDivisionService divisionService,
            IUnitService unitService)
        {
            ZoneService = zoneService;
            DivisionService = divisionService;
            UnitService = unitService;
        }

        public void LoadZone(HttpPostedFileBase file)
        {
            ReadExcel.InitializeSheet(file.InputStream, Path.GetExtension(file.FileName));

            var list = ReadExcel.ConvertToList<Zone>();

            foreach (var zone in list)
            {
                if (string.IsNullOrEmpty(zone.Number) || string.IsNullOrEmpty(zone.NumberDivision))
                    continue;

                var division = DivisionService.GetByNumber(zone.NumberDivision) ?? new Division();

                if (division.Id == Guid.Empty)
                    continue;

                var zoneCurrent = ZoneService.GetByNumber(zone.Number) ?? zone;
                zoneCurrent.Code = zone.Code;
                zoneCurrent.Name = zone.Name;
                zoneCurrent.DivisionId = division.Id;

                ZoneService.AddOrUpdate(zoneCurrent);
            }
        }

        public void LoadUnit(HttpPostedFileBase file)
        {
            ReadExcel.InitializeSheet(file.InputStream, Path.GetExtension(file.FileName));

            var list = ReadExcel.ConvertToList<Unit>();

            foreach (var unit in list)
            {
                if (string.IsNullOrEmpty(unit.Number) || string.IsNullOrEmpty(unit.NumberZone))
                    continue;

                var zone = ZoneService.GetByNumber(unit.NumberZone);

                if (zone?.Id == Guid.Empty)
                    continue;

                var unitCurrent = UnitService.GetByCodeUnitCodeZone(unit.Number, unit.NumberZone) ?? unit;
                unitCurrent.Code = unit.Code;
                unitCurrent.Name = unit.Name;
                unitCurrent.ZoneId = zone.Id;

                UnitService.AddOrUpdate(unitCurrent);
            }
        }

        public void LoadDivision(HttpPostedFileBase file)
        {
            ReadExcel.InitializeSheet(file.InputStream, Path.GetExtension(file.FileName));

            var list = ReadExcel.ConvertToList<Division>();

            foreach (var divsion in list)
            {
                if (string.IsNullOrEmpty(divsion.Number)) continue;

                var divisionCurrent = DivisionService.GetByNumber(divsion.Number) ?? divsion;
                divisionCurrent.Code = divsion.Code;
                divisionCurrent.Name = divsion.Name;

                DivisionService.AddOrUpdate(divisionCurrent);
            }
        }
    }
}

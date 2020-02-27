using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IdentiGo.Data.Repository;
using IdentiGo.Domain.DTO.Master;
using IdentiGo.Domain.Entity.Master;
using IdentiGo.Services.Base;

namespace IdentiGo.Services.Master
{
    public interface IUnitService : IBasicAppService<Unit>
    {
        Unit GetByNumber(string number);

        Unit GetByCodeUnitCodeZone(string code, string codeZone);
    }

    public class UnitService : BasicAppService<Unit>, IUnitService
    {
        readonly IRepository<Unit> _repository;

        public UnitService(IRepository<Unit> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public Unit GetByNumber(string code)
        {
            return _repository.GetManyNoTracking(x => x.Number == code).FirstOrDefault();
        }

        public Unit GetByCodeUnitCodeZone(string number, string numberZone)
        {
            return _repository.GetManyNoTracking(x => x.Number == number && x.Zone.Number == numberZone).FirstOrDefault();
        }
    }
}

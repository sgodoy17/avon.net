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
    public interface IZoneService : IBasicAppService<Zone>
    {
        Zone GetByNumber(string number);
        Zone GetByCode(string code);
    }

    public class ZoneService : BasicAppService<Zone>, IZoneService
    {
        readonly IRepository<Zone> _repository;

        public ZoneService(IRepository<Zone> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public Zone GetByNumber(string number)
        {
            return _repository.GetManyNoTracking(x => x.Number.Equals(number, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        public Zone GetByCode(string code)
        {
            return _repository.GetFirsOne(x => x.Code == code);
        }
    }
}

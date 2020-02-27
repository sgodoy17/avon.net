using IdentiGo.Data;
using IdentiGo.Data.Repository;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Services.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IdentiGo.Services.General
{
    public interface INominationHistoricService : IBasicAppService<NominationHistoric>
    {
        IEnumerable<NominationHistoric> GetByRangeDate(DateTime? dateStart, DateTime? dateEnd);

        IEnumerable<NominationHistoric> GetByNomitationId(Guid id);
    }

    public class NominationHistoricService : BasicAppService<NominationHistoric>, INominationHistoricService
    {
        readonly IRepository<NominationHistoric> _repository;

        public NominationHistoricService(IRepository<NominationHistoric> repository) 
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<NominationHistoric> GetByRangeDate(DateTime? dateStart, DateTime? dateEnd)
        {
            return _repository.GetMany(x => x.DateCreated.Date >= dateStart && x.DateCreated.Date <= dateEnd);            
        }

        public IEnumerable<NominationHistoric> GetByNomitationId(Guid id)
        {
            return _repository.GetManyNoTracking(x => x.NominationId == id);
        }
    }
}

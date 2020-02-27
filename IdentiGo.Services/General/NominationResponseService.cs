using Component.Transversal.Utilities;
using IdentiGo.Data.Repository;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Enums;
using IdentiGo.Services.Base;
using System;
using System.Collections.Generic;

namespace IdentiGo.Services.General
{
    public interface INominationResponseService : IBasicAppService<NominationResponse>
    {
        IEnumerable<NominationResponse> GetByRandeDate(DateTime dateStart, DateTime dateEnd, TypeConsultCandidateResponse typeConsult);

        IEnumerable<NominationResponse> GetByZoneRangeDate(Guid zoneId, DateTime dateStart, DateTime dateEnd, TypeConsultCandidateResponse typeConsult);
    }

    public class NominationResponseService : BasicAppService<NominationResponse>,
        INominationResponseService
    {
        readonly IRepository<NominationResponse> _repository;

        public NominationResponseService(IRepository<NominationResponse> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<NominationResponse> GetByRandeDate(DateTime dateStart, DateTime dateEnd, TypeConsultCandidateResponse typeConsult)
        {

            return _repository.GetMany(x => x.Date.Date >= dateStart && x.Date.Date <= dateEnd
            && (typeConsult == TypeConsultCandidateResponse.All ? Array.Exists(Params.codeResponse, exist => exist.Equals(x.Message)) :
                (typeConsult == TypeConsultCandidateResponse.Accept ? Array.Exists(Params.codeResponseTrue, exist => exist.Equals(x.Message)) : Array.Exists(Params.codeResponseFalse, exist => exist.Equals(x.Message)))
            ));
        }

        public IEnumerable<NominationResponse> GetByZoneRangeDate(Guid zoneId, DateTime dateStart, DateTime dateEnd, TypeConsultCandidateResponse typeConsult)
        {
            return _repository.GetMany(x => x.Date.Date >= dateStart && x.Date.Date <= dateEnd
            && (typeConsult == TypeConsultCandidateResponse.All ? Array.Exists(Params.codeResponse, exist => exist.Equals(x.Message)) :
                (typeConsult == TypeConsultCandidateResponse.Accept ? Array.Exists(Params.codeResponseTrue, exist => exist.Equals(x.Message)) : Array.Exists(Params.codeResponseFalse, exist => exist.Equals(x.Message)))
            ));
        }
    }
}

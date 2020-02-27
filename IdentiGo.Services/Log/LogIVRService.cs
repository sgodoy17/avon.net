using Component.Transversal.Utilities;
using IdentiGo.Data.Repository;
using IdentiGo.Domain.Entity.Log;
using IdentiGo.Domain.Enums;
using IdentiGo.Services.Base;
using System;
using System.Collections.Generic;

namespace IdentiGo.Services.Log
{
    public interface ILogIVRService : IBasicAppService<LogIVR>
    {
        void Add(string codeValidation, string phoneAnswer, string message, bool input = true);

        IEnumerable<LogIVR> GetByRandeDate(DateTime start, DateTime end, TypeConsultCandidateResponse typeConsult);
    }

    public class LogIVRService : BasicAppService<LogIVR>, ILogIVRService
    {
        readonly IRepository<LogIVR> _repository;

        public LogIVRService(IRepository<LogIVR> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public void Add(string codeValidation, string phoneAnswer, string message, bool input = true)
        {
            var log = new LogIVR { CampId = codeValidation, PhoneAnswer = phoneAnswer, Message = message, Input = input };

            _repository.Add(log);

            _repository.UnitOfWork.Commit();
        }

        public IEnumerable<LogIVR> GetByRandeDate(DateTime start, DateTime end, TypeConsultCandidateResponse typeConsult)
        {

            return _repository.GetMany(x => x.Date >= start && x.Date <= end
            && (typeConsult == TypeConsultCandidateResponse.All ? Array.Exists(Params.codeResponse, exist => exist.Equals(x.Message)) :
                (typeConsult == TypeConsultCandidateResponse.Accept ? Array.Exists(Params.codeResponseTrue, exist => exist.Equals(x.Message)) : Array.Exists(Params.codeResponseFalse, exist => exist.Equals(x.Message)))
            ));
        }
    }
}

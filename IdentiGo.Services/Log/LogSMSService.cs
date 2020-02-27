using IdentiGo.Data;
using IdentiGo.Data.Repository;
using IdentiGo.Domain.Entity.Log;
using IdentiGo.Domain.Enums;
using IdentiGo.Services.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IdentiGo.Services.Log
{
    public interface ILogSMSService : IBasicAppService<LogSMS>
    {
        void Add(string codeValidation, string phoneAnswer, string message, string document, bool input = true);

        IEnumerable<LogSMS> GetByTypeRandeDate(DateTime? dateStart, DateTime? dateEnd, TypeConsultCandidateResponse typeConsult);

        IEnumerable<LogSMS> GetByDateRange(DateTime? dateStart, DateTime? dateEnd);

        int GetByPhoneNumber(string phone);
    }

    public class LogSMSService : BasicAppService<LogSMS>, ILogSMSService
    {
        readonly IRepository<LogSMS> _repository;

        public LogSMSService(IRepository<LogSMS> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public void Add(string codeValidation, string phoneAnswer, string message, string document, bool input = true)
        {
            var log = new LogSMS { CodeValidation = codeValidation, PhoneAnswer = phoneAnswer, Message = message, Document = document, Input = input };

            AddOrUpdate(log);
        }

        public IEnumerable<LogSMS> GetByTypeRandeDate(DateTime? dateStart, DateTime? dateEnd, TypeConsultCandidateResponse typeConsult)
        {
            //return _repository.GetMany(x => x.Date >= start && x.Date <= end 
            //&& (typeConsult == TypeConsultCandidateResponse.All ? Array.Exists(Params.codeResponse, exist => exist.Equals(x.CodeValidation)) :
            //    (typeConsult == TypeConsultCandidateResponse.Accept ? Array.Exists(Params.codeResponseTrue, exist => exist.Equals(x.CodeValidation)) : Array.Exists(Params.codeResponseFalse, exist => exist.Equals(x.CodeValidation)))
            //));

            if (TypeConsultCandidateResponse.Accept == typeConsult)
            {
                return MainContext.Create().LogSMS.Where(x => x.Input == true && (dateStart == null || DbFunctions.TruncateTime(x.Date) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.Date) <= dateEnd));
                //return _repository.GetMany(x => x.Input == true && (dateStart == null || x.Date.Date >= dateStart.Value) && (dateEnd == null || x.Date.Date <= dateEnd.Value));
            }
            else if(TypeConsultCandidateResponse.NoAccept == typeConsult)
            {
                return MainContext.Create().LogSMS.Where(x => x.Input == false && (dateStart == null || DbFunctions.TruncateTime(x.Date) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.Date) <= dateEnd));
                //return _repository.GetMany(x => x.Input == false && (dateStart == null || x.Date.Date >= dateStart.Value) && (dateEnd == null || x.Date.Date <= dateEnd.Value));
            }
            else
            {
                return MainContext.Create().LogSMS.Where(x => (dateStart == null || DbFunctions.TruncateTime(x.Date) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.Date) <= dateEnd));
                //return _repository.GetMany(x => (dateStart == null || x.Date.Date >= dateStart.Value) && (dateEnd == null || x.Date.Date <= dateEnd.Value));
            }            
        }

        public IEnumerable<LogSMS> GetByDateRange(DateTime? dateStart, DateTime? dateEnd)
        {
            return MainContext.Create().LogSMS.Where(x => (dateStart == null || DbFunctions.TruncateTime(x.Date) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.Date) <= dateEnd));
            //return _repository.GetMany(x => (dateStart == null || x.Date.Date >= dateStart.Value) && (dateEnd == null || x.Date.Date <= dateEnd.Value));
        }

        public int GetByPhoneNumber(string phone)
        {
            return MainContext.Create().LogSMS.Where(x => x.PhoneAnswer == phone && x.CodeValidation == "cx1" && DbFunctions.TruncateTime(x.Date) >= DateTime.Today.AddDays(-30) && DbFunctions.TruncateTime(x.Date) <= DateTime.Today).Count();
            //return _repository.GetMany(x => x.PhoneAnswer == phone && x.CodeValidation == "cx1" && x.Date.Date >= DateTime.Today.AddDays(-30) && x.Date.Date <= DateTime.Today).Count();
        }
    }
}

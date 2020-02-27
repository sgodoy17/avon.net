using IdentiGo.Data;
using IdentiGo.Data.Repository;
using IdentiGo.Domain.Entity.General;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Enums;
using IdentiGo.Services.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IdentiGo.Services.General
{
    public interface INominationService : IBasicAppService<Nomination>
    {
        List<Nomination> GetNoValid();

        Nomination GetNoValidByDocument(string document);

        Nomination GetById(Guid id);

        void GenerateValidation(ref Nomination user, Config config, bool blackList, List<string> pages, List<string> noPages);

        Nomination GetLastByDocument(string document);

        bool ValidateGenerateQuestion(Nomination userValidation);

        IEnumerable<Nomination> GetByDateRange(DateTime? dateInit = null, DateTime? dateEnd = null);

        Nomination GetLastByPhoneNumber(string phoneNumber);

        IEnumerable<Nomination> GetByTypeStateDateRange(String document, TypeState typeState, DateTime? dateStart, DateTime? dateEnd);

        IEnumerable<Nomination> GetByRangeDate(DateTime dateIni, DateTime dateEnd);

        IEnumerable<Nomination> GetByCampaingZoneTypeStateDateRange(String document, Guid? zoneId, Guid? campaingId, TypeState typeState, DateTime? dateStart, DateTime? dateEnd);

        int GenerateCodeVerificacion(string document);

        string GetUniqueKey();

        bool ValidatePhoneExist(string document, string phone);

        int GetCountValidByCodeUser(string code, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd);

        int GetCountInValidByCodeUser(string code, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd);

        int GetCountPendingByCodeUser(string code, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd);

        int GetCountTotalByCodeUser(string code, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd);

        int GetCountValidByZone(Guid? zoneId, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd);

        int GetCountInValidByZone(Guid? zoneId, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd);

        int GetCountPendingByZone(Guid? zoneId, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd);

        int GetCountTotalByZone(Guid? zoneId, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd);

        int GetCountValidByCodeUnit(Guid? unitId, Guid? campaingId, Guid zoneId, DateTime? dateStart, DateTime? dateEnd);

        int GetCountInValidByCodeUnit(Guid? unitId, Guid? campaingId, Guid zoneId, DateTime? dateStart, DateTime? dateEnd);

        int GetCountPendingByCodeUnit(Guid? unitId, Guid? campaingId, Guid zoneId, DateTime? dateStart, DateTime? dateEnd);

        int GetCountTotalByCodeUnit(Guid? unitId, Guid? campaingId, Guid zoneId, DateTime? dateStart, DateTime? dateEnd);

        int GetByPhoneNumber(string phone);

        bool ValidatePhoneNumber(string document, string phone);

        int GetLastDate();
    }

    public class NominationService : BasicAppService<Nomination>,
        INominationService
    {
        readonly IRepository<Nomination> _repository;

        public NominationService(IRepository<Nomination> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public Nomination UserValidation(Nomination item, int days)
        {
            item.DateCreated = DateTime.Now;
            item.DateUpdate = DateTime.Now;
            var userValidation = _repository.GetMany(x => x.Document == item.Document && item.DateUpdate.Date <= DateTime.Today.AddDays(-days)).FirstOrDefault();

            if (userValidation == null)
                return item;

            userValidation.DateUpdate = item.DateUpdate;
            _repository.Update(userValidation);

            return userValidation;
        }

        public List<Nomination> GetNoValid()
        {
            try
            {
                return _repository.GetMany(x => x.State != State.Success).OrderBy(x => x.DateCreated).ToList();
            }
            catch (Exception)
            {
                return new List<Nomination>();
            }
        }

        public Nomination GetNoValidByDocument(string document)
        {
            try
            {
                return _repository.GetMany(x => x.Document == document && x.State != State.Success).OrderBy(x => x.DateCreated).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Nomination GetById(Guid id)
        {
            return _repository.Get(id);
        }

        public Nomination GetLastByDocument(string document)
        {
            return MainContext.Create().UserValidation.Where(x => x.Document == document).OrderByDescending(x => x.DateUpdate).FirstOrDefault();
            //return _repository.GetManyNoTracking(x => x.Document == document).OrderByDescending(x => x.DateCreated).FirstOrDefault();
        }

        public void GenerateValidation(ref Nomination user, Config config, bool blackList, List<string> pages, List<string> noPages)
        {
            user.DateCreated = DateTime.Now;
            var currentUser = GetLastByDocument(user.Document);

            if (currentUser != null)
            {
                if (currentUser.State == State.Disabled)
                    throw new Exception("Este es un registro invalido");

                if (currentUser.State == State.Locked && currentUser.DateLastValidation.Date.AddDays(config.DayLokedDocument) > DateTime.Today)
                    throw new Exception("El usuario se encuentra bloquedo");

                if (currentUser.State == State.LockedDay && currentUser.DateLastValidation.Date == DateTime.Today)
                    throw new Exception("El usuario se encuentra bloquedo por hoy");

                if (!blackList && currentUser.State == State.Success && currentUser.DateLastValidation.Date.AddDays(config.TimeOutValidation) > DateTime.Today)
                    throw new Exception("Ya se realizo un proceso satisfactoriamente");

                if (currentUser.State != State.Success)
                {
                    user = currentUser;
                    user.NumberIntentByDay = currentUser.State == State.LockedDay ? 0 : currentUser.NumberIntentByDay;
                    user.NumberIntentByDay = currentUser.State == State.Locked ? 0 : currentUser.NumberIntentByDay;
                    user.State = State.Default;
                }
            }

            user.StageProcess = blackList ? StageProccess.ValidaAvon : StageProccess.Init;
            user.State = blackList ? State.Invalid : user.State;
            user.DateUpdate = DateTime.Now;
        }

        public bool ValidateGenerateQuestion(Nomination userValidation)
        {
            if (userValidation == null)
                return false;

            if (userValidation.State == State.Success || userValidation.State == State.Locked || userValidation.State == State.LockedDay)
                return false;

            return true;
        }

        public IEnumerable<Nomination> GetByDateRange(DateTime? dateInit = null, DateTime? dateEnd = null)
        {
            return _repository.GetManyInclude(x => (dateInit == null || x.DateCreated >= dateInit.Value) && (dateEnd == null || x.DateCreated <= dateEnd.Value));
        }

        public Nomination GetLastByPhoneNumber(string phoneNumber)
        {
            return MainContext.Create().UserValidation.Where(x => x.PhoneNumber == phoneNumber).OrderByDescending(x => x.DateUpdate).FirstOrDefault();
            //return _repository.GetMany(x => x.PhoneNumber == phoneNumber).OrderByDescending(x => x.DateUpdate).FirstOrDefault();
        }

        public IEnumerable<Nomination> GetByRangeDate(DateTime dateIni, DateTime dateEnd)
        {
            return MainContext.Create().UserValidation.Where(x => DbFunctions.TruncateTime(x.DateUpdate) >= dateIni && DbFunctions.TruncateTime(x.DateUpdate) <= dateEnd);
            //return _repository.GetMany(x => x.DateUpdate.Date >= dateIni && x.DateUpdate.Date <= dateEnd);
        }

        public IEnumerable<Nomination> GetByTypeStateDateRange(String document, TypeState typeState, DateTime? dateStart, DateTime? dateEnd)
        {
            State[] codes = new State[2];
            codes[0] = State.Success;
            codes[1] = State.Pass;            

            if (TypeState.Valid == typeState)
                return MainContext.Create().UserValidation.Where(x => (document == null || x.Document == document) && codes.Contains(x.State) && x.StageProcess == StageProccess.Finished && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd));
                //return _repository.GetMany(x => (document == null || x.Document == document) && x.State == State.Success && x.StageProcess == StageProccess.Finished && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value));

            if (TypeState.Invalid == typeState)
                return MainContext.Create().UserValidation.Where(x => (document == null || x.Document == document) && x.State == State.Invalid && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd));
                //return _repository.GetMany(x => (document == null || x.Document == document) && x.State == State.Invalid && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value));

            if (TypeState.Pending == typeState)
                return MainContext.Create().UserValidation.Where(x => (document == null || x.Document == document) && x.StageProcess != StageProccess.Finished && x.State != State.Invalid && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd));
                //return _repository.GetMany(x => (document == null || x.Document == document) && x.StageProcess != StageProccess.Finished && x.State != State.Invalid && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value));

            return MainContext.Create().UserValidation.Where(x => (document == null || x.Document == document) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd));
            //return _repository.GetMany(x => (document == null || x.Document == document) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value));
        }

        public IEnumerable<Nomination> GetByCampaingZoneTypeStateDateRange(String document, Guid? zoneId, Guid? campaingId, TypeState typeState, DateTime? dateStart, DateTime? dateEnd)
        {
            State[] codes = new State[2];
            codes[0] = State.Success;
            codes[1] = State.Pass;

            if (TypeState.Valid == typeState)
                return MainContext.Create().UserValidation.Where(x => (document == null || x.Document == document) && codes.Contains(x.State) && x.StageProcess == StageProccess.Finished && (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd));

            if (TypeState.Invalid == typeState)
                return MainContext.Create().UserValidation.Where(x => (document == null || x.Document == document) && x.State == State.Invalid && (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd));

            if (TypeState.Pending == typeState)
                return MainContext.Create().UserValidation.Where(x => (document == null || x.Document == document) && x.StageProcess != StageProccess.Finished && x.State != State.Invalid && (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd));

            return MainContext.Create().UserValidation.Where(x => (document == null || x.Document == document) && (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd));
        }

        //public int GenerateCodeVerificacion(string document)
        //{
        //    string codeVerificacion = (_repository.GetAll().OrderByDescending(x => x.CodeVerification).FirstOrDefault()?.CodeVerification ?? 100).ToString();
        //    string manteca = (MainContext.Create().UserValidation.OrderByDescending(x => x.CodeVerification).FirstOrDefault()?.CodeVerification ?? 100).ToString();
        //    string lastDigit = codeVerificacion.Substring(0, codeVerificacion.Length - 1);
        //    long add = Convert.ToInt64(lastDigit) + 1;
        //    long last = Convert.ToInt64(document) % (10);
        //    int result = int.Parse(add.ToString() + last.ToString());

        //    return result;
        //}

        public int GenerateCodeVerificacion(string document)
        {
            bool exits = true;
            int result;
            Random rng = new Random();

            do
            {
                const int maxValue = 999;
                //int value = rng.Next(100, 999);
                //string codeVerificacion = value.ToString("000");
                string number = rng.Next(100, maxValue + 1).ToString("D3");
                long last = Convert.ToInt64(document) % (10);
                result = int.Parse(number + last.ToString());

                exits = ValidateCodeVerification(result, document);
            }
            while (exits);

            return result;
        }

        public string GetUniqueKey()
        {
            int maxSize = 8;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);

            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }

            return result.ToString();
        }

        public bool ValidatePhoneExist(string document, string phone)
        {
            var currentUser = GetLastByPhoneNumber(phone);

            if (currentUser == null || currentUser.Document == document)
                return false;

            return true;
        }

        public int GetCountValidByCodeUser(string code, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd)
        {
            State[] codes = new State[2];
            codes[0] = State.Success;
            codes[1] = State.Pass;

            return MainContext.Create().UserValidation.Where(x => codes.Contains(x.State) && x.StageProcess == StageProccess.Finished && x.CodeUser == code && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).Count();
            //return _repository.GetMany(x => x.State == State.Success && x.StageProcess == StageProccess.Finished && x.CodeUser == code && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value)).Count();
        }

        public int GetCountInValidByCodeUser(string code, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd)
        {
            return MainContext.Create().UserValidation.Where(x => x.State == State.Invalid && x.CodeUser == code && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).Count();
            //return _repository.GetMany(x => x.State == State.Invalid && x.CodeUser == code && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value)).Count();
        }

        public int GetCountPendingByCodeUser(string code, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd)
        {
            return MainContext.Create().UserValidation.Where(x => x.StageProcess != StageProccess.Finished && x.State != State.Invalid && x.CodeUser == code && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).Count();
            //return _repository.GetMany(x => x.StageProcess != StageProccess.Finished && x.State != State.Invalid && x.CodeUser == code && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value)).Count();
        }

        public int GetCountTotalByCodeUser(string code, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd)
        {
            return MainContext.Create().UserValidation.Where(x => x.CodeUser == code && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).Count();
            //return _repository.GetMany(x => x.CodeUser == code && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value)).Count();
        }

        public int GetCountValidByZone(Guid? zoneId, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd)
        {
            State[] codes = new State[2];
            codes[0] = State.Success;
            codes[1] = State.Pass;

            //MainContext context = new MainContext();
            //IEnumerable<Nomination> tetero = context.UserValidation.Where(x => codes.Contains(x.State) && x.StageProcess == StageProccess.Finished && (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).ToList();            

            return MainContext.Create().UserValidation.Where(x => codes.Contains(x.State) && x.StageProcess == StageProccess.Finished && (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).Count();
            //return _repository.GetMany(x => x.State == State.Success && x.StageProcess == StageProccess.Finished && (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value)).Count();
        }

        public int GetCountInValidByZone(Guid? zoneId, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd)
        {
            return MainContext.Create().UserValidation.Where(x => x.State == State.Invalid && (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).Count();
            //return _repository.GetMany(x => x.State == State.Invalid && (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value)).Count();
        }

        public int GetCountPendingByZone(Guid? zoneId, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd)
        {
            return MainContext.Create().UserValidation.Where(x => x.StageProcess != StageProccess.Finished && x.State != State.Invalid && (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).Count();
            //return _repository.GetMany(x => x.StageProcess != StageProccess.Finished && x.State != State.Invalid && (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value)).Count();
        }

        public int GetCountTotalByZone(Guid? zoneId, Guid? campaingId, DateTime? dateStart, DateTime? dateEnd)
        {
            return MainContext.Create().UserValidation.Where(x => (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).Count();
            //return _repository.GetMany(x => (zoneId == null || x.ZoneId == zoneId) && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value)).Count();
        }

        public int GetCountValidByCodeUnit(Guid? unitId, Guid? campaingId, Guid zoneId, DateTime? dateStart, DateTime? dateEnd)
        {
            State[] codes = new State[2];
            codes[0] = State.Success;
            codes[1] = State.Pass;

            return MainContext.Create().UserValidation.Where(x => x.ZoneId == zoneId && codes.Contains(x.State) && x.StageProcess == StageProccess.Finished && x.UnitId == unitId && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).Count();
            //return _repository.GetMany(x => x.ZoneId == zoneId && x.State == State.Success && x.StageProcess == StageProccess.Finished && x.UnitId == unitId && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value)).Count();
        }

        public int GetCountInValidByCodeUnit(Guid? unitId, Guid? campaingId, Guid zoneId, DateTime? dateStart, DateTime? dateEnd)
        {
            return MainContext.Create().UserValidation.Where(x => x.ZoneId == zoneId && x.State == State.Invalid && x.UnitId == unitId && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).Count();
            //return _repository.GetMany(x => x.ZoneId == zoneId && x.State == State.Invalid && x.UnitId == unitId && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value)).Count();
        }

        public int GetCountPendingByCodeUnit(Guid? unitId, Guid? campaingId, Guid zoneId, DateTime? dateStart, DateTime? dateEnd)
        {
            return MainContext.Create().UserValidation.Where(x => x.ZoneId == zoneId && x.StageProcess != StageProccess.Finished && x.State != State.Invalid && x.UnitId == unitId && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).Count();
            //return _repository.GetMany(x => x.ZoneId == zoneId && x.StageProcess != StageProccess.Finished && x.State != State.Invalid && x.UnitId == unitId && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value)).Count();
        }

        public int GetCountTotalByCodeUnit(Guid? unitId, Guid? campaingId, Guid zoneId, DateTime? dateStart, DateTime? dateEnd)
        {
            return MainContext.Create().UserValidation.Where(x => x.ZoneId == zoneId && x.UnitId == unitId && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || DbFunctions.TruncateTime(x.DateCreated) >= dateStart) && (dateEnd == null || DbFunctions.TruncateTime(x.DateCreated) <= dateEnd)).Count();
            //return _repository.GetMany(x => x.ZoneId == zoneId && x.UnitId == unitId && (campaingId == null || x.CampaingId == campaingId) && (dateStart == null || x.DateCreated.Date >= dateStart.Value) && (dateEnd == null || x.DateCreated.Date <= dateEnd.Value)).Count();
        }

        public int GetByPhoneNumber(string phone)
        {
            DateTime beforeDate = DateTime.Today.AddDays(-30);
            DateTime afterDate = DateTime.Today;

            return MainContext.Create().UserValidation.Where(x => x.PhoneNumber == phone && DbFunctions.TruncateTime(x.DateCreated) >= beforeDate && DbFunctions.TruncateTime(x.DateCreated) <= afterDate).Count();
            //return _repository.GetMany(x => x.PhoneNumber == phone && x.DateCreated.Date >= DateTime.Today.AddDays(-30) && x.DateCreated.Date <= DateTime.Today).Count();
        }

        public bool ValidatePhoneNumber(string document, string phone)
        {
            var exits = MainContext.Create().UserValidation.Where(x => x.Document == document && x.PhoneNumber == phone).FirstOrDefault();
            //var exits = _repository.GetMany(x => x.Document == document && x.PhoneNumber == phone).FirstOrDefault();

            if (exits == null)
                return false;

            return true;
        }

        public bool ValidateCodeVerification(int codeVerification, string document)
        {
            var exits = MainContext.Create().UserValidation.Where(x => x.CodeVerification == codeVerification && x.Document == document).FirstOrDefault();
            //var exits = _repository.GetMany(x => x.Document == document && x.PhoneNumber == phone).FirstOrDefault();

            if (exits == null)
                return false;

            return true;
        }

        public int GetLastDate()
        {
            return _repository.GetAll().OrderBy(x => x.DateCreated).FirstOrDefault().DateCreated.Year;
        }
    }
}

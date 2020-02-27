using IdentiGo.Data.Repository;
using IdentiGo.Domain.Entity.Master;
using IdentiGo.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentiGo.Services.Master
{
    public interface ICampaingService : IBasicAppService<Campaing>
    {
        Campaing GetByNumber(string number);

        void SaveList(List<Campaing> list);
    }

    public class CampaingService : BasicAppService<Campaing>, ICampaingService
    {
        readonly IRepository<Campaing> _repository;

        public CampaingService(IRepository<Campaing> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public Campaing GetByNumber(string number)
        {
            return _repository.GetManyNoTracking(x => x.Number.Equals(number, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        public void SaveList(List<Campaing> list)
        {
            foreach (var campaingList in list)
            {
                if (string.IsNullOrEmpty(campaingList.Number) || _repository.GetMany(x => x.Number == campaingList.Number).Any())
                    continue;

                _repository.AddOrUpdate(campaingList);
                _repository.UnitOfWork.Commit();
            }
        }
    }
}

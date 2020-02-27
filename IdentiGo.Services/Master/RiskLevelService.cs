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
    public interface IRiskLevelService : IBasicAppService<RiskLevel>
    {
        bool AddQuota(Guid riskLevelId, IEnumerable<Quota> quota);

        bool RemoveQuota(Guid riskLevelId, IEnumerable<Quota> quota);

        RiskLevel GetByCategory(string name);
    }

    public class RiskLevelService : BasicAppService<RiskLevel>, IRiskLevelService
    {
        readonly IRepository<RiskLevel> _repository;

        public RiskLevelService(IRepository<RiskLevel> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public bool AddQuota(Guid riskLevelId, IEnumerable<Quota> quota)
        {
            RiskLevel riskLevel = _repository.Get(riskLevelId);
            try
            {
                quota.Except(riskLevel.Quota).ToList().ForEach(x => { riskLevel.Quota.Add(x); });

                _repository.Update(riskLevel);

                _repository.UnitOfWork.Commit();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveQuota(Guid riskLevelId, IEnumerable<Quota> quota)
        {
            try
            {
                var riskLevel = _repository.Get(riskLevelId);

                quota.ToList().ForEach(x => { riskLevel.Quota.Remove(x); });

                _repository.Update(riskLevel);

                _repository.UnitOfWork.Commit();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public RiskLevel GetByCategory(string name)
        {
            return _repository.GetMany(x => x.Description.Equals(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}

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
    public interface IQuotaService : IBasicAppService<Quota>
    {
        IEnumerable<Quota> GetById(Guid[] id);

        List<Quota> GetAllExceptName(string name);
    }

    public class QuotaService : BasicAppService<Quota>, IQuotaService
    {
        readonly IRepository<Quota> _repository;

        public QuotaService(IRepository<Quota> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<Quota> GetById(Guid[] id)
        {
            return _repository.GetMany(x => id.Any(y => y == x.Id));
        }

        public List<Quota> GetAllExceptName(string name)
        {
            return _repository.GetMany(x => !string.Equals(x.Name.ToUpper(), name.ToUpper(), StringComparison.CurrentCulture)).ToList();
        }
    }
}

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
    public interface ISecretaryTransitService : IBasicAppService<SecretaryTransit>
    {
        List<SecretaryTransit> GetAllExceptName(string name);
    }

    public class SecretaryTransitService : BasicAppService<SecretaryTransit>, ISecretaryTransitService
    {
        readonly IRepository<SecretaryTransit> _repository;

        public SecretaryTransitService(IRepository<SecretaryTransit> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public List<SecretaryTransit> GetAllExceptName(string name)
        {
            return _repository.GetMany(x => !string.Equals(x.Name.ToUpper(), name.ToUpper(), StringComparison.CurrentCulture)).ToList();
        }
    }
}

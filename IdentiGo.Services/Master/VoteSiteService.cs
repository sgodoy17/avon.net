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
    public interface IVoteSiteService : IBasicAppService<VoteSite>
    {
        List<VoteSiteDto> GetAllExceptName(string name);
    }

    public class VoteSiteService : BasicAppService<VoteSite>, IVoteSiteService
    {
        readonly IRepository<VoteSite> _repository;

        public VoteSiteService(IRepository<VoteSite> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public List<VoteSiteDto> GetAllExceptName(string name)
        {
            return TypeAdapter.Adapt<IEnumerable<VoteSite>, List<VoteSiteDto>>(_repository.GetMany(x => !string.Equals(x.Name.ToUpper(), name.ToUpper(), StringComparison.CurrentCulture)).ToList());
        }
    }
}

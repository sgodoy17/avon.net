using IdentiGo.Data.Repository;
using IdentiGo.Services.Base;
using IdentiGo.Domain.Entity.CIFIN;

namespace IdentiGo.Services.CIFIN
{
    public interface IProspectaService : IBasicAppService<Prospecta>
    {
    }

    public class ProspectaService : BasicAppService<Prospecta>, IProspectaService
    {
        readonly IRepository<Prospecta> _repository;

        public ProspectaService(IRepository<Prospecta> repository)
            : base(repository)
        {
            _repository = repository;
        }
    }
}

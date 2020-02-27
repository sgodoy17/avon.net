using IdentiGo.Data.Repository;
using IdentiGo.Services.Base;
using IdentiGo.Domain.Entity.CIFIN;

namespace IdentiGo.Services.CIFIN
{
    public interface IValidadorPlusService : IBasicAppService<ValidadorPlus>
    {
    }

    public class ValidadorPlusService : BasicAppService<ValidadorPlus>, IValidadorPlusService
    {
        readonly IRepository<ValidadorPlus> _repository;

        public ValidadorPlusService(IRepository<ValidadorPlus> repository)
            : base(repository)
        {
            _repository = repository;
        }
    }
}

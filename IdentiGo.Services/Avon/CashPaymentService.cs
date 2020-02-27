using IdentiGo.Data.Repository;
using IdentiGo.Domain.Entity.AVON;
using IdentiGo.Services.Base;

namespace IdentiGo.Services.Avon
{
    public interface ICashPaymentService : IBasicAppService<CashPayment>
    {
    }

    public class CashPaymentService : BasicAppService<CashPayment>, ICashPaymentService
    {
        readonly IRepository<CashPayment> _repository;

        public CashPaymentService(IRepository<CashPayment> repository)
            : base(repository)
        {
            _repository = repository;
        }
    }
}

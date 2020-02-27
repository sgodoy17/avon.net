using System.Collections.Generic;
using System.Linq;
using IdentiGo.Data.Repository;
using IdentiGo.Domain.Entity;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Services.Base;

namespace IdentiGo.Services.General
{
    public interface IBlackListService : IBasicAppService<BlackList>
    {
        void SaveList(List<BlackList> list);

        bool ExistByDocument(string document);

        BlackList GetByDocument(string document);
    }

    public class BlackListService : BasicAppService<BlackList>, IBlackListService
    {
        readonly IRepository<BlackList> _repository;

        public BlackListService(IRepository<BlackList> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public void SaveList(List<BlackList> list)
        {
            foreach (var blackList in list)
            {
                if(string.IsNullOrEmpty(blackList.Document) || _repository.GetMany(x => x.Document == blackList.Document).Any())
                    continue;
                _repository.AddOrUpdate(blackList);
                _repository.UnitOfWork.Commit();
            }
        }

        public bool ExistByDocument(string document)
        {
            return _repository.GetMany(x => x.Document == document).Any();
        }

        public BlackList GetByDocument(string document)
        {
            return _repository.GetMany(x => x.Document == document).FirstOrDefault();
        }
    }
}

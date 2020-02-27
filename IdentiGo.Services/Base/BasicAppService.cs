using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentiGo.Data.Repository;
using IdentiGo.Domain.Entity.Base;
using Component.Transversal.Adapters;

namespace IdentiGo.Services.Base
{
    
    public abstract class BasicAppService<TEntity>
        : IBasicAppService<TEntity>
        where TEntity : class, new()
    {
        readonly IRepository<TEntity> _repository;
        public ITypeAdapter TypeAdapter = TypeAdapterFactory.CreateAdapter();

        public BasicAppService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public TEntity Add(TEntity entity)
        {

            _repository.Add(entity);
            _repository.UnitOfWork.Commit();
            return entity;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public TEntity Get(object id)
        {
            return _repository.Get(id);
        }

        public TEntity GetReload(object id)
        {
            return _repository.GetReload(id);
        }

        public virtual TEntity Update(TEntity entity)
        {
            _repository.Update(entity);
            _repository.UnitOfWork.Commit();
            return entity;
        }

        public virtual TEntity AddOrUpdate(TEntity entity)
        {
            _repository.AddOrUpdate(entity);
            _repository.UnitOfWork.Commit();
            return entity;
        }

        public bool Exist(object id)
        {
            return _repository.Get(id) != null;
        }

        public void Delete(object id)
        {
            _repository.Delete(id);
            _repository.UnitOfWork.Commit();
        }

        public IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
        {
            return _repository.GetMany(where);
        }

        public IEnumerable<TEntity> GetManyNoTracking(Func<TEntity, bool> where)
        {
            return _repository.GetManyNoTracking(where);
        }

        public TEntity GetFirsOne(Func<TEntity, bool> where)
        {
            return _repository.GetFirsOne(where);
        }
    }
}

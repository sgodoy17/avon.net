using IdentiGo.Data.UnitOfWork;
using System;
using System.Collections.Generic;

namespace IdentiGo.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity obj);

        TEntity Get(object id);

        TEntity GetReload(object id);

        void Reload(TEntity entity);

        void Delete(TEntity entity);

        void Delete(Func<TEntity, Boolean> where);

        void Update(TEntity entity);

        void UpdateAll(TEntity entity);

        void AddOrUpdate(TEntity entity);

        IList<TEntity> GetAll();

        IList<TEntity> GetMany(Func<TEntity, bool> where);

        IList<TEntity> GetManyNoTracking(Func<TEntity, bool> where);

        IList<TEntity> GetManyInclude(Func<TEntity, bool> where, params string[] navigationProperties);

        TEntity GetFirsOne(Func<TEntity, bool> where);

        IUnitOfWork UnitOfWork { get; }

        void Delete(object id);
    }
}

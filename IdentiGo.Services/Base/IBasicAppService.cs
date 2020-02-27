using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IdentiGo.Services.Base
{
    public interface IBasicAppService<TEntity>
        where TEntity : class, new()
    {
        TEntity Add(TEntity item);

        IEnumerable<TEntity> GetAll();

        TEntity Get(object id);

        TEntity GetReload(object id);

        TEntity GetFirsOne(Func<TEntity, bool> where);

        TEntity Update(TEntity item);

        TEntity AddOrUpdate(TEntity item);

        bool Exist(object id);

        void Delete(object id);

        IEnumerable<TEntity> GetMany(Func<TEntity, bool> where);

        IEnumerable<TEntity> GetManyNoTracking(Func<TEntity, bool> where);
    }
}
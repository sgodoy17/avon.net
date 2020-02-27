using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Commit();

        IDbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class;

        void SetModified<TEntity>(TEntity entity)
            where TEntity : class;

        void Reload<TEntity>(TEntity entity)
            where TEntity : class;

        void Refresh();

        void Refresh(object entity);
    }
}

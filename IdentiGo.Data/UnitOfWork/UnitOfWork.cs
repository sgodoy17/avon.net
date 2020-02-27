using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory _databaseFactory;
        private MainContext _main;

        public UnitOfWork(IDbFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public void SetMainContext(MainContext context)
        {
            _main = context;
        }

        protected MainContext DataContext
        {
            get { return _main ?? (_main = _databaseFactory.Get()); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }

        public IDbSet<TEntity> CreateSet<TEntity>() where TEntity : class
        {
            return DataContext.Set<TEntity>();
        }

        public void SetModified<TEntity>(TEntity entity) where TEntity : class
        {
            DataContext.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public void Reload<TEntity>(TEntity entity) where TEntity : class
        {
            DataContext.Entry<TEntity>(entity).Reload();
        }

        public void Refresh()
        {
            var ctx = ((IObjectContextAdapter)this).ObjectContext;

            // Get all objects in statemanager with entityKey 
            // (context.Refresh will throw an exception otherwise) 
            var objects = (from entry in ctx.ObjectStateManager.GetObjectStateEntries(
                                                       EntityState.Added
                                                      | EntityState.Deleted
                                                      | EntityState.Modified
                                                      | EntityState.Unchanged)
                           where entry.EntityKey != null
                           select entry.Entity);

            ctx.Refresh(RefreshMode.StoreWins, objects);
        }

        public void Refresh(object entity)
        {
            var ctx = ((IObjectContextAdapter)this).ObjectContext;
            ctx.Refresh(RefreshMode.StoreWins, entity);
        }
    }
}

using IdentiGo.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IDbSet<TEntity> _entity;
        private readonly IUnitOfWork _currentUnitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _currentUnitOfWork = unitOfWork;

            _entity = GetSet();
        }

        protected IDbFactory DatabaseFactory
        {
            get;
            private set;
        }

        public TEntity Get(object id)
        {
            return _entity.Find(id);
        }

        public TEntity GetReload(object id)
        {
            var entity = _entity.Find(id);
            _currentUnitOfWork.Reload(entity);
            return entity;
        }

        public void Reload(TEntity entity)
        {
            if (entity != null)
                _currentUnitOfWork.Reload(entity);
        }

        public virtual IList<TEntity> GetAll()
        {
            return _entity.ToList();
        }

        public TEntity Add(TEntity obj)
        {
            return _entity.Add(obj);
        }

        public virtual void Update(TEntity entity)
        {
            _entity.AddOrUpdate(entity);
        }

        public virtual void AddOrUpdate(TEntity entity)
        {
            _entity.AddOrUpdate(entity);
        }

        public virtual void UpdateAll(TEntity entity)
        {
            _currentUnitOfWork.SetModified<TEntity>(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _entity.Remove(entity);
        }
        public void Delete(Func<TEntity, Boolean> where)
        {
            IEnumerable<TEntity> objects = _entity.Where(where).AsEnumerable();
            foreach (TEntity obj in objects)
                _entity.Remove(obj);
        }

        public void Delete(object id)
        {
            _entity.Remove(Get(id));
        }

        public virtual IList<TEntity> GetMany(Func<TEntity, bool> where)
        {
            return _entity.Where(where).ToList();
        }

        public virtual IList<TEntity> GetManyNoTracking(Func<TEntity, bool> where)
        {
            return _entity.AsNoTracking().Where(where).ToList();
        }

        public IList<TEntity> GetManyInclude(Func<TEntity, bool> predicate, params string[] navigationProperties)
        {
            var query = _entity.AsQueryable();
            foreach (string navigationProperty in navigationProperties)
                query = query.Include(navigationProperty);
            return query.Where(predicate).ToList();
        }

        public virtual TEntity GetFirsOne(Func<TEntity, bool> where)
        {
            return _entity.Where(where).FirstOrDefault();
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _currentUnitOfWork;
            }
        }

        public IDbSet<TEntity> GetSet()
        {
            if (_currentUnitOfWork != null)
            {
                IDbSet<TEntity> set = _currentUnitOfWork.CreateSet<TEntity>();

                return set;
            }
            else
                return null;
        }

        public void Refresh(object entry)
        {
            _currentUnitOfWork.Refresh(entry);
        }
    }
}

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
    
    public abstract class BasicAppServiceDto<TEntity, TDto, TDtoList>
        : IBasicAppServiceDto<TDto, TDtoList>
        where TEntity : class, new()
        where TDto : class, new()
        where TDtoList : class, new()
    {
        readonly IRepository<TEntity> _repository;
        public ITypeAdapter TypeAdapter = TypeAdapterFactory.CreateAdapter();

        public BasicAppServiceDto(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public TDto Add(TDto item)
        {

            TEntity entity = TypeAdapter.Adapt<TDto, TEntity>(item);
            var ret = _repository.Add(entity);
            _repository.UnitOfWork.Commit();
            return TypeAdapter.Adapt<TEntity, TDto>(ret);
        }

        public IEnumerable<TDtoList> GetAll()
        {
            return TypeAdapter.Adapt<IEnumerable<TEntity>, List<TDtoList>>(_repository.GetAll());
        }

        public TDto Get(object id)
        {
            return TypeAdapter.Adapt<TEntity, TDto>(_repository.Get(id));
        }

        public virtual TDto Update(TDto item)
        {
            TEntity entity = TypeAdapter.Adapt<TDto, TEntity>(item);
            _repository.Update(entity);
            _repository.UnitOfWork.Commit();
            return TypeAdapter.Adapt<TEntity, TDto>(entity);
        }

        public virtual TDto AddOrUpdate(TDto item)
        {
            TEntity entity = TypeAdapter.Adapt<TDto, TEntity>(item);
            _repository.AddOrUpdate(entity);
            _repository.UnitOfWork.Commit();
            return TypeAdapter.Adapt<TEntity, TDto>(entity);
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


    }
}

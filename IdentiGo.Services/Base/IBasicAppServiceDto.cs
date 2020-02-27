using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IdentiGo.Services.Base
{
    public interface IBasicAppServiceDto<TDto, out TDtoList>
        where TDto : class, new()
        where TDtoList : class, new()
    {
        TDto Add(TDto item);

        IEnumerable<TDtoList> GetAll();

        TDto Get(object id);

        TDto Update(TDto item);

        TDto AddOrUpdate(TDto item);

        bool Exist(object id);

        void Delete(object id);
    }
}
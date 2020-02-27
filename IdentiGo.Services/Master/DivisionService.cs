using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IdentiGo.Data.Repository;
using IdentiGo.Domain.DTO.Master;
using IdentiGo.Domain.Entity.Master;
using IdentiGo.Services.Base;

namespace IdentiGo.Services.Master
{
    public interface IDivisionService : IBasicAppService<Division>
    {
        Division GetByNumber(string code);
    }

    public class DivisionService : BasicAppService<Division>, IDivisionService
    {
        readonly IRepository<Division> _repository;

        public DivisionService(IRepository<Division> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public Division GetByNumber(string code)
        {
            return _repository.GetManyNoTracking(x => x.Number == code).FirstOrDefault();
        }

    }
}

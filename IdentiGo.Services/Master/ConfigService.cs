using System;
using System.Collections.Generic;
using System.Linq;
using Component.Transversal.Utilities;
using IdentiGo.Data.Repository;
using IdentiGo.Domain.DTO;
using IdentiGo.Domain.Entity;
using IdentiGo.Domain.Entity.General;
using IdentiGo.Services.Base;

namespace IdentiGo.Services.Master
{
    public interface IConfigService : IBasicAppService<Config>
    {
    }

    public class ConfigService : BasicAppService<Config>, IConfigService
    {
        private readonly IRepository<Config> _repository;

        public ConfigService(IRepository<Config> repository)
            : base(repository)
        {
            _repository = repository;
        }
    }
}

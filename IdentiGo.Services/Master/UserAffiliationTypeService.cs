using IdentiGo.Data.Repository;
using IdentiGo.Domain.Security;
using IdentiGo.Services.Base;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component.Transversal.WebApi;
using Component.Transversal.Extensions;
using System.Text.RegularExpressions;
using IdentiGo.Domain.Entity;
using System.Security.Claims;
using Component.Transversal.Utilities;
using IdentiGo.Domain.DTO;
using IdentiGo.Domain.Entity.Master;
using IdentiGo.Domain.DTO.Master;

namespace IdentiGo.Services.General
{
    public interface IUserAffiliationService : IBasicAppService<UserAffiliation>
    {
        List<UserAffiliation> GetByAffiliationTypeId(Guid affiliationTypeId);
        List<UserAffiliation> GetByAffiliationTypeIdExceptName(Guid affiliationTypeId, string name);
    }

    public class UserAffiliationService : BasicAppService<UserAffiliation>, IUserAffiliationService
    {
        readonly IRepository<UserAffiliation> repository;

        public UserAffiliationService(IRepository<UserAffiliation> repository)
            : base(repository)
        {
            this.repository = repository;
        }

        public List<UserAffiliation> GetByAffiliationTypeId(Guid affiliationTypeId) 
        {
            return repository.GetMany(x => x.AffiliationTypeId == affiliationTypeId).ToList();
        }

        public List<UserAffiliation> GetByAffiliationTypeIdExceptName(Guid affiliationTypeId, string name)
        {
            return repository.GetMany(x => x.AffiliationTypeId == affiliationTypeId && !string.Equals(x.Name.ToUpper(), name.ToUpper(), StringComparison.CurrentCulture)).ToList();
        }
    }
}

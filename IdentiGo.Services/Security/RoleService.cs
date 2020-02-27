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
using PasswordHasher = Component.Transversal.Cryptography.PasswordHasher;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Helpers;
using IdentiGo.Domain.Enums;

namespace IdentiGo.Services.Security
{
    public interface IRoleService : IBasicAppService<Role>
    {
        List<Role> GetByName(string[] name);

        IEnumerable<Role> GetById(string[] id);

        List<Role> GetByTypeRole(TypeRole typeRole, bool admin = false);

        IEnumerable<Role> GetByGroupRoleId(Guid id);
    }

    public class RoleService : BasicAppService<Role>, IRoleService
    {
        readonly IRepository<Role> repository;

        public RoleService(IRepository<Role> repository)
            : base(repository)
        {
            this.repository = repository;
        }

        public List<Role> GetByName(string[] name)
        {
            return repository.GetMany(x => name.Any(y => y == x.Name)).ToList();
        }

        public IEnumerable<Role> GetById(string[] id)
        {
            return repository.GetMany(x => id.Any(y => y == x.Id.ToString()));
        }

        public List<Role> GetByTypeRole(TypeRole typeRole, bool admin = false) 
        {
            return repository.GetMany(x => x.TypeRole == typeRole && (admin || x.Name != RoleName.Role1)).ToList();
        }

        public IEnumerable<Role> GetByGroupRoleId(Guid id)
        {
            return repository.GetMany(x => x.GroupRole.Any(group => group.Id == id));
        }
    }
}

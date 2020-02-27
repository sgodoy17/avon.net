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
    public interface IGroupRoleService : IBasicAppService<GroupRole>
    {
        List<GroupRole> GetByDisplayName(string[] displayName);

        IEnumerable<GroupRole> GetByName(string[] name);

        bool AddRoles(Guid id, IEnumerable<Role> roles);

        bool RemoveRoles(Guid id, IEnumerable<Role> roles);

        IEnumerable<Guid> GetIdByUserId(Guid userId);

        IEnumerable<GroupRole> GetById(string[] id);
    }

    public class GroupRoleService : BasicAppService<GroupRole>, IGroupRoleService
    {
        readonly IRepository<GroupRole> repository;

        public GroupRoleService(IRepository<GroupRole> repository)
            : base(repository)
        {
            this.repository = repository;
        }

        public List<GroupRole> GetByDisplayName(string[] displayName)
        {
            return repository.GetMany(x => displayName.Any(y => y == x.Name)).ToList();
        }

        public IEnumerable<GroupRole> GetByName(string[] name)
        {
            return repository.GetMany(x => name.Any(y => y == x.Name));
        }

        public bool AddRoles(Guid id, IEnumerable<Role> roles)
        {
            GroupRole groupRole = repository.Get(id);
            try
            {
                roles.Except(groupRole.Role).ToList().ForEach(x => { groupRole.Role.Add(x); });
                repository.Update(groupRole);
                repository.UnitOfWork.Commit();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveRoles(Guid id, IEnumerable<Role> roles)
        {
            try
            {
                var groupRole = repository.Get(id);

                roles.ToList().ForEach(x => { groupRole.Role.Remove(x); });

                repository.Update(groupRole);

                repository.UnitOfWork.Commit();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Guid> GetIdByUserId(Guid userId)
        {
            return repository.GetMany(x => x.User.Any(user => user.Id == userId)).Select(x => x.Id);
        }
        public IEnumerable<GroupRole> GetById(string[] id)
        {
            return repository.GetMany(x => id.Any(y => y == x.Id.ToString()));
        }
    }
}

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
using IdentiGo.Domain.DTO;
using Component.Transversal.Utilities;

namespace IdentiGo.Services.Security
{
    public interface IUserService : IBasicAppService<User>
    {
        IdentityResult CreateUser(User user, string newPassword);

        IdentityResult EditUser(User userDto);

        SignInStatus ValidatePassword(string user, string password);

        User GetByUserName(string userName);

        bool AddRoles(Guid userId, IEnumerable<Role> roles);

        bool RemoveRoles(Guid userId, IEnumerable<Role> roles, User currentUser = null);

        bool AddRoles(IEnumerable<Role> roles);

        bool RemoveRoles(IEnumerable<Role> roles);

        User GetByCompanyIdDocument(string document);

        bool AddRolesByGroupId(Guid id, IEnumerable<Role> enumerable);

        bool RemoveRolesByGroupId(Guid id, IEnumerable<Role> enumerable);

        bool AddGroupRoles(Guid userId, IEnumerable<GroupRole> groupRoles);

        bool RemoveGroupRoles(Guid userId, IEnumerable<GroupRole> groupRoles);

        string ValidChangePassword(string password);

        void LoadList(List<User> list);
    }

    public class UserService : BasicAppService<User>, IUserService
    {
        readonly IRepository<User> repository;

        public UserService(IRepository<User> repository)
            : base(repository)
        {
            this.repository = repository;
        }

        public IdentityResult CreateUser(User user, string newPassword)
        {
            try
            {
                user.UserName = user.Email;
                List<string> error = new List<string>();
                if (user == null) return IdentityResult.Failed("datos de el usario invalidos.");

                if (user.Birthdate.AddYears(18) > DateTime.Today) error.Add("El usuario debe ser mayor de 18.");

                if (user.Birthdate.Date > DateTime.Today) error.Add(string.Format("La {0} no puede ser mayor a la fecha actual.", ReflectionExtensions.GetPropertyDisplayName<User>(i => i.Birthdate)));

                var errorPasswrd = ValidChangePassword(user.Password);
                if (!string.IsNullOrEmpty(errorPasswrd))
                    error.Add(errorPasswrd);

                if (repository.GetMany(x => x.UserName == user.UserName).Any()) error.Add(string.Format("El {0} {1} ya existe.", ReflectionExtensions.GetPropertyDisplayName<User>(i => i.UserName), user.UserName));

                if (repository.GetMany(x => x.UserName == user.Email).Any()) error.Add(string.Format("El {0} {1} ya existe.", ReflectionExtensions.GetPropertyDisplayName<User>(i => i.Email), user.Email));

                if (repository.GetMany(x => x.IdType == user.IdType && x.IdNumber == user.IdNumber).Any()) error.Add(string.Format("El {0} {1} ya existe.", ReflectionExtensions.GetPropertyDisplayName<User>(i => i.IdNumber), user.IdNumber));

                if (error.Any())
                    return IdentityResult.Failed(error.ToArray());

                user.SecurityStamp = Guid.NewGuid().ToString();
                user.PasswordHash = Crypto.HashPassword(newPassword);
                user.ActivationDate = DateTime.Now;
                user.LastUpdate = DateTime.Now;

                repository.AddOrUpdate(user);
                repository.UnitOfWork.Commit();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(ex.Message);
            }
            // Save the user and return the outcome.
        }

        public string ValidChangePassword(string password)
        {
            //if (!Regex.Match(string.IsNullOrWhiteSpace(password) ? "Invalid" : password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,20}$").Success)
                //return "Las contraseñas deben tener al menos un carácter que sea un dígito, una letra minúscula, una letra mayúscula, uno que no sea una letra ni un dígito y de 6 a 20 caracteres.";
            if (!Regex.Match(string.IsNullOrWhiteSpace(password) ? "Invalid" : password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,20}$").Success)
                return "Las contraseñas deben tener al menos un carácter que sea un dígito, una letra minúscula, una letra mayúscula y de 6 a 20 caracteres.";
            return "";
        }

        public bool RemoveRolesByGroupId(Guid groupId, IEnumerable<Role> roles)
        {
            try
            {
                var list = repository.GetMany(x => x.GroupRole.Any(y => y.Id == groupId));
                foreach (var user in list)
                {
                    var rolesRemove = roles.Where(role => !role.GroupRole.Any(x => x.Id != groupId));

                    RemoveRoles(user.Id, rolesRemove, user);
                }
                return true;
            }
            catch { return false; }
        }

        public bool AddRolesByGroupId(Guid id, IEnumerable<Role> roles)
        {
            try
            {
                bool result = true;
                var list = repository.GetMany(x => x.GroupRole.Any(y => y.Id == id));
                foreach (var user in list)
                {
                    result = AddRoles(user.Id, roles);
                    if (!result) break;
                }
                return result;
            }
            catch { return false; }
        }

        public IdentityResult EditUser(User user)
        {
            try
            {
                List<string> error = new List<string>();

                if (user == null) return IdentityResult.Failed("datos de el usario invalidos.");
                if (user.Birthdate.AddYears(18) > DateTime.Today) error.Add("El usuario debe ser mayor de 18.");
                if (user.Birthdate.Date > DateTime.Today) error.Add(string.Format("La {0} no puede ser mayor a la fecha actual.", ReflectionExtensions.GetPropertyDisplayName<User>(i => i.Birthdate)));
                if (repository.GetMany(x => x.Id != user.Id && x.UserName == user.Email).Any()) error.Add(string.Format("El {0} {1} ya existe.", ReflectionExtensions.GetPropertyDisplayName<User>(i => i.Email), user.Email));
                if (repository.GetMany(x => x.Id != user.Id && x.IdType == user.IdType && x.IdNumber == user.IdNumber).Any()) error.Add(string.Format("El {0} {1} ya existe.", ReflectionExtensions.GetPropertyDisplayName<User>(i => i.IdNumber), user.IdNumber));

                if (error.Any())
                    return IdentityResult.Failed(error.ToArray());

                User currentUser = repository.Get(user.Id);
                currentUser.UpdateFromUser(user);
                currentUser.UserName = user.Email;
                currentUser.LastUpdate = DateTime.Now;
                repository.AddOrUpdate(currentUser);
                repository.UnitOfWork.Commit();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(ex.Message);
            }
            // Save the user and return the outcome.
        }

        public SignInStatus ValidatePassword(string user, string password)
        {
            if (repository.GetMany(x => x.UserName == user && Crypto.VerifyHashedPassword(x.PasswordHash, password)).Any())
                return SignInStatus.Success;
            return SignInStatus.Failure;
        }

        public User GetByUserName(string userName)
        {
            return repository.GetMany(x => x.UserName == userName).FirstOrDefault();
        }

        public bool AddRoles(Guid userId, IEnumerable<Role> roles)
        {
            try
            {
                User user = repository.Get(userId);
                roles.Except(user.Roles.Select(x => x.Role)).ToList().ForEach(x =>
                {
                    user.Roles.Add(new UserRole { UserId = user.Id, RoleId = x.Id });
                });
                repository.Update(user);
                repository.UnitOfWork.Commit();
                return true;
            }
            catch { return false; }
        }

        public bool RemoveRoles(Guid userId, IEnumerable<Role> roles, User currentUser = null)
        {
            try
            {
                User user = currentUser ?? repository.Get(userId);
                roles.ToList().ForEach(x =>
                {
                    var removeItem = user.Roles.Where(y => y.RoleId == x.Id).FirstOrDefault();
                    user.Roles.Remove(removeItem);
                });
                repository.Update(user);
                repository.UnitOfWork.Commit();
                return true;
            }
            catch { return false; }
        }

        public bool AddRoles(IEnumerable<Role> roles)
        {
            bool result = true;
            var User = repository.GetAll();
            foreach (var user in User)
            {
                result = AddRoles(user.Id, roles);
                if (!result) break;
            }
            return result;
        }

        public bool RemoveRoles(IEnumerable<Role> roles)
        {
            bool result = true;
            var User = repository.GetAll();
            foreach (var user in User)
            {
                result = RemoveRoles(user.Id, roles);
                if (!result) break;
            }
            return result;
        }

        public User GetByCompanyIdDocument(string document)
        {
            return repository.GetMany(x => x.IdNumber == document).FirstOrDefault();
        }

        public bool AddGroupRoles(Guid userId, IEnumerable<GroupRole> groupRoles)
        {
            try
            {
                User user = repository.Get(userId);
                groupRoles.Except(user.GroupRole).ToList().ForEach(x =>
                {
                    user.GroupRole.Add(x);
                });

                repository.Update(user);

                repository.UnitOfWork.Commit();

                AddRoles(user.Id, groupRoles.SelectMany(x => x.Role).Distinct());

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveGroupRoles(Guid userId, IEnumerable<GroupRole> groupRoles)
        {
            try
            {
                User user = repository.GetReload(userId);
                groupRoles.ToList().ForEach(groupRole =>
                {
                    user.GroupRole.Remove(groupRole);
                    var roles = user.GroupRole.SelectMany(x => x.Role);
                    var rolesRemove = groupRole.Role.Where(groupRoleRole =>  !roles.Any(x => x.Id == groupRoleRole.Id));
                    RemoveRoles(groupRole.Id, rolesRemove, currentUser: user);
                    repository.UnitOfWork.Commit();
                });

                return true;
            }
            catch { return false; }

        }

        public void LoadList(List<User> list)
        {
            foreach (var user in list)
            {
                user.Id = repository.GetFirsOne(x => x.UserName == user.UserName)?.Id ?? user.Id;
                EditUser(user);
            }
        }
    }
}

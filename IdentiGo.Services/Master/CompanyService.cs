using IdentiGo.Data.Repository;
using IdentiGo.Domain.Security;
using IdentiGo.Services.Base;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using IdentiGo.Domain.Entity.General;

namespace IdentiGo.Services.General
{
    public interface ICompanyService : IBasicAppService<Company>
    {
        bool AddRoles(int companyId, IEnumerable<Role> roles);

        bool RemoveRoles(int companyId, IEnumerable<Role> roles);

        IEnumerable<Claim> CreateClaims(int? companyId);

        Company UpdateManual(Company company);

        Company GetByPublicKey(Guid publicKey);
    }

    public class CompanyService : BasicAppService<Company>, ICompanyService
    {
        private readonly IRepository<Company> _repository;

        public CompanyService(IRepository<Company> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public bool AddRoles(int companyId, IEnumerable<Role> roles)
        {
            Company company = _repository.Get(companyId);
            try
            {
                roles.Except(company.Role).ToList().ForEach(x => { company.Role.Add(x); });
                _repository.Update(company);
                _repository.UnitOfWork.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveRoles(int companyId, IEnumerable<Role> roles)
        {
            try
            {
                var company = _repository.Get(companyId);

                roles.ToList().ForEach(x => { company.Role.Remove(x); });

                _repository.Update(company);

                _repository.UnitOfWork.Commit();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Claim> CreateClaims(int? companyId)
        {
            var enterpriseClaims = new List<Claim>();

            var company = Get(companyId ?? 0);

            if (company != null)
            {
                company.GetType().GetProperties().ToList().ForEach(prop =>
                {
                    enterpriseClaims.Add(new Claim(string.Format(CustomClaimTypes.EnterpriseFieldFormat, prop.Name),
                    (prop.GetMethod != null && prop.GetValue(company) != null
                        ? prop.GetValue(company).ToString()
                        : string.Empty)));
                });
            }

            return enterpriseClaims;
        }

        public Company UpdateManual(Company item)
        {
            var company = _repository.Get(item.Id);

            company.Color = item.Color;
            company.Image = item.Image;
            company.Name = item.Name;
            company.Nit = item.Nit;

            _repository.Update(company);
            _repository.UnitOfWork.Commit();
            return company;
        }

        public Company GetByPublicKey(Guid publicKey)
        {
            try
            {
                var obj = _repository.GetMany(x => x.PublicKey == publicKey).FirstOrDefault();

                var adapted = obj;

                return adapted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }

        }

    }
}

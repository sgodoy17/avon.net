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
    public interface IDepartmentService : IBasicAppService<Department>
    {
        List<Department> GetAllExceptName(string name);
    }

    public class DepartmentService : BasicAppService<Department>, IDepartmentService
    {
        readonly IRepository<Department> repository;

        public DepartmentService(IRepository<Department> repository)
            : base(repository)
        {
            this.repository = repository;
        }

        public List<Department> GetAllExceptName(string name)
        {
            return repository.GetMany(x => !string.Equals(x.Name.ToUpper(), name.ToUpper(), StringComparison.CurrentCulture)).ToList();
        }
    }
}

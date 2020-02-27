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
    public interface IAffiliationTypeService : IBasicAppService<AffiliationType>
    {
    }

    public class AffiliationTypeService : BasicAppService<AffiliationType>, IAffiliationTypeService
    {
        readonly IRepository<AffiliationType> repository;

        public AffiliationTypeService(IRepository<AffiliationType> repository)
            : base(repository)
        {
            this.repository = repository;
        }

        public List<string> Address(string address, int options) 
        {
            List<string> addressList = new List<string>();
            string pattern = @"\d*";
            for (int i = 0; i < options; i++) 
            {
                addressList.Add(Regex.Replace(address, pattern, new Random().Next(0, 9).ToString()));
            }
            return addressList;
        }
    }
}

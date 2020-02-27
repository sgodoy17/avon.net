using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentiGo.WebApi.Models
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        public List<string> Pages { get; set; }
    }
}
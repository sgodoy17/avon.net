using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Transversal.WebApi.ViewModel
{
    public class AuthViewModel
    {
        public string ClientId { get; set; }

        public string GrantType { get; set; }

        public string ClientSecret { get; set; }
    }
}

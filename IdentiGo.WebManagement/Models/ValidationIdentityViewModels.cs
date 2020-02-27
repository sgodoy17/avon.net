using Component.Transversal.WebApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentiGo.WebManagement.Models
{
    public class ValidationIdentityViewModels
    {
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Display(Name = "Tipo de Document")]
        public IdentificationType IdType { get; set; }

        [Display(Name = "N° Document")]
        [MaxLength(12)]
        public string IdNumber { get; set; }

        public bool Valid { get; set; }
    }
}
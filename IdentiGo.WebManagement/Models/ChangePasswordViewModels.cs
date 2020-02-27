using Component.Transversal.WebApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentiGo.WebManagement.Models
{
    public class ChangePasswordViewModels
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Contraseña Anterior")]
        public string OldPassword { get; set; }
        
        public bool RememberMe { get; set; }

        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public virtual string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public virtual string ConfirmPassword { get; set; }
    }
}
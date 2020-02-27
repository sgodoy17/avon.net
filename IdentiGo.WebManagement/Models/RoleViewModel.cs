using IdentiGo.Domain.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentiGo.WebManagement.Models
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Tipo de Rol")]
        [Required(ErrorMessage = "El Campo Tipo de Rol es obligatorio")]
        public TypeRole TypeRole { get; set; }
    }

    public class EditUserViewModel
    {
        public Guid Id { get; set; }

        public int? CompanyId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
        public IEnumerable<SelectListItem> ModuleList { get; set; }
    }

    public class EditCompanyViewModel
    {
        public int Id { get; set; }

        [Display(Name = "NIT")]
        [Required(ErrorMessage = "El campo NIT es obligatorio")]
        public string Nit { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Name { get; set; }

        [Display(Name = "Logo")]
        [Required(ErrorMessage = "El campo Logo es obligatorio")]
        public string Image { get; set; }
        
        public string TitleImage { get; set; }

        [Display(Name = "Color")]
        [Required(ErrorMessage = "El campo Color es obligatorio" )]
        public string Color { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}
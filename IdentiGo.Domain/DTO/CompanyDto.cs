using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentiGo.Domain.Security;
using System.Web.Mvc;
using System.Web;
namespace IdentiGo.Domain.DTO
{
    public class CompanyDto
    {
        public int Id { get; set; }

        [Display(Name = "NIT")]
        [Required(ErrorMessage = "El campo NIT es obligatorio")]
        public string Nit { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }

        [Display(Name = "Color")]
        public string Color { get; set; }

        public Guid PublicKey { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }

        public HttpPostedFileBase File { get; set; }

        public virtual ICollection<RoleDto> Role { get; set; }
    }
}

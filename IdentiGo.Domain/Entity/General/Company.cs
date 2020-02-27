using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentiGo.Domain.Security;

namespace IdentiGo.Domain.Entity.General
{
    [Table("COMPANY")]
    public class Company
    {
        public Company() 
        {
            Role = new HashSet<Role>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
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

        public virtual ICollection<Role> Role { get; set; }

        [NotMapped]
        public IEnumerable<System.Web.Mvc.SelectListItem> RolesList { get; set; }
    }
}

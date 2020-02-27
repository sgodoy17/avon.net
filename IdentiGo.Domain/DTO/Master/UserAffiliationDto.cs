using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentiGo.Domain.Security;
using IdentiGo.Domain.Enums;
namespace IdentiGo.Domain.DTO.Master
{
    public class UserAffiliationDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Name { get; set; }

        [Display(Name = "Otros Nombres")]
        public string OtherName { get; set; }

        [Required(ErrorMessage = "El campo Tipo de Afiliación es obligatorio")]
        [Display(Name = "Tipo de Afiliación")]
        public Guid AffiliationTypeId { get; set; }
        public virtual AffiliationTypeDto AffiliationType { get; set; }
    }
}

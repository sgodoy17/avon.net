using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IdentiGo.Domain.Entity.Master;

namespace IdentiGo.Domain.DTO.Master
{
    public class CountryDto
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [StringLength(10)]
        [Display(Name = "ID Alternativo (Otros sistemas)")]
        public string IdAlt { get; set; }

        public virtual ICollection<DepartmentDto> Department { get; set; }


    }
}

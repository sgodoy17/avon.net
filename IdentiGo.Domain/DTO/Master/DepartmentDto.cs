using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentiGo.Domain.Entity.Master;

namespace IdentiGo.Domain.DTO.Master
{
    [Table("DEPARTMENT")]
    public class DepartmentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo País es obligatorio")]
        [Display(Name = "País")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "El campo Nombre es un campo obligatorio")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [StringLength(10)]
        [Display(Name = "ID Alternativo (Otros sistemas)")]
        public string IdAlt { get; set; }

        public virtual ICollection<CityDto> City { get; set; }
    }
}

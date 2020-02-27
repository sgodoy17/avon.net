using System.ComponentModel.DataAnnotations;

namespace IdentiGo.Domain.DTO.Master
{
    public class CityDto
    {
        public int Id { get; set; }

        public int CountryId { get; set; }
        public virtual CountryDto Country{ get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "El campo Department es obligatorio")]
        public int DepartmentId { get; set; }
        public virtual DepartmentDto Department { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [StringLength(10)]
        [Display(Name = "ID Alternativo (Otros sistemas)")]
        public string IdAlt { get; set; }
    }
}

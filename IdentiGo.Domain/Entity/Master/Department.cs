using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Domain.Entity.Master
{
    [Table("DEPARTMENT")]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Country")]
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

        public virtual ICollection<City> City { get; set; }
    }
}

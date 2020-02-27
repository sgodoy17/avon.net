using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Domain.Entity.Master
{
    [Table("COUNTRY")]
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [StringLength(10)]
        [Display(Name = "ID Alternativo (Otros sistemas)")]
        public string IdAlt { get; set; }

        public virtual ICollection<Department> Department { get; set; }

    }
}

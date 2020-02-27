using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentiGo.Domain.Security;
using IdentiGo.Domain.Enums;
namespace IdentiGo.Domain.Entity.Master
{
    [Table("UNIT")]
    public class Unit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Número de Zona")]
        [ForeignKey("Zone")]
        public Guid ZoneId { get; set; }
        public virtual Zone Zone { get; set; }

        [Display(Name = "Número")]
        public string Number { get; set; }

        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [NotMapped]
        public string NumberZone { get; set; }

        [NotMapped]
        public string CodeName { get { return $"{Name}-{Code}"; } set{ } }

        [NotMapped]
        public string NumberCodeName { get { return $"{Number} - {Code} - {Name}"; } set { } }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace IdentiGo.Domain.Entity.Master
{
    [Table("QUOTA")]
    public class Quota
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Monto")]
        public string Amount { get; set; }

        public virtual ICollection<RiskLevel> RiskLevel { get; set; }
    }
}

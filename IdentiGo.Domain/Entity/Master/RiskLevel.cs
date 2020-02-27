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
    [Table("RISKLEVEL")]
    public class RiskLevel
    {

        public RiskLevel() 
        {
            Quota = new HashSet<Quota>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Nivel")]
        public int Level { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public virtual ICollection<Quota> Quota { get; set; }

        [NotMapped]
        public IEnumerable<System.Web.Mvc.SelectListItem> QuotaList { get; set; }
    }
}

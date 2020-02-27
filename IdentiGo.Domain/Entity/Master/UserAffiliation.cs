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
    [Table("USERAFFILIATION")]
    public class UserAffiliation
    {
        public UserAffiliation() 
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Tipo de Afiliación")]
        [ForeignKey("AffiliationType")]
        public Guid AffiliationTypeId { get; set; }
        public virtual AffiliationType AffiliationType { get; set; }

    }
}

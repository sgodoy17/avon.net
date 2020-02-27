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
    [Table("SECRETARYTRANSIT")]
    public class SecretaryTransit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
}

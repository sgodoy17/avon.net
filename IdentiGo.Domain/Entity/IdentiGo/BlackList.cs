using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Entity.IdentiGo
{
    [Table("BLACKLIST")]
    public class BlackList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Documento")]
        public string Document { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
}

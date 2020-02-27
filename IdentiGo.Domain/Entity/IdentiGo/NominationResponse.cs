using IdentiGo.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Entity.IdentiGo
{
    [Table("NOMINATIONRESPONSE")]
    public class NominationResponse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Nombramiento")]
        [ForeignKey("Nomination")]
        public Guid NominationId { get; set; }

        public virtual Nomination Nomination { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public StageProccess Stage { get; set; }
    }
}

using IdentiGo.Domain.Entity.IdentiGo;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Entity.AVON
{
    [Table("CASHPAYMENT")]
    public class CashPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Nomination")]
        public Guid NominationId { get; set; }

        public virtual Nomination Nomination { get; set; }

        public int Document { get; set; }

        public string Genre { get; set; }

        public string Risk { get; set; }

        public int BirthDate { get; set; }

        public int Division { get; set; }

        public int Zone { get; set; }

        public int Unit { get; set; }

        public DateTime DateCreated { get; set; }

        public int Result { get; set; }

        public string Description { get; set; }
    }
}

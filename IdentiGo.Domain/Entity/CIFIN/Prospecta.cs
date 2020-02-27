using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentiGo.Domain.Entity.IdentiGo;

namespace IdentiGo.Domain.Entity.CIFIN
{
    public class Prospecta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Nomination")]
        public Guid NominationId { get; set; }

        public virtual Nomination Nomination { get; set; }

        public bool? consultadaReciente { get; set; }

        public bool? generoInconsistencias { get; set; }

        //public InconsistenciasDTO[] inconsistencias { get; set; }

        public string nombreTitular { get; set; }

        public string numeroIdentificacion { get; set; }

        public string resultado { get; set; }

        public string tipoIdentificacion { get; set; }
    }
}

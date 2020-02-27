using IdentiGo.Domain.Entity.Master;
using IdentiGo.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Entity.IdentiGo
{
    [Table("NOMINATIONHISTORIC")]
    public class NominationHistoric
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Nombramiento")]
        [ForeignKey("Nomination")]
        public Guid NominationId { get; set; }

        public virtual Nomination Nomination { get; set; }

        [Display(Name = "Fecha de Consulta/Cambio")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Creación")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Display(Name = "Código de Verificación")]
        public int? CodeVerification { get; set; }

        [Display(Name = "Campaña")]
        [ForeignKey("Campaing")]
        public Guid? CampaingId { get; set; }

        public virtual Campaing Campaing { get; set; }

        [Display(Name = "División")]
        [ForeignKey("Division")]
        public Guid? DivisionId { get; set; }
        public virtual Division Division { get; set; }

        [Display(Name = "Zona")]
        [ForeignKey("Zone")]
        public Guid? ZoneId { get; set; }

        public virtual Zone Zone { get; set; }

        [Display(Name = "Unidad")]
        [ForeignKey("Unit")]
        public Guid? UnitId { get; set; }

        public virtual Unit Unit { get; set; }

        [Display(Name = "Estado del Proceso")]
        public State State { get; set; }

        [Display(Name = "Estado del Documento")]
        public StateDocument StateDocument { get; set; }

        [Display(Name = "Etapa del Proceso")]
        public StageProccess StageProcess { get; set; }

        [Display(Name = "Teléfono Candidata")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Teléfono Empresaria")]
        public string PhoneAnswer { get; set; }

        [Display(Name = "Código de Empresaria")]
        public string CodeUser { get; set; }
    }
}

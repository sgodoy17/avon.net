using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentiGo.Domain.Entity.General;
using IdentiGo.Domain.Enums;
using IdentiGo.Domain.Security;
using IdentiGo.Domain.Entity.CIFIN;
using IdentiGo.Domain.Entity.Master;
using System.ComponentModel;

namespace IdentiGo.Domain.Entity.IdentiGo
{
    [Table("NOMINATION")]
    public class Nomination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public virtual ICollection<NominationHistoric> NominationHistoric { get; set; }

        [Display(Name = "Usuario")]
        [ForeignKey("User")]
        public Guid? UserId { get; set; }

        public User User { get; set; }
        
        public virtual Company Company { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Documento")]
        public string Document { get; set; }

        [Display(Name = "Nombre(s)")]
        public string Name { get; set; }

        [Display(Name = "Apellidos(s)")]
        public string LastName { get; set; }

        [Display(Name = "Teléfono Candidata")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Teléfono Empresaria")]
        public string PhoneAnswer { get; set; }

        [Display(Name = "Cupo")]
        public string Score { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Modificación")]
        public DateTime DateUpdate { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Ultima Validación")]
        public DateTime DateLastValidation { get; set; } = DateTime.Now;

        [Display(Name = "Fecha Finalización")]
        [DefaultValue("")]
        public DateTime? DateNomination { get; set; }

        [Display(Name = "Estado del Proceso")]
        public State State { get; set; }

        [Display(Name = "Estado del Documento")]
        public StateDocument StateDocument { get; set; }

        [Display(Name = "Etapa del Proceso")]
        public StageProccess StageProcess { get; set; }

        [Display(Name = "# Intento por Día")]
        public int NumberIntentByDay { get; set; }

        [Display(Name = "Total de Intentos")]
        public int TotalNumberIntent { get; set; }

        [Display(Name = "Código de Empresaria")]
        public string CodeUser { get; set; }

        [Display(Name = "Código de Verificación")]
        public int? CodeVerification { get; set; }

        [Display(Name = "Tipo de Teléfono")]
        public TypePhone TypePhone { get; set; }

        [Display(Name = "Tipo de Proceso")]
        public TypeProcess TypeProcess { get; set; }

        [Display(Name = "Sexo")]
        public Gender Sex { get; set; } = Gender.Female;

        #region Relations

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

        [ForeignKey("RiskLevel")]
        [Display(Name = "Nivel de Riesgo")]
        public Guid? RiskLevelId { get; set; }

        public virtual RiskLevel RiskLevel { get; set; }

        [Display(Name = "Nivel de Riesgo Avon")]
        public int AvonRiskLevel { get; set; }

        public virtual ICollection<ValidadorPlus> InfoCifin { get; set; }

        public virtual ICollection<Prospecta> InfoProspecta { get; set; }

        #endregion

        #region notMapped
        
        [NotMapped]
        public bool Consult { get; set; }

        [NotMapped]
        public TypeState TypeState { get; set; }

        #endregion
    }

    public enum TypeProcess
    {
        [Display(Name = "Aplicativo Android")]
        Appi,
        [Display(Name = "Manual")]
        Manual,
        [Display(Name = "Aplicativo IOS")]
        AppiIOS,
        [Display(Name = "Manager")]
        Manager,
    }

    public enum TypePhone
    {
        [Display(Name = "Celular")]
        Cell = 1,
        [Display(Name = "Fijo")]
        Fixed = 2
    }

    public enum TypeState
    {
        [Display(Name = "Todos")]
        All,
        [Display(Name = "Aprobados")]
        Valid,
        [Display(Name = "Incumplen")]
        Invalid,
        [Display(Name = "Pendientes")]
        Pending
    }

    public class NominationString
    {
        public string CodeUser { get; set; }

        public string Document { get; set; }

        public string TypePhone { get; set; }

        public string PhoneNumber { get; set; }

        public string PhoneAnswer { get; set; }

        public string Campaing { get; set; }

        public string Genre { get; set; }
    }
}

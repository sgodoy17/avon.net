using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace IdentiGo.Domain.Entity.Master
{
    [Table("ZONE")]
    public class Zone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Número de División")]
        [ForeignKey("Division")]
        public Guid DivisionId { get; set; }
        public virtual Division Division { get; set; }

        [Display(Name = "Número")]
        public string Number { get; set; }

        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Auto Nombramiento")]
        public bool SelfSend { get; set; }

        [Display(Name = "Intentos por Teléfono")]
        public int NumberTrys { get; set; }

        [Display(Name = "Enviar Código Verificación")]
        public bool SendCode { get; set; }

        [Display(Name = "Aplica Pago Contado")]
        public bool CashPayment { get; set; }

        [NotMapped]
        public string NumberDivision { get; set; }

        [NotMapped]
        public string DisplayValue { get => $"{Number} - {Code} - {Name}"; set { } }
    }
}

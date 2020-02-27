using System.ComponentModel.DataAnnotations;

namespace IdentiGo.Domain.Enums
{
    public enum CashPaymentEnum
    {
        [Display(Name = "No aplica para pago contado")]
        NotApply = 0,
        [Display(Name = "Aplica para pago contado")]
        Apply = 1,
        [Display(Name = "No ingreso cedula")]
        Document = 2,
        [Display(Name = "Genero no valido")]
        Genre = 3,
        [Display(Name = "División no valida")]
        Division = 4,
        [Display(Name = "Zona no valida")]
        Zone = 5,
        [Display(Name = "Unidad no valida")]
        Unit = 6,
        [Display(Name = "Fecha Nacimiento no valida")]
        BirthDate = 7,
        [Display(Name = "Nivel de Riesgo invalido")]
        Risk = 8
    }
}

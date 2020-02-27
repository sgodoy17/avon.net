using System.ComponentModel.DataAnnotations;
namespace IdentiGo.Domain.Enums
{
    public enum TypeControl
    {
        [Display(Name = "Selección Unica")]
        Radio = 0,
        [Display(Name = "Multiple Selección")]
        Checkbox = 1
    }
}
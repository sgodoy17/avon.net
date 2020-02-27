using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Domain.Enums
{
    public enum PageRespon
    {
        [Display(Name = "Por Defecto")]
        Default = 0,
        [Display(Name = "Exitoso")]
        Success = 1,
        [Display(Name = "Erroneo")]
        Error = 2,
        [Display(Name = "No responde")]
        NotFound = 3,
        [Display(Name = "No hay información")]
        NotInformation = 5,
        [Display(Name = "No Aplica")]
        NotApply = 4
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Domain.Enums
{
    public enum TypeConsultCandidateResponse
    {
        [Display(Name = "Todos")]
        All,
        [Display(Name = "Aceptaron")]
        Accept,
        [Display(Name = "No Aceptaron")]
        NoAccept
    }
}

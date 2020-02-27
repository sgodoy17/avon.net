using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Domain.Enums
{
    public enum StageProccess
    {
        [Display(Name = "Inicializado")]
        Init = 0,
        [Display(Name = "Aprobación de Tratamiento")]
        DataAuthorization = 1,
        [Display(Name = "Valida AVON")]
        ValidaAvon = 2,
        [Display(Name = "Confirmación de Candidata")]
        CandidateConfirmation = 3,
        [Display(Name = "Score")]
        Score = 4,
        [Display(Name = "Finalizado")]
        Finished = 5,
        [Display(Name = "QuestionValidation")]
        QuestionValidation = 11,
        [Display(Name = "ConsultInformation")]
        ConsultInformation = 12,
    }
}

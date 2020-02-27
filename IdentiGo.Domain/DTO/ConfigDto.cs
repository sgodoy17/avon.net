using IdentiGo.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdentiGo.Domain.DTO
{
    public class ConfigDto
    {
        public Guid Id { get; set; }

        [Display(Name = "# Preguntas")]
        [Required(ErrorMessage = "El campo # Preguntas es obligatorio")]
        [Range(0, int.MaxValue)]
        public int NumberQuestion { get; set; }

        [Display(Name = "# Preguntas Correctas")]
        [Required(ErrorMessage = "El campo # Preguntas Correctas es obligatorio")]
        [Range(0, int.MaxValue)]
        public int NumberQuestionValid { get; set; }

        [Display(Name = "# Intentos por Document")]
        [Required(ErrorMessage = "El campo # Intentos por Documento es obligatorio")]
        [Range(0, int.MaxValue)]
        public int NumberIntentByDocument { get; set; }

        [Required(ErrorMessage = "El campo Tiempo de Espera es requerido")]
        [Display(Name = "Tiempo de Espera")]
        public int TimeOut { get; set; }

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "El campo Empresa es obligatorio")]
        public int CompanyId { get; set; }
        public virtual CompanyDto Company { get; set; }

        public List<PageDto> Page { get; set; }

        public virtual IEnumerable<SelectListItem> PageList { get; set; }

        //public Config UpdateEntity() 
        //{
        //    return new Config 
        //    {
        //        Id = this.Id,
        //        NumberIntentByDocument = this.NumberIntentByDocument,
        //        NumberQuestion = this.NumberQuestion,
        //        NumberQuestionValid = this.NumberQuestionValid,
        //        CompanyID = this.
        //    }
        //}

    }
}

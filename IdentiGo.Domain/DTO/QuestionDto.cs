using IdentiGo.Domain.DTO.Master;
using IdentiGo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentiGo.Domain.DTO
{
    public class QuestionDto
    {
        public QuestionDto()
        {
            QuestionOption = new List<QuestionOptionDto>();
        }

        public Guid Id { get; set; }

        [Display(Name = "Texto")]
        [Required(ErrorMessage = "El campo Texto es Obligatorio")]
        public string Text { get; set; }

        [Display(Name = "Tipo de Pregunta")]
        [Required(ErrorMessage = "El campo Tipo de Pregunta es Obligatorio")]
        public TypeQuestion TypeQuestion { get; set; }

        [Display(Name = "Tipo de Afiliación")]
        public Guid? AffiliationTypeId { get; set; }
        public virtual AffiliationTypeDto AffiliationType { get; set; }

        [Display(Name = "Tipo de Control")]
        [Required(ErrorMessage = "El campo Tipo de Control es obligatorio")]
        public TypeControl TypeControl { get; set; }

        [Required(ErrorMessage = "El campo Número de Opciones es obligatorio")]
        [Display(Name = "Número de Opciones")]
        [Range(1, 6)]
        public int NumberOption { get; set; }

        [Required(ErrorMessage = "El campo página es requerido")]
        [Display(Name = "Página")]
        public Guid PageId { get; set; }
        public virtual PageDto Page { get; set; }
        
        [Display(Name = "Campo")]
        [Required(ErrorMessage = "El campo Campo es obligatorio")]
        public string Field { get; set; }

        public virtual List<QuestionOptionDto> QuestionOption { get; set; }
    }
}

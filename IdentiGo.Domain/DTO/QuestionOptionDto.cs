using IdentiGo.Domain.DTO.Master;
using IdentiGo.Domain.Entity.Master;
using IdentiGo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Domain.DTO
{
    public class QuestionOptionDto
    {
        public Guid Id { get; set; }

        public Guid QuestionId { get; set; }
        public virtual QuestionDto Question { get; set; }

        [Display(Name = "Texto")]
        [Required(ErrorMessage = "El campo Texto es Obligatorio")]
        public string Text { get; set; }

        public bool Valid { get; set; }

        public bool Selected { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace IdentiGo.Domain.DTO.Master
{
    public class VoteSiteDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Name { get; set; }

    }
}

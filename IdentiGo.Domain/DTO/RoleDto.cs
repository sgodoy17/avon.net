using IdentiGo.Domain.DTO;
using IdentiGo.Domain.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Security
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Nombre a Mostrar")]
        public string DisplayName { get; set; }

        [Display(Name = "Tipo de Rol")]
        public TypeRole TypeRole { get; set; }

        public virtual List<CompanyDto> Company { get; set; }
    }
}

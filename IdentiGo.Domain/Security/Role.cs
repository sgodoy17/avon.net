using IdentiGo.Domain.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentiGo.Domain.Entity.General;

namespace IdentiGo.Domain.Security
{
    public class Role : IdentityRole<Guid, UserRole>
    {
        public Role() 
        {
        }

        public Role(string name)
        {
            this.Name = name;
        }

        [Display(Name = "Nombre a Mostrar")]
        public string DisplayName { get; set; }

        [Display(Name = "Tipo de Rol")]
        public TypeRole TypeRole { get; set; }

        public virtual ICollection<GroupRole> GroupRole { get; set; }
    }

    public enum TypeRole 
    {
        [Display(Name = "Rol")]
        Role,
        [Display(Name = "Permiso")]
        Modulo
    }
}

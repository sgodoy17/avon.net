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
    public class GroupRole
    {
        public GroupRole() 
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        public virtual ICollection<Role> Role { get; set; }

        public virtual ICollection<User> User { get; set; }

    }
}

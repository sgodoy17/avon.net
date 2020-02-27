using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Security
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        //public virtual User User { get; set; }
    }
}

using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Security
{
    public class UserClaim : IdentityUserClaim<Guid>
    {

    }
}

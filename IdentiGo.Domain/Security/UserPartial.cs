using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentiGo.Domain.DTO;
using IdentiGo.Domain.Enums;
using Component.Transversal.Cryptography;
using Component.Transversal.WebApi;
using Component.Transversal.WebApi.Responses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Component = System.ComponentModel.Component;
using PasswordHasher = Component.Transversal.Cryptography.PasswordHasher;
using IdentiGo.Domain.Entity;

namespace IdentiGo.Domain.Security
{
    ///// <summary>
    ///// Represents a bicycle user or employee: any user interacting with the software.
    ///// </summary>
    //public partial class User
    //{
    //}
}

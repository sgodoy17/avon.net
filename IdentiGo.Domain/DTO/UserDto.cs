using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentiGo.Domain.Enums;
using IdentiGo.Domain.Security;
using Component.Transversal.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace IdentiGo.Domain.DTO
{
    public class UserDto
    {
        /// <summary>
        /// Unique ID of User, provided by <see cref="Microsoft.AspNet.Identity.EntityFramework.IdentityUser">IdentityUser</see>
        /// </summary>
        public Guid Id { get; set; }

        [Display(Name = "Empresa")]
        public int? CompanyId { get; set; }
        public virtual CompanyDto Company { get; set; }

        /// <summary>
        /// Email of user, provided by <see cref="Microsoft.AspNet.Identity.EntityFramework.IdentityUser">IdentityUser</see>
        /// </summary>
        [Required]
        [MaxLength(256)]
        [System.ComponentModel.DisplayName("Correo electrónico")]
        public string Email { get; set; }

        /// <summary>
        /// PhoneNumber of user, provided by <see cref="Microsoft.AspNet.Identity.EntityFramework.IdentityUser">IdentityUser</see>
        /// </summary>
        [Required]
        [MaxLength(15)]
        [System.ComponentModel.DisplayName("Número telefónico")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// First name of user
        /// </summary>
        [Required]
        [MaxLength(15)]
        [System.ComponentModel.DisplayName("Primer Nombre")]
        public string Name1 { get; set; }

        /// <summary>
        /// Second first name of user if applicable, otherwise null
        /// </summary>
        [MaxLength(15)]
        [System.ComponentModel.DisplayName("Segundo Nombre")]
        public string Name2 { get; set; }

        /// <summary>
        /// First last name of user
        /// </summary>
        [Required]
        [MaxLength(15)]
        [System.ComponentModel.DisplayName("Primer Apellido")]
        public string LastName1 { get; set; }

        /// <summary>
        /// Second last name of user if applicable, otherwise null
        /// </summary>
        [MaxLength(15)]
        [System.ComponentModel.DisplayName("Segundo Apellido")]
        public string LastName2 { get; set; }

        /// <summary>
        /// Document type of identification number:
        /// 
        /// 0 = Unknown
        /// 1 = CC Cedula Ciudanía
        /// 2 = CE Cédula Extrangería
        /// 3 = CD Carné Diplomatico
        /// 4 = TI Tarjeta de Identidad
        /// 5 = PA Pasaporte
        /// 6 = RC Registro Civil
        /// 7 = N.U.I.P. Número Único de Identificación Personal
        /// </summary>
        [Required]
        [System.ComponentModel.DisplayName("Tipo Document")]
        public IdentificationType IdType { get; set; }

        /// <summary>
        /// Number of the document specified in IdType
        /// </summary>
        [Required]
        [MaxLength(12)]
        [System.ComponentModel.DisplayName("Número Document")]
        public string IdNumber { get; set; }

        /// <summary>
        /// First address line
        /// </summary>
        [Required]
        [MaxLength(45)]
        [System.ComponentModel.DisplayName("Dirección 1")]
        public string Address1 { get; set; }

        /// <summary>
        /// Second address line
        /// </summary>
        [MaxLength(45)]
        [System.ComponentModel.DisplayName("Dirección 2")]
        public string Address2 { get; set; }

        /// <summary>
        /// Date of birth as datetime
        /// </summary>
        [Required]
        [System.ComponentModel.DisplayName("Fecha Nacimiento")]
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Gender using Gender enum
        /// </summary>
        [Required]
        [System.ComponentModel.DisplayName("Género")]
        public Gender Gender { get; set; }

        /// <summary>
        /// Represends the password entered by the user. This is not (directly) saved, but only used to transfer the data between the view and the controller. The password is checked to be the same as <see cref="ConfirmPassword">ConfirmPassword</see>, and is then saved in the database in a hashed form.
        /// </summary>
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        //[Required(ErrorMessage = "El campo Contraseña es obligatorio")]
        [NotMapped]
        public virtual string Password { get; set; }

        /// <summary>
        /// Represends the password confirmation entered by the user, when creating a new password.  This is not (directly) saved, but only used to transfer the data between the view and the controller. The password is checked to be the same as <see cref="Password">Password</see>, and is then saved in the database in a hashed form.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        //[Required(ErrorMessage = "El campo Confirmar Contraseña es obligatorio")]
        [NotMapped]
        public virtual string ConfirmPassword { get; set; }

        /// <summary>
        /// Date the account was activated by an employee of AMVA
        /// </summary>
        [System.ComponentModel.DisplayName("Fecha de Registro")]
        public DateTime ActivationDate { get; set; }

        /// <summary>
        /// Last update of this record
        /// </summary>
        [System.ComponentModel.DisplayName("Ultima Actualización")]
        public DateTime LastUpdate { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> RolesList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ModuleList { get; set; }

    }
}
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
using System.Security.Principal;
using System.Web.Mvc;
using IdentiGo.Domain.Entity.General;

namespace IdentiGo.Domain.Security
{
    /// <summary>
    /// Represents a bicycle user or employee: any user interacting with the software.
    /// </summary>
    public class User : IdentityUser<Guid, UserLogin, UserRole, UserClaim>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get { return base.Id; } set { base.Id = value; } }

        [Display(Name = "Nombre de Usuario")]
        public override string Email { get { return base.Email; } set { base.Email = value; } }

        [Display(Name = "Nombre de Usuario")]
        public override string UserName { get { return base.UserName; } set { base.UserName = value; } }
        /// <summary>
        /// First name of user
        /// </summary>
        [Required]
        [MaxLength(15)]
        [DisplayName("Primer Nombre")]
        public string Name1 { get; set; }

        /// <summary>
        /// Second first name of user if applicable, otherwise null
        /// </summary>
        [MaxLength(15)]
        [DisplayName("Segundo Nombre")]
        public string Name2 { get; set; }

        /// <summary>
        /// First last name of user
        /// </summary>
        [Required]
        [MaxLength(15)]
        [DisplayName("Primer Apellido")]
        public string LastName1 { get; set; }

        /// <summary>
        /// Second last name of user if applicable, otherwise null
        /// </summary>
        [MaxLength(15)]
        [DisplayName("Segundo Apellido")]
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
        [DisplayName("Tipo Document")]
        public IdentificationType IdType { get; set; }

        /// <summary>
        /// Number of the document specified in IdType
        /// </summary>
        [Required]
        [MaxLength(12)]
        [DisplayName("Número Document")]
        public string IdNumber { get; set; }

        /// <summary>
        /// First address line
        /// </summary>
        [Required]
        [MaxLength(45)]
        [DisplayName("Dirección 1")]
        public string Address1 { get; set; }

        /// <summary>
        /// Date of birth as datetime
        /// </summary>
        [Required]
        [DisplayName("Fecha Nacimiento")]
        public DateTime Birthdate { get; set; }
        
        /// <summary>
        /// Gender using Gender enum
        /// </summary>
        [Required]
        [DisplayName("Género")]
        public Gender Gender { get; set; }

        /// <summary>
        /// Date the account was activated by an employee of AMVA
        /// </summary>
        [DisplayName("Fecha de Registro")]
        public DateTime ActivationDate { get; set; }

        /// <summary>
        /// Last update of this record
        /// </summary>
        [DisplayName("Ultima Actualización")]
        public DateTime LastUpdate { get; set; }

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
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        //[Required(ErrorMessage = "El campo Confirmar Contraseña es obligatorio")]
        [NotMapped]
        public virtual string ConfirmPassword { get; set; }

        /// <summary>
        /// Returns the complete name of the user, firstnames and last names
        /// </summary>
        public virtual string CompleteName
        {
            get { return string.Format("{0} {1} {2} {3}", Name1, Name2, LastName1, LastName2); }
        }

        /// <summary>
        /// Returns the first first name and the first last name of the user
        /// </summary>
        public virtual string ShortName
        {
            get { return string.Format("{0} {1}", Name1, LastName1); }
        }

        [NotMapped]
        public IEnumerable<SelectListItem> RolesList { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> ModuleList { get; set; }

        public virtual ICollection<GroupRole> GroupRole { get; set; }

        /// <summary>
        /// Method to generate a ClaimsIdentity for a user trying to access the website or management portal.
        /// </summary>
        /// <param name="manager">The object which represents the user trying to log in</param>
        /// <returns>The new user identity</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, Guid> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            return userIdentity;
        }

        [Display(Name = "Código de Verificación")]
        public string CodeVerification { get; set; }

        public bool Active { get; set; } = true;

        public void UpdateFromUser(User model)
        {
            if (model == null) throw new ArgumentNullException("model");

            Id = model.Id;
            Email = model.Email;
            PhoneNumber = model.PhoneNumber;
            Name1 = model.Name1;
            Name2 = model.Name2;
            LastName1 = model.LastName1;
            LastName2 = model.LastName2;
            IdType = model.IdType;
            IdNumber = model.IdNumber;
            Address1 = model.Address1;
            Birthdate = model.Birthdate;
            Gender = model.Gender;
            CodeVerification = model.CodeVerification;
            Active = model.Active;
        }
    }
}

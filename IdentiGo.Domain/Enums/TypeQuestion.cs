using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentiGo.Domain.Enums
{
    /// <summary>
    /// Bloodtype
    /// </summary>
    public enum TypeQuestion
    {
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Dirección")]
        Address,

        /// <summary>
        /// User Affiliation
        /// </summary>
        [Display(Name = "Afiliación")]
        Affiliation,

        /// <summary>
        /// Type Date
        /// </summary>
        [Display(Name = "Fecha")]
        Date,

        /// <summary>
        /// Type Municipality
        /// </summary>
        [Display(Name = "Municipality")]
        Municipality,

        /// <summary>
        /// Type Departament
        /// </summary>
        [Display(Name = "Department")]
        Department,

        /// <summary>
        /// Type Number
        /// </summary>
        [Display(Name = "Númerica")]
        Number,

        /// <summary>
        /// Type Affiliation
        /// </summary>
        [Display(Name = "Tipo de Afiliación")]
        TypeAffiliation,

        /// <summary>
        /// Vote Site
        /// </summary>
        [Display(Name = "Sitio de Votación")]
        VoteSite,

        /// <summary>
        /// Vote Site
        /// </summary>
        [Display(Name = "Secretaria de Tránsito")]
        SecretaryTransit
        
    }
}
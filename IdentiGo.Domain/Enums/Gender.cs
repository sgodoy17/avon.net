using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Domain.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// The gender has not been specified or is unknown.
        /// </summary>
        [Display(Name = "Undefined")]
        Undefined = 0,
        /// <summary>
        /// Male, masculine.
        /// </summary>
        [Display(Name = "Masculino")]
        Male = 1,
        /// <summary>
        /// Female, feminine.
        /// </summary>
        [Display(Name = "Femenino")]
        Female = 2,
        /// <summary>
        /// The person's gender is not male nor female.
        /// </summary>
        [Display(Name = "Other")]
        Other = 3
    }
}

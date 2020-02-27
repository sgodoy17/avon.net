using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Component.Transversal.WebApi.Documentation;

namespace Component.Transversal.WebApi.ViewModel
{
    /// <summary>
    /// Used to request basic user information based on Civica Card Number
    /// </summary>
    public class ValidateUserViewModel
    {
        /// <summary>
        /// Id of Totem who makes the request
        /// </summary>
        [Required]
        [SampleData(12)]
        public int Totem { get; set; }

        /// <summary>
        /// User Civica Card Number
        /// </summary>
        [Required]
        [SampleData("4998712")]
        public string CivicaCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component.Transversal.WebApi.Documentation;

namespace Component.Transversal.WebApi.Responses
{
    /// <summary>
    /// Initial response for user information, that is obtained using the card ID or cedula number.
    /// </summary>
    public class ValidateUserResponse
    {
        /// <summary>
        /// Unique user ID
        /// </summary>
        [SampleData(57)]
        public Guid Id { get; set; }

        /// <summary>
        /// The user-based salt for generating the password hash
        /// </summary>
        [SampleData("{60869A86-D153-4493-8AB4-778E58E92F04}")]
        public string Salt { get; set; }

        /// <summary>
        /// Short name of the user, combination of first first name and first last name
        /// </summary>
        [SampleData("Pepito Perez")]
        public string Name { get; set; }

        /// <summary>
        /// Current validation status of the user
        /// </summary>
        [SampleData(BicycleUserStatus.Validated)]
        public BicycleUserStatus BicycleUserStatus { get; set; }
    }
}

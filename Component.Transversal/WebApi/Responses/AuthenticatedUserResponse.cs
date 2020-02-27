using System;
using Component.Transversal.WebApi.Documentation;

namespace Component.Transversal.WebApi.Responses
{
    /// <summary>
    /// Response after a user has tried to authenticate
    /// </summary>
    public class AuthenticatedUserResponse
    {
        /// <summary>
        /// Complete name of the user, combination of both first names and both last name
        /// </summary>
        [SampleData("Pepito Perez Alvarez")]
        public string CompleteName { get; set; }

        /// <summary>
        /// Current validation status of the user
        /// </summary>
        [SampleData(BicycleUserStatus.Validated)]
        public BicycleUserStatus BicycleUserStatus { get; set; }

        /// <summary>
        /// Authentication status of the user in the current session at a totem. 
        /// </summary>
        [SampleData(AuthenticationStatus.Authenticated)]
        public AuthenticationStatus AuthenticationStatus { get; set; }
    }
}
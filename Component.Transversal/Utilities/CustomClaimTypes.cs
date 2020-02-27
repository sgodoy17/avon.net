namespace Component.Transversal.Utilities
{
    /// <summary>
    /// Types of defined claims for application 
    /// </summary>
    public class CustomClaimTypes
    {
        /// <summary>
        /// Claim for User Identification Number
        /// </summary>
        public const string UserId = "http://app/identity/claims/UserId";

        public const string IsAuthenticated = "http://app/identity/claims/IsAuthenticated";

        public const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public const string IdentityProvider = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";

        /// <summary>
        /// Claim for secondary Identification Number User
        /// </summary>
        public const string UserId2 = "http://app/identity/claims/UserId2";

        /// <summary>
        /// Claim for the user's full name
        /// </summary>
        public const string UserFullName = "http://app/identity/claims/FullName";

        /// <summary>
        /// Claim for the anonymous user name
        /// </summary>
        public const string AnonimousUserName = "http://app/identity/claims/AnonimousUserName";

        /// <summary>
        /// Claim for user email
        /// </summary>
        public const string UserEmail = "http://app/identity/claims/Email";

        /// <summary>
        /// Claim for user signature
        /// </summary>
        public const string UserSignature = "http://app/identity/claims/Signature";

        /// <summary>
        /// Claim for Profile
        /// </summary>
        public const string Profile = "http://app/identity/claims/Profile";

        /// <summary>
        /// Claim to know if they are authorized to enter the security system
        /// </summary>
        public const string SecuritySystemAllowed = "http://app/identity/claims/SecuritySystemAllowed";

        /// <summary>
        /// Claim to know the password expiration
        /// </summary>
        public const string PasswordExpiration = "http://app/identity/claims/PasswordExpiration";

        /// <summary>
        /// Claim to know the password expiration
        /// </summary>
        public const string ExpiredPassword = "http://app/identity/claims/ExpiredPassword";

        /// <summary>
        /// Claim for the type of user within the application
        /// </summary>
        public const string TypeUserLogin = "http://app/identity/claims/TypeUserLogin";

        /// <summary>
        /// Claim for Enterprise
        /// </summary>
        public const string EnterpriseFieldFormat = "http://app/identity/claims/Enterprise/{0}";

    }
}

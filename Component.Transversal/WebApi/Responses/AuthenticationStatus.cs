namespace Component.Transversal.WebApi.Responses
{
    /// <summary>
    /// Authentication status of a user in the current session at a totem. Status 'Authenticated' is required to continue in the process.
    /// </summary>
    public enum AuthenticationStatus
    {
        /// <summary>
        /// Unknown status
        /// </summary>
        NotAuthenticated = 0,

        /// <summary>
        /// User has been successfully authenticated using his or her pin code. Allow access.
        /// </summary>
        Authenticated = 1,

        /// <summary>
        /// User has supplied a wrong pin code. Deny all access access, but give the user the option to try again.
        /// </summary>
        WrongPinCode = 2,

        /// <summary>
        /// User has been temporarily locked out,  and may try again after a certain time.
        /// </summary>
        TooManyAttemptsBlockedTemporarily = 3,

        /// <summary>
        /// User has been locked out, and is required to reset the pin code in the web page or at a attention center of AMVA.
        /// </summary>
        TooManyAttemptsBlocked = 4
    }
}
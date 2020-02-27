using Component.Transversal.WebApi.Documentation;

namespace Component.Transversal.WebApi.Responses
{
    /// <summary>
    /// Gives the current validation status of the user:
    /// 
    /// 0 = unvalidated
    /// 1 = validated and active
    /// 2 = blocked
    /// 3 = sanction active
    /// 4 = revoked
    /// </summary>
    public enum BicycleUserStatus
    {
        /// <summary>
        /// 0 = unvalidated
        /// </summary>
        Unvalidated = 0,
        /// <summary>
        /// 1 = validated and active
        /// </summary>
        Validated = 1,
        /// <summary>
        /// 2 = blocked
        /// </summary>
        Blocked = 2,
        /// <summary>
        /// 3 = sanction active
        /// </summary>
        SantionActive = 3,
        /// <summary>
        /// 4 = revoked
        /// </summary>
        Revoked = 4
    }
}
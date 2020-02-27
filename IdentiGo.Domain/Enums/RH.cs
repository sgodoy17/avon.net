using System.ComponentModel;

namespace IdentiGo.Domain.Enums
{
    /// <summary>
    /// Bloodtype
    /// </summary>
    public enum Rh
    {
        /// <summary>
        /// Bloodtype undefined
        /// </summary>
        [Description("No definido")]
        Undefined = 0,

        /// <summary>
        /// RH O+ / O Positive 
        /// </summary>
        [Description("O+")]
        Op = 1,

        /// <summary>
        /// RH O- / O Negative
        /// </summary>
        [Description("O-")]
        On = 2,

        /// <summary>
        /// RH B+ / B Positive 
        /// </summary>
        [Description("B+")]
        Bp = 3,

        /// <summary>
        /// RH B- / B Negative 
        /// </summary>
        [Description("B-")]
        Bn = 4,

        /// <summary>
        /// RH A+ / A Positive 
        /// </summary>
        [Description("A+")]
        Ap = 5,

        /// <summary>
        /// RH A- / A Negative 
        /// </summary>
        [Description("A-")]
        An = 6,

        /// <summary>
        /// RH AB+ / AB Positive 
        /// </summary>
        [Description("AB+")]
        ABp = 7,

        /// <summary>
        /// RH AB- / AB Negative 
        /// </summary>
        [Description("AB-")]
        ABn = 8
    }
}
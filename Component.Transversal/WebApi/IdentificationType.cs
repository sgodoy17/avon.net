using System.ComponentModel.DataAnnotations;
namespace Component.Transversal.WebApi
{
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
    public enum IdentificationType
    {
        /// <summary>
        /// 0 = Unknown
        /// </summary>
        [Display(Name = "Unknown")]
        Unknown = 0,

        /// <summary>
        /// 1 = CC Cedula Ciudanía
        /// </summary>
        [Display(Name = "Cédula Ciudania")]
        CedulaCiudania = 1,

        /// <summary>
        /// 2 = CE Cédula Extrangería
        /// </summary>
        [Display(Name = "Cédula Extrangeria")]
        CedulaExtrangeria = 2,

        /// <summary>
        /// 3 = CD Carné Diplomatico
        /// </summary>
        [Display(Name = "Carne Diplomatico")]
        CarneDiplomatico = 3,

        /// <summary>
        /// 4 = TI Tarjeta de Identidad
        /// </summary>
        [Display(Name = "Tarjeta De Identidad")]
        TarjetaDeIdentidad = 4,

        /// <summary>
        /// 5 = PA Pasaporte
        /// </summary>
        [Display(Name = "Pasaporte")]
        Pasaporte = 5,

        /// <summary>
        /// 6 = RC Registro Civil
        /// </summary>
        [Display(Name = "Registro Civil")]
        RegistroCivil = 6,

        /// <summary>
        /// 7 = N.U.I.P. Número Único de Identificación Personal
        /// </summary>
        [Display(Name = "N.U.I.P.")]
        Nuip = 7
    }
}

using IdentiGo.ExternalServices.WsNombraAvon;
using System;

namespace IdentiGo.ExternalServices
{
    /// <summary>
    /// NombraYaExternalService
    /// </summary>
    public class NombraAvonExternalService
    {
        private AvonServiceEndPointService NombraAvonService;

        public NombraAvonExternalService()
        {
            try
            {
                NombraAvonService = new AvonServiceEndPointService();
            }
            catch
            {
                throw new Exception("Servicio AVON Fuera de línea. Por favor comuniquese con servicio a la representante");
            }
        }

        /// <summary>
        /// ConsultDocument
        /// </summary>
        /// <param name="parameter1">string</param>
        /// <param name="parameter2">string</param>
        /// <param name="parameter3">string</param>
        /// <returns></returns>
        public Result ConsultDocument(string parameter1, string parameter2 = "", string parameter3 = "")
        {
            return NombraAvonService.ConsultarCedula(parameter1, parameter2, parameter3);
        }

        /// <summary>
        /// ConsultCode
        /// </summary>
        /// <param name="parameter1">string</param>
        /// <param name="parameter2">string</param>
        /// <param name="parameter3">string</param>
        /// <returns></returns>
        public Result ConsultCode(string parameter1, string parameter2 = "", string parameter3 = "")
        {
            return NombraAvonService.ConsultarGZSL(parameter1, parameter2, parameter3);
        }

        /// <summary>
        /// PagoContado
        /// </summary>
        /// <param name="cedula">int</param>
        /// <param name="genero">string</param>
        /// <param name="riesgo">string</param>
        /// <param name="fec_Nacimiento">int</param>
        /// <param name="division">int</param>
        /// <param name="zona">int</param>
        /// <param name="unidad">int</param>
        /// <returns></returns>
        public int PagoContado(int cedula, string genero, string riesgo, int fec_Nacimiento, int division, int zona, int unidad)
        {
            return NombraAvonService.ValidarPagoContado(cedula, genero, riesgo, fec_Nacimiento, division, zona, unidad);
        }
    }
}

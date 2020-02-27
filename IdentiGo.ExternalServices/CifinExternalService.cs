using IdentiGo.Domain.Entity.CIFIN;
using IdentiGo.Domain.Entity.General;
using IdentiGo.ExternalServices.WsInformacionComercial;
using IdentiGo.ExternalServices.WsProspecta;
using IdentiGo.ExternalServices.WsProspectaPlus;
using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

namespace IdentiGo.ExternalServices
{
    /// <summary>
    /// Class CifinExternalService
    /// Privides a way to connect to CIFIN service and obtains commercial and forecast information and assign it to an XML format
    /// </summary>
    public class CifinExternalService
    {
        private InformacionComercialWSService infComercialWse;
        private ProspectaPlusWSService ProspectaPlusWSService;
        private ProspectaWSService prospectaWse;

        /// <summary>
        /// Method CifinExternalService
        /// Validate the external connection to CIFIN with the credentials provided
        /// </summary>
        /// <param name="config">The credentials for this service</param>
        public CifinExternalService(Config config)
        {
            try
            {
                SecurityToken clientToken = X509TokenProvider.CreateToken(StoreLocation.LocalMachine, StoreName.My, "info@enterdev.com.co", X509FindType.FindByIssuerName);
                MessageSignature sig = new MessageSignature(clientToken);

                infComercialWse = new InformacionComercialWSService
                {
                    Credentials = new NetworkCredential(config.UserCIFIN, config.PasswordCIFIN),
                    Url = "https://cifin.asobancaria.com/InformacionComercialWS/services/InformacionComercial",
                };

                infComercialWse.RequestSoapContext.Security.Tokens.Add(clientToken);
                infComercialWse.RequestSoapContext.Security.Elements.Add(sig);

                prospectaWse = new ProspectaWSService
                {
                    Credentials = new NetworkCredential(config.UserCIFIN, config.PasswordCIFIN),
                    Url = "https://cifin.asobancaria.com/ws/ProspectaWebService/services/Prospecta",
                };

                prospectaWse.RequestSoapContext.Security.Tokens.Add(clientToken);
                prospectaWse.RequestSoapContext.Security.Elements.Add(sig);

                ProspectaPlusWSService = new ProspectaPlusWSService
                {
                    Credentials = new NetworkCredential(config.UserCIFIN, config.PasswordCIFIN),
                    Url = "https://cifin.asobancaria.com/ws/ProspectaPlusWebService/services/ProspectaPlus",
                };

                ProspectaPlusWSService.RequestSoapContext.Security.Tokens.Add(clientToken);
                ProspectaPlusWSService.RequestSoapContext.Security.Elements.Add(sig);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Method ConsultProspecta
        /// Validate the user's identity docuemnt
        /// </summary>
        /// <param name="numberDocument">User's document</param>
        /// <returns>Cifin user information</returns>
        //public Prospecta ConsultProspecta(string numberDocument)
        //{
        //    var prospectaDto = prospectaWse.consultaProspecta("2058", numberDocument, "1");

        //    if (prospectaDto == null)
        //        return null;

        //    return new Prospecta
        //    {
        //        consultadaReciente = prospectaDto.consultadaReciente,
        //        generoInconsistencias = prospectaDto.generoInconsistencias,
        //        nombreTitular = prospectaDto.nombreTitular,
        //        numeroIdentificacion = prospectaDto.numeroIdentificacion,
        //        resultado = prospectaDto.resultado,
        //        tipoIdentificacion = prospectaDto.tipoIdentificacion,
        //    };
        //}

        /// <summary>
        /// Method ConsultProspectaPlus
        /// Validate the user's identity docuemnt
        /// </summary>
        /// <param name="numberDocument">User's document</param>
        /// <returns>Cifin user information</returns>
        public Prospecta ConsultProspecta(string numberDocument)
        {
            var prospectaDto = ProspectaPlusWSService.consultaProspecta("3058", numberDocument, "1");

            if (prospectaDto == null)
                return null;

            return new Prospecta
            {
                consultadaReciente = prospectaDto.consultadaReciente,
                generoInconsistencias = prospectaDto.generoInconsistencias,
                nombreTitular = prospectaDto.nombreTitular,
                numeroIdentificacion = prospectaDto.numeroIdentificacion,
                resultado = prospectaDto.resultado,
                tipoIdentificacion = prospectaDto.tipoIdentificacion,
            };
        }

        /// <summary>
        /// Method ConsultInformacionService
        /// Check the user's commercial information
        /// </summary>
        /// <param name="document">User's document</param>
        /// <returns>Cifin user commercial information</returns>
        public ValidadorPlus ConsultInformacionService(string document)
        {
            var data = "";

            ParametrosConsultaDTO parmetrosDto = new ParametrosConsultaDTO
            {
                tipoIdentificacion = "1",
                numeroIdentificacion = document,
                motivoConsulta = "24",
                codigoInformacion = "153"
            };

            try
            {
                data = infComercialWse.consultaXml(parmetrosDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return FromXml<InfoValidadorPlus>(data)?.Tercero;
        }

        /// <summary>
        /// Method FromXml
        /// Provides XML format data
        /// </summary>
        /// <typeparam name="T">The provider type param</typeparam>
        /// <param name="xml">Provider XML string</param>
        /// <returns>XML object</returns>
        protected T FromXml<T>(String xml)
        {
            T returnedXmlClass = default(T);

            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                        returnedXmlClass = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                }
            }
            catch (Exception)
            {
            }

            return returnedXmlClass;
        }
    }
}

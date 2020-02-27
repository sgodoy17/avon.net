using IdentiGo.Domain.Entity.CIFIN;
using IdentiGo.Domain.Entity.General;
using IdentiGo.ExternalServices.WsProspectaPlusTest;
using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace IdentiGo.ExternalServices
{
    public class CifinExternalServiceTest
    {
        private ProspectaPlusWSService ProspectaWSService;

        public CifinExternalServiceTest(Config config)
        {
            try
            {
                SecurityToken securityToken = X509TokenProvider.CreateToken(StoreLocation.LocalMachine, StoreName.My, "info@enterdev.com.co", X509FindType.FindByIssuerName);
                MessageSignature messageSignature = new MessageSignature(securityToken);

                ProspectaWSService = new ProspectaPlusWSService
                {
                    Credentials = new NetworkCredential("310675", "Av0n10"),
                    Url = "https://cifinpruebas.asobancaria.com/ws/ProspectaPlusWebService/services/ProspectaPlus",
                };

                ProspectaWSService.RequestSoapContext.Security.Tokens.Add(securityToken);
                ProspectaWSService.RequestSoapContext.Security.Elements.Add(messageSignature);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Prospecta ConsultProspecta(string numberDocument)
        {
            var prospectaDto = ProspectaWSService.consultaProspecta("3058", numberDocument, "1");

            if (prospectaDto == null) return null;

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
    }
}

using IdentiGo.Domain.Entity.General;
using IdentiGo.Services.Master;
using RestSharp;
using System;
using System.Linq;
using System.Text;

namespace IdentiGo.ExternalServices
{
    public interface IIntrasoftService
    {
        void Send(string phone, string message);
    }

    public class IntrasoftService : IIntrasoftService
    {
        private string requestUrl = "http://107.20.199.106/restapi/sms/1/text/single";
        private string login = "pruebasenterdev";
        private string password = "enterdeV357";
        private Config config;
        private RestClient client;

        public IntrasoftService(IConfigService configService)
        {
            client = new RestClient(requestUrl);

            config = configService.GetAll().FirstOrDefault();
        }

        public void Send(string phone, string message)
        {
            try
            {
                byte[] concatenated = Encoding.ASCII.GetBytes(login + ":" + password);                
                string header = "Basic " + Convert.ToBase64String(concatenated);
                var request = new RestRequest(Method.POST);
                var codephone = 57 + phone;

                request.AddHeader("accept", "application/json");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", header);
                request.AddParameter("application/json", "{\"from\":\"890131\", \"to\":\""+codephone+"\",\"text\":\""+message+"\"}", ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);

                switch (response.Content.ToString())
                {
                    case "400":
                        throw new Exception("Error en Plataforma de SMS: Usuario inactivo o datos de acceso inválidos.");
                    case "401":
                        throw new Exception("Error en Plataforma de SMS: Línea no autorizada por la plataforma.");
                    case "402":
                        throw new Exception("Error en Plataforma de SMS: El contenido del mensaje es vacío.");
                    case "404":
                        throw new Exception("Error en Plataforma de SMS: Cupo de mensajes insuficientes.");
                    case "407 ":
                        throw new Exception("Error en Plataforma de SMS: No se realizó ninguna transacción.");
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}

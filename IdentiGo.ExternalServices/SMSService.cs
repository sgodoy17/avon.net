using IdentiGo.Domain.Entity.General;
using IdentiGo.Services.Master;
using RestSharp;
using System;
using System.Linq;

namespace IdentiGo.ExternalServices
{
    public interface ISMSService
    {
        void Send(string phone, string message);
    }

    public class SMSService : ISMSService
    {
        private string requestUrl = "http://www.ubiquom.com/send_mo/interdev.php";
        private string login = "interdev";
        private string password = "e6bfYFq4";
        private Config config;

        private RestClient client;

        public SMSService(IConfigService configService)
        {
            client = new RestClient(requestUrl);

            config = configService.GetAll().FirstOrDefault();
        }

        public void Send(string phone, string message)
        {
            try
            {
                var request = new RestRequest(Method.POST);

                request.AddParameter("to", 901);
                request.AddParameter("text", message);
                request.AddParameter("msisdn", phone);
                request.AddParameter("login", login);
                request.AddParameter("clave", password);

                var response = client.Execute(request);
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

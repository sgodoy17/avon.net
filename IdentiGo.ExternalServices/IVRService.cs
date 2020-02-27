using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Services.General;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using Microsoft.Win32;
using System.Globalization;
using System.Linq;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using Newtonsoft.Json;

namespace IdentiGo.ExternalServices
{
    public interface IIVRService
    {
        void Send(string number, int campid);
    }

    public class IVRService : IIVRService
    {
        private string requestUrl = "http://ws.ip-com.com";//"http://192.168.6.37:8081/api/verificator/GenerateValidation"; //"http://localhost:49539/api/verificator/IVRValidation";
        private string userId = "andes";
        private string userPW = "AndesBPO_Avon2017";
        private int productId = 2013;

        private RestClient client;
        public IVRService()
        {
            client = new RestClient(requestUrl);
        }

        public void Send(string number, int campid)
        {
            try
            {
                var response = GenerateBase(campid);

                if (string.IsNullOrEmpty(response?.base_id))
                    throw new Exception("No se pudo crear baseId para IVR");

                EnabledBaseId(response.base_id);

                SendNumber(response.base_id, $"57{number}", campid);
            }
            catch (Exception ex)
            {
                // catch exception and log it
                throw new Exception(ex.Message);
            }

        }

        ResponseIVR GenerateBase(int campid)
        {
            var dateInit = DateTime.Now;
            var dateEnd = DateTime.Now.AddHours(1);

            var request = new RestRequest(Method.POST);

            request.AddParameter("transaction_type", 901);
            request.AddParameter("user_id", userId);
            request.AddParameter("user_pw", userPW);
            request.AddParameter("campid", campid);
            request.AddParameter("product_id", productId);
            request.AddParameter("datetime_init", dateInit.ToString("yyyy-MM-dd"));
            request.AddParameter("datetime_end", dateEnd.ToString("yyyy-MM-dd"));
            request.AddParameter("daytime_init", dateInit.ToString("HH:mm:ss"));
            request.AddParameter("daytime_end", dateEnd.ToString("HH:mm:ss"));

            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<ResponseIVR>(ConverToJson(response.Content));
        }

        private string ConverToJson(string content)
        {
            return $"{{ '{ content.Replace("=", "':'").Replace("&", "','")}' }}";
        }

        void EnabledBaseId(string baseId)
        {
            var request = new RestRequest(Method.POST);
            request.AddParameter("transaction_type", "904");
            request.AddParameter("user_id", userId);
            request.AddParameter("user_pw", userPW);
            request.AddParameter("product_id", productId); 
            request.AddParameter("base_id", baseId);
            request.AddParameter("status", 1); 

            // execute the request
            client.Execute(request);
        }

        void SendNumber(string baseId, string number, int campid)
        {
            var request = new RestRequest(Method.POST);
            request.AddParameter("transaction_type", 905); 
            request.AddParameter("user_id", userId); 
            request.AddParameter("user_pw", userPW); 
            request.AddParameter("product_id", productId); 
            request.AddParameter("campid", campid); 
            request.AddParameter("base_id", baseId);

            request.AddFile("file", GenerateFile(number).ToArray(), "IVR.csv");

            client.Execute(request);
        }

        static MemoryStream GenerateFile(string number)
        {
            MemoryStream ms = new MemoryStream();

            TextWriter tw = new StreamWriter(ms);

            tw.WriteLine(number);

            tw.Flush();

            return ms;
        }
    }

    public class ResponseIVR
    {
        public string Response_code { get; set; }
        public string response_msg { get; set; }
        public string response_desc { get; set; }
        public string transaction_type { get; set; }
        public string base_id { get; set; }
    }
}

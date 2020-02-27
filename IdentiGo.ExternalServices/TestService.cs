using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Services.General;

namespace IdentiGo.ExternalServices
{
    public interface ITestService
    {
        string Send(Nomination user);
    }

    public class TestService : ITestService
    {
        public readonly INominationService UserValidationService;
        private string requestUrl = "http://192.168.6.37:8081/api/verificator/GenerateValidation"; //"http://localhost:49539/api/verificator/TestValidation";

        public TestService(
            INominationService userValidationService)
        {
            UserValidationService = userValidationService;
        }

        public string Send(Nomination user)
        {
            try
            {
                System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                request.Method = "POST";
                request.ContentType = "application/json";
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    string json = serializer.Serialize(user);
                    sw.Write(json);
                    sw.Flush();
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                return response.StatusDescription;
            }
            catch (Exception)
            {
                // catch exception and log it
                return null;
            }

        }
    }
}

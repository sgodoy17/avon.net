using IdentiGo.Data;
using IdentiGo.Data.Repository;
using IdentiGo.Data.UnitOfWork;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Enums;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace IdentiGo.Transversal.Utilities
{
    public static class FileUtil
    {
        public static MemoryStream GenerateFile()
        {
            IDbFactory db = new DbFactory();
            IUnitOfWork unitWork = new UnitOfWork(db);
            Repository<Nomination> _repository = new Repository<Nomination>(unitWork);

            MemoryStream ms = new MemoryStream();

            TextWriter tw = new StreamWriter(ms);

            tw.WriteLine("CodigoVerificacion|Campaña|Zona|Unidad|Cedula|Nombres|Apellidos|TelefonoPrincipal|NiveldeRiesgo|Fecha_HoraActualizacion");

            var date = DateTime.Today.AddDays(-38);

            foreach (var item in _repository.GetMany(x => x.DateUpdate.Date >= DateTime.Today.AddDays(-38) && x.DateUpdate.Date <= DateTime.Today))
            {
                if (item.CodeVerification == null || item.CampaingId == null || item.ZoneId == null)
                    continue;
                else if (item.RiskLevelId == null)
                    tw.WriteLine($"{string.Format("{0:00000000000}", item.CodeVerification)}|{Regex.Replace(item.Campaing.Number, @"[^\d]", "")}|{item.Zone.Number}|{item.Unit?.Number}|{item.Document}|{item.Name}|{item.LastName}|{item.PhoneNumber}|{""}|{item.DateUpdate}");
                else if (item.StateDocument == StateDocument.PagoContado)
                    tw.WriteLine($"{string.Format("{0:00000000000}", item.CodeVerification)}|{Regex.Replace(item.Campaing.Number, @"[^\d]", "")}|{item.Zone.Number}|{item.Unit?.Number}|{item.Document}|{item.Name}|{item.LastName}|{item.PhoneNumber}|{item.AvonRiskLevel}|{item.DateUpdate}");
                else
                    tw.WriteLine($"{string.Format("{0:00000000000}", item.CodeVerification)}|{Regex.Replace(item.Campaing.Number, @"[^\d]", "")}|{item.Zone.Number}|{item.Unit?.Number}|{item.Document}|{item.Name}|{item.LastName}|{item.PhoneNumber}|{item.RiskLevel.Level}|{item.DateUpdate}");
            }

            tw.Flush();

            //var fs = new FileStream(@"C:/Users/simon.godoy/Desktop/test.txt", FileMode.Create, FileAccess.ReadWrite);
            //var fs = new FileStream(@"C:/Users/eduard.sanchez/Desktop/test.txt", FileMode.Create, FileAccess.ReadWrite);

            //ms.WriteTo(fs);

            return ms;
        }

        public static void SendFileFTP()
        {
            try
            {
                var fileName = $"{FtpConfig.FileName}.txt";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FtpConfig.Directory + fileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(FtpConfig.UserName, FtpConfig.Password);

                var sourceStream = GenerateFile();
                byte[] fileContents = sourceStream.ToArray();
                sourceStream.Close();

                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static int GenerateCodeVerificacion(string document)
        {
            bool exits = true;
            int result;
            Random rng = new Random();

            do
            {
                const int maxValue = 999;
                string number = rng.Next(100, maxValue + 1).ToString("D3");
                long last = Convert.ToInt64(document) % (10);
                result = int.Parse(number + last.ToString());

                exits = ValidateCodeVerification(result, document);
            }
            while (exits);

            return result;
        }

        public static bool ValidateCodeVerification(int codeVerification, string document)
        {
            var exits = MainContext.Create().UserValidation.Where(x => x.CodeVerification == codeVerification && x.Document == document).FirstOrDefault();

            if (exits == null)
                return false;

            return true;
        }
    }
}

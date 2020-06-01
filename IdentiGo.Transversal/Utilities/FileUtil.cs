using IdentiGo.Data;
using IdentiGo.Data.Repository;
using IdentiGo.Data.UnitOfWork;
using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Enums;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace IdentiGo.Transversal.Utilities
{
    public static class FileUtil
    {
        public static string CreateTmpFile()
        {
            string fileName = string.Empty;

            try
            {
                fileName = Path.GetTempFileName();

                FileInfo fileInfo = new FileInfo(fileName);

                fileInfo.Attributes = FileAttributes.Temporary;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return fileName;
        }

        public static void UpdateTmpFile(string tmpFile)
        {
            try
            {
                StreamWriter streamWriter = File.AppendText(tmpFile);

                DateTime from = DateTime.Today.AddDays(-38);
                DateTime to = DateTime.Today;

                var nominations = MainContext.Create().UserValidation.Where(x => DbFunctions.TruncateTime(x.DateUpdate) >= from && DbFunctions.TruncateTime(x.DateUpdate) <= to);

                streamWriter.WriteLine("CodigoVerificacion|Campaña|Zona|Unidad|Cedula|Nombres|Apellidos|TelefonoPrincipal|NiveldeRiesgo|Fecha_HoraActualizacion");

                foreach (var item in nominations)
                {
                    if (item.CodeVerification == null || item.CampaingId == null || item.ZoneId == null)
                        continue;
                    else if (item.RiskLevelId == null)
                        streamWriter.WriteLine($"{string.Format("{0:00000000000}", item.CodeVerification)}|{Regex.Replace(item.Campaing.Number, @"[^\d]", "")}|{item.Zone.Number}|{item.Unit?.Number}|{item.Document}|{item.Name}|{item.LastName}|{item.PhoneNumber}|{""}|{item.DateUpdate}");
                    else if (item.StateDocument == StateDocument.PagoContado)
                        streamWriter.WriteLine($"{string.Format("{0:00000000000}", item.CodeVerification)}|{Regex.Replace(item.Campaing.Number, @"[^\d]", "")}|{item.Zone.Number}|{item.Unit?.Number}|{item.Document}|{item.Name}|{item.LastName}|{item.PhoneNumber}|{item.AvonRiskLevel}|{item.DateUpdate}");
                    else
                        streamWriter.WriteLine($"{string.Format("{0:00000000000}", item.CodeVerification)}|{Regex.Replace(item.Campaing.Number, @"[^\d]", "")}|{item.Zone.Number}|{item.Unit?.Number}|{item.Document}|{item.Name}|{item.LastName}|{item.PhoneNumber}|{item.RiskLevel.Level}|{item.DateUpdate}");
                }

                streamWriter.Flush();
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void DeleteTmpFile(string tmpFile)
        {
            try
            {
                if (File.Exists(tmpFile))
                {
                    File.Delete(tmpFile);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*public static MemoryStream GenerateFile()
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
        }*/

        public static void SendFileFTP()
        {
            try
            {
                string tmpFile = CreateTmpFile();

                UpdateTmpFile(tmpFile);
                
                var fileName = $"{FtpConfig.FileName}.txt";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FtpConfig.Directory + fileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(FtpConfig.UserName, FtpConfig.Password);

                /*var sourceStream = GenerateFile();
                byte[] fileContents = sourceStream.ToArray();
                sourceStream.Close();*/

                byte[] fileContents;

                using (StreamReader sourceStream = new StreamReader(tmpFile))
                {
                    fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                }

                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();
                DeleteTmpFile(tmpFile);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

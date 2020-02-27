using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Microsoft.Owin.Security.DataHandler.Encoder;

namespace IdentiGo.WebApi.Models
{
    public class AudienceModel
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string Document { get; set; }

        public string Plataform { get; set; }

        public string Data { get; set; }
    }

    public class AudienceLastModel
    {
        public int CompanyId { get; set; }

        public string Document { get; set; }
    }
}
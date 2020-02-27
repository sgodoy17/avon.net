using IdentiGo.Domain.Entity.Log;
using IdentiGo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentiGo.WebManagement.Areas.Log.Models
{
    public class SMSModel
    {
        [Display(Name = "Estado")]
        public TypeConsultCandidateResponse StateSMS { get; set; }

        public int Total { get; set; }

        public int Accept { get; set; }

        public int NotAccept { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime? DateStart { get; set; }

        [Display(Name = "Fecha de Fin")]
        public DateTime? DateEnd { get; set; }

        public IEnumerable<LogSMS> LogSMS { get; set; }
    }
}
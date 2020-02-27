using IdentiGo.Domain.Entity.IdentiGo;
using System;
using System.Collections.Generic;

namespace IdentiGo.WebManagement.Areas.Report.Models
{
    public class NominationHistoricModel
    {
        public IEnumerable<NominationHistoric> HictoricNomination { get; set; }

        public int? CodeVerification { get; set; }

        public DateTime DateStart { get; set; } = DateTime.Now;

        public DateTime DateEnd { get; set; } = DateTime.Now;
    }
}
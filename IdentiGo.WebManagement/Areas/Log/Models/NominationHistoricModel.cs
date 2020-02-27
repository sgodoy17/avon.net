using IdentiGo.Domain.Entity.IdentiGo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentiGo.WebManagement.Areas.Log.Models
{
    public class NominationHistoricModel
    {
        public IEnumerable<NominationHistoric> HictoricNomination { get; set; }

        public int? CodeVerification { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime? DateStart { get; set; }

        [Display(Name = "Fecha de Fin")]
        public DateTime? DateEnd { get; set; }

        [Display(Name = "Año")]
        public int? DateYear { get; set; }

        public int Pending { get; set; }

        public int Invalid { get; set; }

        public int Success { get; set; }

        public int Total { get; set; }
    }
}
using IdentiGo.Domain.Entity.IdentiGo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentiGo.WebManagement.Areas.General.Models
{
    public class NominationZoneModel
    {
        [Display(Name = "Documento")]
        public string Document { get; set; }

        [Display(Name = "Estado")]
        public TypeState TypeState { get; set; }

        [Display(Name = "Código de Campaña")]
        public Guid? CampaingId { get; set; }

        public Guid ZoneId { get; set; }

        public int Pending { get; set; }

        public int Invalid { get; set; }

        public int Success { get; set; }

        public int Total { get; set; }

        public bool Init { get; set; } = false;

        public IEnumerable<Nomination> Nomination { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime? DateStart { get; set; }

        [Display(Name = "Fecha de Fin")]
        public DateTime? DateEnd { get; set; }
    }
}
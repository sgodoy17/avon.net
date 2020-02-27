using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentiGo.Transversal.Model
{
    public class AdminReportModel
    {
        [Display(Name = "Tipo de Consulta")]
        public TypeConsulManagerZone TypeConsulAdmin { get; set; }

        [Display(Name = "Código de Campaña")]
        public Guid? CampaingId { get; set; }

        [Display(Name = "Empresaria")]
        public string Name { get; set; }

        [Display(Name = "Código de Zona")]
        public Guid? ZoneId { get; set; }

        public int Pending { get; set; }

        public int Invalid { get; set; }

        public int Success { get; set; }

        public int Total { get; set; }

        public List<ListIimpresarioReportModel> ListIimpresario { get; set; } = new List<ListIimpresarioReportModel>();

        public bool Init { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime? DateStart { get; set; }

        [Display(Name = "Fecha de Fin")]
        public DateTime? DateEnd { get; set; }

        [Display(Name = "Año")]
        public int? DateYear { get; set; }
    }
}


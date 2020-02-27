using IdentiGo.Domain.Entity.IdentiGo;
using System;
using System.ComponentModel.DataAnnotations;

namespace IdentiGo.WebManagement.Areas.Report.Models
{
    public class ExportDataViewModel
    {
        [Display(Name = "Código de Campaña")]
        public Guid? CampaingId { get; set; }

        [Display(Name = "Estado")]
        public TypeState TypeState { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime ExportDateStart { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Fin")]
        public DateTime ExportDateEnd { get; set; } = DateTime.Now;
    }
}
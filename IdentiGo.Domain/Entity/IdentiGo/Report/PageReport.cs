using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Entity.IdentiGo.Report
{
    [NotMapped]
    [Table("PAGEREPORT")]
    public class PageReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int CompanyId { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Fecha Final")]
        public DateTime? EndDate { get; set; }

        public List<PageReportInfo> PageReportInfo { get; set; }

    }

    [NotMapped]
    public class PageReportInfo
    {
        /// <summary>
        /// Nombre de la página
        /// </summary>
        [Display(Name = "Página")]
        public string Page { get; set; }

        /// <summary>
        /// Número de personas consultadas en página
        /// </summary>
        [Display(Name = "# Consultas")]
        public int NumberConsult { get; set; }

        /// <summary>
        /// Número de personas con información
        /// </summary>
        [Display(Name = "# Consultas con información")]
        public int NumberConsultInformacion { get; set; }

        /// <summary>
        /// Número de personas sin información
        /// </summary>
        [Display(Name = "# Consultas sin información")]
        public int NumberConsultNotInformacion { get; set; }

        [Display(Name = "# Consultas sin Responder")]
        public int NumberConsultNotFound { get; set; }

        /// <summary>
        /// % personas con información
        /// </summary>
        [Display(Name = "% Consultas con información")]
        public decimal PorcentInformaction { get; set; }

        /// <summary>
        /// % personas sin información
        /// </summary>
        [Display(Name = "% Consultas sin información")]
        public decimal PorcentNotInformation { get; set; }

        [Display(Name = "% Consultas sin Responer")]
        public decimal PorcentNotFound { get; set; }
    }
}

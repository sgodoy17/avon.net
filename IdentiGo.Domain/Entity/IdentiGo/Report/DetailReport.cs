using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Entity.IdentiGo.Report
{
    [NotMapped]
    [Table("DETAILREPORT")]
    public class DetailReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int CompanyId { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha Final")]
        public DateTime EndDate { get; set; }

        [Display(Name = "# de Cédula")]
        public string NumberDocument { get; set; }

        public Nomination UserValidation { get; set; }
        
    }
}

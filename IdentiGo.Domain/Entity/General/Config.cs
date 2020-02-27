using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Entity.General
{
    [Table("CONFIG")]
    public class Config
    {
        public Config()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Display(Name = "Destinatario (Email Error)")]
        public string EmailTo { get; set; }

        [Display(Name = "Asunto (Email Error)")]
        public string Subject { get; set; }
        
        [Display(Name = "Usuario (CIFN)")]
        public string UserCIFIN { get; set; }

        [Display(Name = "Clave (CIFIN)")]
        public string PasswordCIFIN { get; set; }
        
        [Display(Name = "# de intentos por Documento (Día)")]
        public int NumberIntentByDocument { get; set; }

        [Display(Name = "Total de Intentos")]
        public int NumberIntentByDocumentTotal { get; set; }

        [Display(Name = "Tiempo Bloqueado(Días)")]
        public int DayLokedDocument { get; set; }

        [Display(Name = "Tiempo de Validación")]
        public int TimeOutValidation { get; set; }

        [Display(Name = "Tiempo de Espera")]
        public int TimeOut { get; set; }

        [Display(Name = "Tiempo de Actualización (Día)")]
        public int TimeOutUpdate { get; set; }
        
        public string PageTemp { get; set; }

        [NotMapped]
        public IEnumerable<System.Web.Mvc.SelectListItem> PageList { get; set; }
    }
}

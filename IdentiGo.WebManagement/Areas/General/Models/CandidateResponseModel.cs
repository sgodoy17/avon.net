using IdentiGo.Domain.Entity.IdentiGo;
using IdentiGo.Domain.Entity.Log;
using IdentiGo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentiGo.WebManagement.Areas.General.Models
{
    public class CandidateResponseModel
    {
        public string Code { get; set; }

        public IEnumerable<NominationResponse> CandidateResponse { get; set; }

        [Display(Name ="Tipo de Consulta")]
        public TypeConsultCandidateResponse TypeConsult { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime DateStart { get; set; } = DateTime.Now;

        [Display(Name = "Fecha de Fin")]
        public DateTime DateEnd { get; set; } = DateTime.Now;
    }
}
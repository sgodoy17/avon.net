using IdentiGo.Domain.Entity.IdentiGo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentiGo.Transversal.Model
{
    public class ManagerZoneReportModel
    {
        [Display(Name = "Tipo de Consulta")]
        public TypeConsulManagerZone TypeConsulManagerZone { get; set; }

        [Display(Name = "Código de Campaña")]
        public Guid? CampaingId { get; set; }

        [Display(Name = "Empresaria")]
        public string Name { get; set; }

        public Guid ZoneId { get; set; }

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

    public class ListIimpresarioReportModel
    {
        [Display(Name = "Empresaria")]
        public string Code { get; set; }

        [Display(Name = "Pendientes")]
        public int Pending { get; set; }

        [Display(Name = "Invalidos")]
        public int Invalid { get; set; }

        [Display(Name = "Finalizados")]
        public int Success { get; set; }

        [Display(Name = "Total")]
        public int Total { get; set; }
        public Guid CodeId { get; internal set; }
    }

    public class CandidateReport
    {
        [Display(Name = "Zona")]
        public Guid? ZoneId { get; set; }

        [Display(Name = "Código de Campaña")]
        public Guid? CampaingId { get; set; }

        [Display(Name = "Tipo de Consulta")]
        public TypeConsulManagerZone TypeConsulManagerZone { get; set; }

        [Display(Name = "Empresaria")]
        public string Name { get; set; }

        public Guid UnitId { get; set; }

        public int Pending { get; set; }

        public int Invalid { get; set; }

        public int Success { get; set; }

        public int Total { get; set; }

        public List<Nomination> ListNomination { get; set; } = new List<Nomination>();

        [Display(Name = "Fecha de Inicio")]
        public DateTime? DateStart { get; set; }

        [Display(Name = "Fecha de Fin")]
        public DateTime? DateEnd { get; set; }

        [Display(Name = "Año")]
        public int? DateYear { get; set; }
    }

    public class EditNomination
    {
        [Display(Name = "Código de Campaña")]
        public string CodeCampaing { get; set; }

        [Display(Name = "Tipo de Consulta")]
        public TypeConsulManagerZone TypeConsulManagerZone { get; set; }

        [Display(Name = "Empresaria")]
        public string Code { get; set; }

        public Guid CodeId { get; set; }

        public Guid NominationId { get; set; }

        public Nomination Nomination { get; set; }
    }

    public enum TypeConsulManagerZone
    {
        [Display(Name = "Unidad")]
        Unit,
        [Display(Name = "Empresaria")]
        Impresario
    }
}


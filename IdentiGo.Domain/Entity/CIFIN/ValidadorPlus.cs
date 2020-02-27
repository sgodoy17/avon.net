using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using IdentiGo.Domain.Entity.IdentiGo;

namespace IdentiGo.Domain.Entity.CIFIN
{

    [Serializable, XmlRootAttribute(ElementName = "CIFIN", IsNullable = true), XmlType("CIFIN")]
    public class InfoValidadorPlus
    {
        [XmlElement("Tercero")]
        public ValidadorPlus Tercero { get; set; }
    }

    [Table("VALIDADORPLUS")]
    public class ValidadorPlus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [XmlIgnore]
        public Guid Id { get; set; }

        [ForeignKey("Nomination")]
        [XmlIgnore]
        public Guid NominationId { get; set; }

        [XmlIgnore]
        public Nomination Nomination { get; set; }

        public string IdentificadorLinea { get; set; }

        public string TipoIdentificacion { get; set; }

        public string CodigoTipoIndentificacion { get; set; }

        public string NumeroIdentificacion { get; set; }

        public string NombreTitular { get; set; }

        public string NombreCiiu { get; set; }

        public string LugarExpedicion { get; set; }

        public string FechaExpedicion { get; set; }

        public string Estado { get; set; }

        public string NumeroInforme { get; set; }

        public string EstadoTitular { get; set; }

        public string RangoEdad { get; set; }

        public string CodigoCiiu { get; set; }

        public string CodigoDepartamento { get; set; }

        public string CodigoMunicipio { get; set; }

        public string Fecha { get; set; }

        public string Hora { get; set; }

        public string Entidad { get; set; }

        public string RespuestaConsulta { get; set; }
    }
}

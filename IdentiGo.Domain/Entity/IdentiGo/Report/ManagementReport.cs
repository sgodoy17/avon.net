using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Entity.IdentiGo.Report
{
    [NotMapped]
    [Table("MANAGEMENTREPORT")]
    public class ManagementReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Empresa")]
        public int CompanyId { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Fecha Final")]
        public DateTime? EndDate { get; set; }

        #region Validation

        /// <summary>
        /// El número de personas que pasaron por el proceso
        /// </summary>
        [Display(Name = "# Personas")]
        public int NumberPerson { get; set; }

        /// <summary>
        /// El número de personas que pasaron por el proceso
        /// </summary>
        [Display(Name = "# Personas que pasaron")]
        public int NumberPersonSuccess { get; set; }

        /// <summary>
        /// El número de personas que fallaron el proceso
        /// </summary>
        [Display(Name = "# Personas que fallaron")]
        public int NumberPersonFailed { get; set; }

        /// <summary>
        /// Número de personas muertas consultadas en el proceso
        /// </summary>
        [Display(Name = "# Personas muertas")]
        public int NumberPersonDead { get; set; }

        /// <summary>
        /// Número de personas que se encontraban en la lista de control
        /// </summary>
        [Display(Name = "# Personas en lista de control")]
        public int NumberPersonCheckList { get; set; }

        #endregion

        #region Question

        [Display(Name = "# Personas")]
        public int NumberPersonAnswer { get; set; }

        /// <summary>
        /// El número de personas que contestaron correctamente, es decir, que cumplieron con la validación del parámetro de repuestas correctas para poder pasar el validador de identidad
        /// </summary>
        [Display(Name = "# Personas contestaron correctamente")]
        public int NumberPersonAnswerSuccess { get; set; }

        /// <summary>
        /// El número de personas que fallaron el proceso, es decir, que NO cumplieron con la validación del parámetro de repuestas correctas para poder pasar el validador de identidad.
        /// </summary>
        [Display(Name = "# Personas contestaron erroneamente")]
        public int NumberPersonAnswerFailed { get; set; }

        /// <summary>
        /// El % de respuestas buenas contestadas (en promedio) 3 de 4, 4 de 4, 2 de 4, 1 de 4. (no sé si sea viable dado que el parámetro puede cambiar – validar Eduard).
        /// </summary>
        [Display(Name = "Promedio Respuestas")]
        public decimal AverageAnswer { get; set; }

        /// <summary>
        /// El % de respuestas buenas contestadas (en promedio) 3 de 4, 4 de 4, 2 de 4, 1 de 4. (no sé si sea viable dado que el parámetro puede cambiar – validar Eduard).
        /// </summary>
        [Display(Name = "Promedio Respuestas correctas")]
        public decimal AverageAnswerSuccess { get; set; }

        /// <summary>
        /// El % de respuestas malas contestadas (no sé si sea viable dado que el parámetro puede cambiar – validar Eduard)
        /// </summary>
        [Display(Name = "Promedio Respuestas Incorrectas")]
        public decimal AverageAnswerFailed { get; set; }

        /// <summary>
        /// El número de intentos por persona, es decir, cuantas veces lo ejecutaron por el proceso durante el periodo de tiempo.
        /// </summary>
        [Display(Name = "# intentos por persona")]
        public decimal NumberTriedPerson { get; set; }

        #endregion

        #region Page

        [Display(Name = "# Personas Consultadas")]
        public int NumberPersonConsult { get; set; }

        /// <summary>
        /// Número de personas con información de páginas (aquellas que tenían al menos una página con información)
        /// </summary>
        [Display(Name = "# Personas con información (Páginas)")]
        public int NumberPersonInformationPage { get; set; }

        /// <summary>
        /// Número de personas sin información de páginas (aquellas que no tenían nada de información)
        /// </summary>
        [Display(Name = "# Personas sin información (Páginas)")]
        public int NumberPersonNotInformationPage { get; set; }

        /// <summary>
        /// Número de personas que no se pudieron consultar en páginas (aquellas personas que pasaron por el proceso pero las paginas estaba caída)
        /// </summary>
        [Display(Name = "# Personas que no se consultaron")]
        public int NumberPersonNotConsultPage { get; set; }

        #endregion

    }
}

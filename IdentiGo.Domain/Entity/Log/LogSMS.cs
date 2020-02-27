using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Entity.Log
{
    [Table("LOGSMS")]
    public class LogSMS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string PhoneAnswer { get; set; }

        public string Message { get; set; }

        public string CodeValidation { get; set; }

        public bool Input { get; set; }

        public string Document { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}

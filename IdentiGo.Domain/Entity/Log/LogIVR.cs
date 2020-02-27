using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentiGo.Domain.Entity.Log
{
    [Table("LOGIVR")]
    public class LogIVR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string PhoneAnswer { get; set; }

        public string Message { get; set; }

        public string CampId { get; set; }

        public bool Input { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}

using System;
using IdentiGo.Domain.Enums;

namespace IdentiGo.Domain.DTO
{
    public class UserValidationDto
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }
        public virtual UserDto User { get; set; }

        public int CompanyId { get; set; }
        public CompanyDto Company { get; set; }

        public string Document { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Sex { get; set; }

        public PageRespon Attorney { get; set; }

        public PageRespon ControllersShip { get; set; }

        public PageRespon Fosyga { get; set; }

        public PageRespon Policeman { get; set; }

        public PageRespon Registrar { get; set; }

        public PageRespon Ruaf { get; set; }

        public PageRespon Runt { get; set; }

        public PageRespon Sena { get; set; }

        public PageRespon Simit { get; set; }

        public PageRespon Sisben { get; set; }

        public bool Finish { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdate { get; set; }

        public State State { get; set; }
    }

    public class ResponMessage
    {
        string Result { get; set; }

        string Message { get; set; }

        string PhoneAnswer { get; set; }
    }
}

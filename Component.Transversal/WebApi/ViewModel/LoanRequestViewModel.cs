using System;

namespace Component.Transversal.WebApi.ViewModel
{

    public class LoanRequestViewModel
    {
        public Guid IdUser { get; set; }

        public Guid IdBicycle { get; set; }

        public Guid IdBiclycleStation { get; set; }

        public DateTime LoadTime { get; set; }
    }

}
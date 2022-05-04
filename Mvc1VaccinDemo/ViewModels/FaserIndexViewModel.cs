using System.Collections.Generic;

namespace Mvc1VaccinDemo.ViewModels
{
    public class FaserIndexViewModel 
    {
        public string q { get; set; }

        public List<FasViewModel> Faser { get; set; } = new List<FasViewModel>();


        public class FasViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

    }
}
using System;

namespace Mvc1VaccinDemo.ViewModels
{
    public class SelectVaccineViewModel
    {
        public string SelectedVaccinName { get; set; }
        public int NrOrdered { get; set; }
        public DateTime FirstDeliveryDate { get; set; }
        public int NrInFirstDelivery { get; set; }


    }
}
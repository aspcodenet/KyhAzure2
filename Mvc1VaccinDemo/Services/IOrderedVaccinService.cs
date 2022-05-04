using System;

namespace Mvc1VaccinDemo.Services
{
    public interface IOrderedVaccinService
    {
        OrderedVaccinGetTotalResult GetTotal(string vaccinName);
    }

    public class OrderedVaccinGetTotalResult
    {
        public int NrOrdered { get; set; }
        public DateTime FirstDeliveryDate { get; set; }
        public int NrInFirstDelivery { get; set; }
    }
}
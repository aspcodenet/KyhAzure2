using System;
using System.Collections.Generic;

namespace Mvc1VaccinDemo.Services
{
    class OrderedVaccinService : IOrderedVaccinService
    {
        private static Dictionary<string, OrderedVaccinGetTotalResult> values = new Dictionary<string, OrderedVaccinGetTotalResult>();
        private static Random _rnd = new Random();
        public OrderedVaccinGetTotalResult GetTotal(string vaccinName)
        {
            OrderedVaccinGetTotalResult result;
            if (!values.TryGetValue(vaccinName, out result))
            {
                result = new OrderedVaccinGetTotalResult();
                result.NrOrdered = _rnd.Next(1, 50) * 500;
                result.NrInFirstDelivery = _rnd.Next(1, result.NrOrdered / 2);
                result.FirstDeliveryDate = DateTime.Now.AddDays(_rnd.Next(0, 10));
                values[vaccinName] = result;
            }
            System.Threading.Thread.Sleep(3000);
            return result;
        }
    }
}
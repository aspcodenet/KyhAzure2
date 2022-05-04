using System;
using Mvc1VaccinDemo.Data;

namespace Mvc1VaccinDemo.ViewModels
{
    /*
     *
     Antal gjorda vaccineringar
Antal godkända vaccin
Antal personer
Antal suppliers
Namn på senaste vaccinet som godkänts
Datum för     senaste vaccinet som godkänts
     *
     */
    public class HomeIndexViewModel  //DTO
    {
        public int AntalGjordaVaccineringar { get; set; }
        public int AntalGodkandaVaccin { get; set; }
        public int NumberOfSuppliers { get; set; }
        public int NumberOfPersons { get; set; }
        public DateTime SenastGodkand { get; set; }
        public string SenastGodkandVaccin { get; set; }
    }
}
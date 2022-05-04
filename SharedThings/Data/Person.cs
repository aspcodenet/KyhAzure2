using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mvc1VaccinDemo.Data;

namespace SharedThings.Data
{
    public class Person
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string PersonalNumber { get; set; }

        [MaxLength(100)]
        public string EmailAddress { get; set; }
        [MaxLength(100)]
        public string StreetAddress { get; set; }
        [MaxLength(100)]
        public string City { get; set; }

        public int PostalCode { get; set; }

        public VaccineringsFas VaccineringsFas { get; set; }

        public DateTime ?PreliminaryNextVaccinDate { get; set; }


        public List<Vaccinering> Vaccinering { get; set; }

        [MaxLength(512)]
        public string Description { get; set; }


    }
}
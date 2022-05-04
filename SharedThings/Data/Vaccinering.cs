using System;

namespace SharedThings.Data
{
    public class Vaccinering
    {
        public int Id { get; set; } //

        public Vaccin Vaccin { get; set; } //Covid-19 Vaccine Moderna
        public DateTime Timestamp { get; set; } //2021-01-19 kl 09:12

        public string HealthCareCentralName { get; set; } // Nacka Vårdcentral
    }
}
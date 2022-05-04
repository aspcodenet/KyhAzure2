using System;
using System.ComponentModel.DataAnnotations;

namespace SharedThings.Data
{


    public class Vaccin
    {
        public int Id { get; set; }

        public enum VaccinType
        {
            Unknown,    
            mRNA,
            Vector
        }

        public Supplier Supplier { get; set; }

        [MaxLength(50)]
        public string Namn { get; set; }
        public DateTime ?EuOkStatus { get; set; } //Om NULL = inte godkänt  2020-01-07
        public VaccinType Type { get; set; }


        //Will be removed soon
        public string Comment { get; set; }
        public int AntalDoser { get; set; }
    }
}
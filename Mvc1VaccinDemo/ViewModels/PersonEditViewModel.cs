using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mvc1VaccinDemo.Data;

namespace Mvc1VaccinDemo.ViewModels
{
    public class PersonEditViewModel 
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string PersonalNumber { get; set; }

        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(100)]
        public string StreetAddress { get; set; }
        [MaxLength(100)]
        public string City { get; set; }

        [Range(10000,99999)]
        public int PostalCode { get; set; }


        [DataType(DataType.Date)]
        public DateTime? PreliminaryNextVaccinDate { get; set; }


        public int SelectedVaccineringsFasId { get; set; }
        public List<SelectListItem> AllaVaccineringsFaser { get; set; }
        public string Description { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mvc1VaccinDemo.ViewModels
{

    public class NotCurrentHourAttribute : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int hour = Convert.ToInt32(value);
            if (hour == DateTime.Now.Hour)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-notcurrenthour", ErrorMessage);
        }
    }

    public class VaccinNewViewModel 
    {
        [Range(1,100000, ErrorMessage = "Välj en din dumbom")]
        public int SelectedSupplierId { get; set; }
        public List<SelectListItem> AllSuppliers { get; set; } = new List<SelectListItem>();


        [MaxLength(50)]
        [Remote("ValidateNoDuplicateName","Vaccin")]
        public string Namn { get; set; }
        public DateTime? EuOkStatus { get; set; }

        public int Type { get; set; }

        public List<SelectListItem> Types { get; set; } = new List<SelectListItem>();

        [Required]
        [MaxLength(1000)]
        public string Comment { get; set; }

        [Range(1, 1000000)]// 
        public int AntalDoser { get; set; }

        [Range(0, 23)]// 
        [NotCurrentHour(ErrorMessage = "Du får inte skriva en aktuellt klockslag")]
        public int Hour { get; set; }
    }
}
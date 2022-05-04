using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mvc1VaccinDemo.ViewModels
{
    public class VaccineringsFasNewViewModel
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public string Comment { get; set; }

        [Range(1, 1000000)]
        public int SelectedMyndighetsId { get; set; }

        public List<SelectListItem> AllaMyndigheter { get; set; }


    }
}
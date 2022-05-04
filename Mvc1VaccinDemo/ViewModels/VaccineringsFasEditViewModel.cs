using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace Mvc1VaccinDemo.ViewModels
{
    public class VaccineringsFasEditViewModel 
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Skriv in ett namn dumsnut")]
        [MaxLength(30, ErrorMessage = "Maxlängd uppnådd")]
        [MinLength(2, ErrorMessage = "Du har völ ett längre namn än så???")]
        public string Name { get; set; }

        
        [Required(ErrorMessage = "Skriv in kommentar tack")]
        public string Comment { get; set; }

        [Range(1,1000000)]
        public int SelectedMyndighetsId { get; set; }

        public List<SelectListItem> AllaMyndigheter { get; set; }



    }
}
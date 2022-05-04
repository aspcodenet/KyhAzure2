using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mvc1VaccinDemo.ViewModels
{
    public class VaccinEditViewModel 
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public int Type { get; set; }
        public List<SelectListItem> Types { get; set; } = new List<SelectListItem>();


        public int SelectedSupplierId { get; set; }
        public List<SelectListItem> AllSuppliers { get; set; } = new List<SelectListItem>();
        //  SelectListItem   key     value
        //                   1      AstraZeneca och Oxford University
        //                   2       Pfizer/BioNTech


        [MaxLength(50)]
        public string Namn { get; set; }
        

        [DisplayName("Test123")]
        [DataType(DataType.Date)]
        public DateTime? EuOkStatus { get; set; }
        //



        [Required]
        [MaxLength(1000)]
        public string Comment { get; set; }
        
        [Range(1,1000000)]// 
        public int AntalDoser { get; set; }

    }
}
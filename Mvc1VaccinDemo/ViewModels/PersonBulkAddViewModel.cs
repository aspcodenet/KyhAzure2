using System.ComponentModel.DataAnnotations;

namespace Mvc1VaccinDemo.ViewModels
{
    public class PersonBulkAddViewModel
    {
        [Range(1,20)]
        public int Antal { get; set; }
    }
}
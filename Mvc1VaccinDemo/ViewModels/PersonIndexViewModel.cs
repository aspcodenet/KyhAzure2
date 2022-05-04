using System.Collections.Generic;

namespace Mvc1VaccinDemo.ViewModels
{
    public class PersonIndexViewModel 
    {
        public string q { get; set; }
        public List<PersonViewModel> Personer { get; set; } = new List<PersonViewModel>();
        public string SortOrder { get; set; }
        public string SortField { get; set; }
        public string OppositeSortOrder { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}
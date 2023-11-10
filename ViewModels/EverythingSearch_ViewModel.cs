using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class EverythingSearch_ViewModel
    {
        [MaxLength(100)]
        public string? Keyword { get; set; }
        public int MinTargetPrice { get; set; }
        public int MaxTargetPrice { get; set; }
        public bool SearchOnlyProjects { get; set; }
        public bool SearchOnlyUsers { get; set; }
    }
}

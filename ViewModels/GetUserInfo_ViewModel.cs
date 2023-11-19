using ApplicationY.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class GetUserInfo_ViewModel
    {
        public int Id { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? UserName { get; set; }   
        public string? PseudoName { get; set; }
        public string? SearchName { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public string? Link1Tag { get; set; }
        public string? Link2Tag { get; set; }
        public string? Link1 { get; set; }
        public string? Link2 { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsCompany { get; set; }
        public string? CountryFullName { get; set; }
        public int? ProjectsCount { get; set; }
        public int SubscribersCount { get; set; }
        public Country? Country { get; set; }
        public List<Project>? Projects { get; set; }
    }
}

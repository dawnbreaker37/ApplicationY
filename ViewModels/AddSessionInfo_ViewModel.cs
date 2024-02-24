using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class AddSessionInfo_ViewModel
    {
        public int Id { get; set; }
        public string? LocationName { get; set; }
        public string? DeviceName { get; set; }
        public bool IsClosed { get; set; }
        public DateTime SessionDate { get; set; }
        [Required(ErrorMessage = "Session current location's required")]
        public string? Location { get; set; }
        [Required(ErrorMessage = "User information's required")]
        public int UserId { get; set; }
    }
}

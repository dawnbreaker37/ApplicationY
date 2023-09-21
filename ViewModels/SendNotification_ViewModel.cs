using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class SendNotification_ViewModel
    {
        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }
        [Required]
        [MaxLength(400)]
        public string? Description { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}

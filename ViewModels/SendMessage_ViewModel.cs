
using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class SendMessage_ViewModel
    {
        [Required(ErrorMessage = "Enter message text")]
        [MaxLength(2500, ErrorMessage = "Max length of message text is 2500 characters")]
        [MinLength(6, ErrorMessage = "Min length of message is 6 characters")]
        public string? Text { get; set; }
        public DateTime SentAd { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}

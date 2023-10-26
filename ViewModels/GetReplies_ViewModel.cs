using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class GetReplies_ViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter message text")]
        [MaxLength(2500, ErrorMessage = "Max length of message text is 2500 characters")]
        [MinLength(1, ErrorMessage = "Min length of message is 1 character")]
        public string? Text { get; set; }
        public DateTime SentAt { get; set; }
        [Required]
        public int? UserId { get; set; }
        [Required]
        public int CommentId { get; set; }
        public string? Username { get; set; }
    }
}

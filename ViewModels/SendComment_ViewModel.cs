using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class SendComment_ViewModel
    {
        [Required(ErrorMessage = "Enter your comment's text")]
        [MaxLength(1500, ErrorMessage = "Your comment is too large (max length: 1500 symbols)")]
        public string? Text { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProjectId { get; set; }
    }
}

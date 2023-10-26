using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsChecked { get; set; }
        [ForeignKey("Message")]
        public int? CommentId { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }
        public Comment? Comment { get; set; }
    }
}

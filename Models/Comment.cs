using System.ComponentModel.DataAnnotations;

namespace ApplicationY.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [MaxLength(1500)]
        public string? Text { get; set; }
        public DateTime SentAt { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public bool IsRemoved { get; set; }
        public User? User { get; set; }
        public Project? Project { get; set; }
        public List<Reply>? Replies { get; set; }
    }
}

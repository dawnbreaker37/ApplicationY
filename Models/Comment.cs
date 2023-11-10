using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [MaxLength(1500)]
        public string? Text { get; set; }
        public DateTime SentAt { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public bool IsRemoved { get; set; }
        public User? User { get; set; }
        public Project? Project { get; set; }
        public List<Reply>? Replies { get; set; }
    }
}

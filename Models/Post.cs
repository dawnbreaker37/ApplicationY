using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Post
    {
        public int Id { get; set; }
        [MaxLength(2500)]
        public string? Text { get; set; }
        [ForeignKey("Project")]
        public int? LinkedProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsPrivate { get; set; }
        public bool AllowMentions { get; set; }
        public Project? Project { get; set; }
        public User? User { get; set; }
        public List<LikedPost>? LikedPosts { get; set; }
        public List<Mention>? Mentions { get; set; }
    }
}

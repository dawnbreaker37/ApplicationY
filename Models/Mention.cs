using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Mention
    {
        public int Id { get; set; }
        [MaxLength(450)]
        public string? Text { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRemoved { get; set; } 
        [ForeignKey("Post")]
        public int? PostId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public Post? Post { get; set; }
        public User? User { get; set; }

    }
}

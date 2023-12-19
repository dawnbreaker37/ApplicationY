using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class LikedPost
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        [ForeignKey("Post")]
        public int? PostId { get; set; }
        public bool IsRemoved { get; set; }
        public User? User { get; set; }
        public Post? Post { get;set; }
    }
}

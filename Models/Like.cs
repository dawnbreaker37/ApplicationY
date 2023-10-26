using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Like
    {
        public int Id { get; set; }
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public bool IsRemoved { get; set; }
        public User? User { get; set; }
        public Project? Project { get; set; }
    }
}

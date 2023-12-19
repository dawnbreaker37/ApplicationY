using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Purge
    {
        public int Id { get; set; }
        [MaxLength(900)]
        public string? Description { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public int SenderId { get; set; }
        public bool IsClosed { get; set; }
        public DateTime SentAt { get; set; }
        public User? User { get; set; }
        public Project? Project { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Message
    {
        public int Id { get; set; }
        [MaxLength(2500)]
        public string? Text { get; set; }
        [ForeignKey("User")]
        public int? SenderId { get; set; }
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public int UserId { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsChecked { get; set; }
        public DateTime SentAt { get; set; }
        public Project? Project { get; set; }
        public User? User { get; set; }
    }
}

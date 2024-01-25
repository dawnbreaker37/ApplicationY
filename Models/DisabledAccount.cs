using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class DisabledAccount
    {
        public int Id { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public DateTime DisabledAt { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public int SenderId { get; set; }
        public User? User { get; set; }
    }
}

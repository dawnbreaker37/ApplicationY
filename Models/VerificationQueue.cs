using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class VerificationQueue
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public User? User { get; set; }

    }
}

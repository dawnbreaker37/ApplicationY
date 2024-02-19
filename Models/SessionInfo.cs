using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class SessionInfo
    {
        public int Id { get; set; }
        public DateTime SessionDate { get; set; }
        public string? Location { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

    }
}

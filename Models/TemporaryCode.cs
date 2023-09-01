using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class TemporaryCode
    {
        public int Id { get; set; }
        [MaxLength(75)]
        public string? Title { get; set; }
        public string? Code { get; set; }
        public DateTime? SentDate { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}

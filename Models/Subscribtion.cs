using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Subscribtion
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public int SubscriberId { get; set; }
        public bool IsRemoved { get; set; }
        public User? User { get; set; }
    }
}

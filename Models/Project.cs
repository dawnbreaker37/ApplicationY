using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [MaxLength(2500)]
        public string? Description { get; set; }
        public string? TextPart1 { get; set; }
        public string? TextPart2 { get; set; }
        public string? TextPart3 { get; set; }
        public string? Link1 { get; set; }
        public string? Link2 { get; set; }
        public string? YoutubeLink { get; set; }
        public int PriceOfProject { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}

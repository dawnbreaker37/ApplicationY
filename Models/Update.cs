using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Update
    {
        public int Id { get; set; }
        [MaxLength(75)]
        public string? Title { get; set; }
        [MaxLength(750)]
        public string? Description { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsRemoved { get; set; }
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}

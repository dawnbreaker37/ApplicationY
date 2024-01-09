using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Audio
    {
        public int Id { get; set; }
        [MaxLength(40)]
        public string? Title { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsRemoved { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}

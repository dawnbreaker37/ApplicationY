using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class DisabledProject
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime DisabledAt { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}

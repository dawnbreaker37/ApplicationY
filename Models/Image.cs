using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public bool IsRemoved { get; set; }
        public Project? Project { get; set; }
    }
}

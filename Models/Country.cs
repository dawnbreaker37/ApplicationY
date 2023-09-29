using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ISO { get; set; }
        public List<User>? Users { get; set; }
    }
}

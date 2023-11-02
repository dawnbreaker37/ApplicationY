using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class EditLinks_ViewModel
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Link1Tag { get; set; }
        [MaxLength(50)]
        public string? Link2Tag { get; set; }
        public string? Link1 { get; set; }
        public string? Link2 { get; set; }
    }
}

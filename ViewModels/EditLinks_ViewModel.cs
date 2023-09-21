using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class EditLinks_ViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Link tag is required")]
        [MaxLength(50)]
        public string? Link1Tag { get; set; }
        [Required(ErrorMessage = "Link tag is required")]
        [MaxLength(50)]
        public string? Link2Tag { get; set; }
        [Required]
        public string? Link1 { get; set; }
        [Required]
        public string? Link2 { get; set; }
    }
}

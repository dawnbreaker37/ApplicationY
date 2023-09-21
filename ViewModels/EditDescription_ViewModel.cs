
using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class EditDescription_ViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Description value is required")]
        [MaxLength(600, ErrorMessage = "The entered description is too long")]
        public string? Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class EditAccount_ViewModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? ProfilePhoto { get; set; }
        [Required(ErrorMessage = "Pseudoname is required")]
        [MaxLength(60, ErrorMessage = "Entered pseudoname is too long")]
        [MinLength(4, ErrorMessage = "Entered pseudoname is too short")]
        public string? PseudoName { get; set; }
        [Required(ErrorMessage = "Searchname is required")]
        [MaxLength(12, ErrorMessage = "Entered searchname is too long")]
        [MinLength(4, ErrorMessage = "Entered searchname is too short")]
        public string? SearchName { get; set; }
        [MaxLength(600, ErrorMessage = "This description is too large. Please, enter a shorter descripton (up to 600 characters)")]
        public string? Description { get; set; }
        [Required]
        public string? RealSearchName { get; set; }

    }
}

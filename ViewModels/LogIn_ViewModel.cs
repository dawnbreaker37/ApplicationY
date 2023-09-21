using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class LogIn_ViewModel
    {
        [MaxLength(90)]
        [Required(ErrorMessage = "Username or email are required")]
        public string? UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter password")]
        public string? Password { get; set; }
    }
}

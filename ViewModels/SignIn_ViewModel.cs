using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class SignIn_ViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "E-Mail is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [MinLength(4, ErrorMessage = "Username can't contain less than 4 digits")]
        [MaxLength(24, ErrorMessage = "Username can't contain more than 24 digits")]
        public string? UserName { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(24, ErrorMessage = "Reccomended length of password is no more than 24 digits")]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords are not equal to each other")]
        public string? PasswordConfirm { get; set; }
    }
}

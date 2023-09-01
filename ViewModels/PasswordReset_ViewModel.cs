using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class PasswordReset_ViewModel
    {
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }
        [MinLength(4, ErrorMessage = "Username is invalid")]
        [MaxLength(24, ErrorMessage = "Username is invalid")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Reserve code is required")]
        [MinLength(6, ErrorMessage = "Enter 6-digit code")]
        [MaxLength(6, ErrorMessage = "Enter 6-digit code")]
        public string? ReserveCode { get; set; }
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Your new password must contain 8 digits")]
        [MaxLength(24, ErrorMessage = "Your new password length can't be more than 24 digits")]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password aren't equal to each other")]
        public string? PasswordConfirm { get; set; }
        public bool CreateSecurePassword { get; set; }
    }
}

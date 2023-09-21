using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class ChangeEmail_ViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="Current email is required")]
        public string? Email { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "New email is required")]
        public string? NewEmail { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Min length of password is 8 digits")]
        [MaxLength(24, ErrorMessage = "Max length of password id 24 digits")]
        public string? Password { get; set; }
        [MaxLength(6, ErrorMessage = "Max length of reserve code is 6 digits")]
        public string? ReserveCode { get; set; }
        [MaxLength(9, ErrorMessage = "Verification code must have 9 digits")]
        public string? VerifcationCode { get; set; }
    }
}

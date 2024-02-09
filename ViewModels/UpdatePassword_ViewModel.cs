using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class UpdatePassword_ViewModel
    {
        [MinLength(6)]
        [MaxLength(6)]
        public string? ReserveCode { get; set; }
        [Required]
        public string? Token { get; set; }
        [Required]
        public string? EmailOrUsername { get; set; }
        [Required]
        public bool SearchByUsername { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(24)]
        [MinLength(8)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(24)]
        [MinLength(8)]
        [Compare("Password")]
        public string? PasswordConfirm { get; set; }
    }
}

using ApplicationY.Models;
using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class PasswordChange_ViewModel
    {
        public int UserId { get; set; }
        public User? UserInfo { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(24)]
        [MinLength(8)]
        public string? CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(24)]
        [MinLength(8)]
        public string? NewPassword { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(24)]
        [MinLength(8)]
        public string? ConfirmPassword { get; set; }
        [MinLength(6)]
        [MaxLength(6)]
        public string? ReserveCode { get; set; }
    }
}

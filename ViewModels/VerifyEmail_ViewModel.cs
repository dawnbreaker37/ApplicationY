using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class VerifyEmail_ViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        public string? Token { get; set; }
        [Required]
        public string? Code { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApplicationY.Models
{
    public class User : IdentityUser<int>
    {
        [MinLength(4)]
        [MaxLength(24)]
        public string? PseudoName { get; set; }
        [MinLength(4)]
        [MaxLength(12)]
        public string? SearchName { get; set; }
        [MinLength(6)]
        [MaxLength(6)]
        public string? ReserveCode { get; set; }
        public List<TemporaryCode>? TemporaryCodes { get; set; }
    }
}

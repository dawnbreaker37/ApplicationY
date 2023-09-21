using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class EditPersonalInfo_ViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool IsCompany { get; set; }
    }
}

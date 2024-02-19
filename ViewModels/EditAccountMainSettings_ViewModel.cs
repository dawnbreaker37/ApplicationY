using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class EditAccountMainSettings_ViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool AreMessagesDisabled { get; set; }
        [Required]
        public bool AreCommentsDisabled { get; set; }
    }
}

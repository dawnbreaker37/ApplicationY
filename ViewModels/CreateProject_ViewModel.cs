using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class CreateProject_ViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title name for project is required")]
        [MinLength(3, ErrorMessage = "Min. length of project title is 3 digits")]
        [MaxLength(40, ErrorMessage = "Max. length of project title is 40 digits")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Description for the project is required")]
        [MinLength(40, ErrorMessage = "Description requires to contain more than 40 digits")]
        [MaxLength(1600, ErrorMessage = "Title description of project can't be larger that 1600 digits")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "At least a small description of your project is required")]
        [MinLength(120, ErrorMessage = "Describe your project in at least 120 digits")]
        [MaxLength(6000, ErrorMessage = "Full description of your project can't contain more than 6000 digits")]
        public string? TextPart { get; set; }
        public string? Link1 { get; set; }
        public string? Link2 { get; set; }
        public int Duration { get; set; } = 0;
        public DateTime? Deadline { get; set; }
        public string? YoutubeLink { get; set; }
        [Required(ErrorMessage = "Select the category of your project")]
        public int CategoryId { get; set; }
        [DataType(DataType.Url)]
        public string? DonationLink { get; set; }
        [DataType(DataType.Currency)]
        public int ProjectPrice { get; set; }
        [MaxLength(400, ErrorMessage = "Please, try to describe your price change annotation shorter")]
        public string? TargetPriceChangeAnnotation { get; set; }
        public int CurrentPrice { get; set; }
        [Required]
        public int UserId { get; set; }
        public IFormFileCollection? ImagesFiles { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? Images { get; set; }
        public string? Audios { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationY.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [MaxLength(2500)]
        public string? Description { get; set; }
        public string? TextPart1 { get; set; }
        public string? TextPart2 { get; set; }
        public string? TextPart3 { get; set; }
        public string? Link1 { get; set; }
        public string? Link2 { get; set; }
        public string? YoutubeLink { get; set; }
        //public string? DonationLink { get; set; }
        public bool IsBudget { get; set; }
        [MaxLength(1200)]
        public string? DonationRules { get; set; }
        public DateTime? Deadline { get; set; }
        public int TargetPrice { get; set; }
        public bool IsPinned { get; set; }
        [MaxLength(400)]
        public string? PriceChangeAnnotation { get; set; }
        public int PastTargetPrice { get; set; }
        public int Views { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public int MainPhotoId { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsClosed { get; set; }
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }
        public Category? Category { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Update>? Updates { get; set; }
        public List<Image>? Images { get; set; }
        public List<Audio>? Audios { get; set; }
        public List<Post>? Posts { get; set; }
        public List<Purge>? Purges { get; set; }
        [NotMapped]
        public string? CategoryName { get; set; }
        [NotMapped]
        public string? CategoryDescription { get; set; }
        [NotMapped]
        public string? UserName { get; set; }
        [NotMapped]
        public int Duration { get; set; }
    }
}

using ApplicationY.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class GetPost_ViewModel
    {
        public int Id { get; set; }
        [MaxLength(2500)]
        public string? Text { get; set; }
        public int? LinkedProjectId { get; set; }
        public bool AllowMentions { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsLiked { get; set; }
        public int LikedCount { get; set; } 
        public string? CreatorName { get; set; }
        public Project? Project { get; set; }
        public DateTime? ProjectCreatedAt { get; set; }
    }
}

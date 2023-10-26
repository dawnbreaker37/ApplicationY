using ApplicationY.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplicationY.ViewModels
{
    public class GetMessages_ViewModel
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public bool IsRemoved { get; set; }
        public int? SenderId { get; set; }
        public bool IsChecked { get; set; }
        public int RepliesCount { get; set; }
        public DateTime SentAt { get; set; }
        public string? SenderName { get; set; }
        public string? ProjectName { get; set; }
    }
}

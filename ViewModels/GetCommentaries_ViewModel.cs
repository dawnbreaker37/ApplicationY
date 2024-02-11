namespace ApplicationY.ViewModels
{
    public class GetCommentaries_ViewModel
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public int? SenderId { get; set; }
        public string? SenderName { get; set; }
        public int? ProjectId { get; set; }
        public bool IsEdited { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime SentAt { get; set; }
        public int RepliesCount { get; set; }
    }
}

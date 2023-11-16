namespace ApplicationY.ViewModels
{
    public class GetLikedProjects_ViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CreatorName { get; set; }
        public string? CreatorSearchName { get; set; }
        public DateTime LastUpdateAtDate { get; set; }
        public DateTime CreateAtDate { get; set; }
        public int? ProjectId { get; set; }
        public int Views { get; set; }
    }
}

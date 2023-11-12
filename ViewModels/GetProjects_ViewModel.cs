namespace ApplicationY.ViewModels
{
    public class GetProjects_ViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int TargetPrice { get; set; }
        public int PastTargetPrice { get; set; }
        public int Views { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryIcon { get; set; }
        public string? TextPart1 { get; set; }
        public string? TextPart2 { get; set; }
        public string? TextPart3 { get; set; }
        public string? PriceChangeAnnotation { get; set; }
        public bool IsClosed { get; set; }
        public bool IsRemoved { get; set; }
        public string? Link1 { get; set; }
        public string? Link2 { get; set; }
        public string? YoutubeLink { get; set; }
    }
}

namespace ApplicationY.ViewModels
{
    public class GetReleaseNotes_ViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? ProjectName { get; set; }
    }
}

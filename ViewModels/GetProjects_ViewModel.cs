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
    }
}

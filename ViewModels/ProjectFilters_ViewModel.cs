namespace ApplicationY.ViewModels
{
    public class ProjectFilters_ViewModel
    { 
        public int UserId { get; set; }
        public int SenderId { get; set; }
        public bool GetAdditionalInfo { get; set; }
        public int MinTargetPrice { get; set; }
        public int MaxTargetPrice { get; set; }
        public string? Keyword { get; set; }
        public bool IsLocked { get; set; }
    }
}

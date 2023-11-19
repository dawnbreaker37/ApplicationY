namespace ApplicationY.ViewModels
{
    public class GetSubscribtions_ViewModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int SubscriberId { get; set; }
        public bool IsRemoved { get; set; }
        public string? UserName { get; set; }
        public string? UserSearchName { get; set; }
    }
}

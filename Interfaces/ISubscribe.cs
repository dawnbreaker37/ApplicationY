using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface ISubscribe
    {
        public Task<bool> IsUserSubscribed(int UserId, int SubscriberId);
        public Task<bool> SubscribeAsync(int UserId, int SubscriberId);
        public Task<bool> DeclineSubscribtionAsync(int UserId, int SubscriberId);
        public IQueryable<GetSubscribtions_ViewModel>? GetSubscribtions(int UserId);
        public Task<int> GetSubscribersCount(int UserId);
    }
}

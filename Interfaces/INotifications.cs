using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface INotifications : IBase<Notification>
    {
        public Task<bool> SendNotification(SendNotification_ViewModel Model);
        public Task<int> RemoveNotification(int Id, int UserId);
        public Task<bool> RemoveAllNotifications(int Id);
        public IQueryable<Notification> GetAll(int Id);
    }
}

using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IMessage
    {
        public IQueryable<Message>? GetAllMessagesOfProject(int ProjectId);
        public IQueryable<GetMessages_ViewModel>? GetAllMyMessages(int UserId);
        public Task<bool> SendAsync(SendMessage_ViewModel Model);
        public Task<int> CheckAsync(int MessageId, int UserId);
        public Task<bool> RemoveAsync(int Id, int UserId);
    }
}

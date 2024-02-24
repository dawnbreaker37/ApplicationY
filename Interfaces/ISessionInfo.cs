using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface ISessionInfo : IBase<SessionInfo>
    {
        public Task<bool> TerminateAllSessionsAsync(int UserId);
        public Task<int> AddSessionInfoAsync(AddSessionInfo_ViewModel Model);
        public Task<int> TerminateSessionAsync(int Id, int UserId);
        public IQueryable<SessionInfo>? GetSessionInfos(int Id);
    }
}

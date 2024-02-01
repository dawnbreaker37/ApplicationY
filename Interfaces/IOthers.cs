using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IOthers
    {
        public Task<int> SendPurgeAsync(int Id, int SenderId, int UserId, string Description);
        public Task<GetUserRole_ViewModel?> GetUserFullRoleInfoAsync(int UserId, int RoleId);
        public Task<int> GetUserRoleAsync(int UserId);
        public Task<string?> GetRoleNameAsync(int RoleId);
        public IQueryable<GetUserRole_ViewModel>? GetAllRoles(int CurrentRoleId);
    }
}

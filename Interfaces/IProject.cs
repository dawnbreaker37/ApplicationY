using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IProject : IBase<Project>
    {
        public Task<string?> CreateAsync(CreateProject_ViewModel Model);
        public Task<string?> EditAsync(CreateProject_ViewModel Model);
        public Task<int> RemoveAsync(int Id, int UserId);
        public Task<Project?> GetProjectAsync(int Id, bool GetAdditionalInfo);
        public IQueryable<Project?>? GetUsersAllProjects(int UserId, int SenderId, bool GetAdditionalInfo);
        public IQueryable<Project?>? GetUserAllProjectsByFilters(ProjectFilters_ViewModel Model);
        public Task<int> GetUsersLastProjectIdAsync(int UserId);
        public Task<int> CloseProjectAsync(int Id, int UserId);
        public Task<int> UnlockProjectAsync(int Id, int UserId);
        public Task<int> LikeTheProjectAsync(int ProjectId, int UserId);
        public Task<int> RemoveFromLikesAsync(int ProjectId, int UserId);
        public Task<bool> HasProjectBeenAlreadyLiked(int ProjectId, int UserId);
        public Task<int> ProjectLikesCount(int ProjectId);
        public IQueryable<GetProjects_ViewModel>? GetRandomProjects(int Count);
    }
}

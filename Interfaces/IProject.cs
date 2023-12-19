using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IProject : IBase<Project>
    {
        public Task<string?> CreateAsync(CreateProject_ViewModel Model);
        public Task<string?> EditAsync(CreateProject_ViewModel Model);
        public Task<List<string?>?> EditImagesAsync(int Id, IFormFileCollection Images);
        public Task<int> RemoveAsync(int Id, int UserId);
        public Task<Project?> GetProjectAsync(int Id, bool GetUsername, bool GetAdditionalInfo, int UserId);
        public IQueryable<Project?>? GetUsersAllProjects(int UserId, int SenderId, bool GetAdditionalInfo, bool GetCompressed);
        public IQueryable<Project?>? GetUserAllProjectsByFilters(ProjectFilters_ViewModel Model);
        public Task<int> GetUsersLastProjectIdAsync(int UserId);
        public Task<int> CloseProjectAsync(int Id, int UserId);
        public Task<int> UnlockProjectAsync(int Id, int UserId);
        public Task<int> LikeTheProjectAsync(int ProjectId, int UserId);
        public Task<int> RemoveFromLikesAsync(int ProjectId, int UserId, bool RemoveAll);
        public Task<int> PinAsync(int Id, int UserId);
        public Task<int> UnpinAsync(int Id, int UserId);
        public Task<int> ReleaseNotesCountAsync(int ProjectId);
        public Task<bool> HasProjectBeenAlreadyLiked(int ProjectId, int UserId);
        public Task<int> ProjectLikesCount(int ProjectId);
        public Task<bool> AddReleaseNoteAsync(int ProjectId, string Title, string Description);
        public IQueryable<GetLikedProjects_ViewModel>? GetLikedProjects(int UserId);
        public IQueryable<GetLikedProjects_ViewModel>? GetLikedProjectsSimplified(int UserId);
        public IQueryable<GetReleaseNotes_ViewModel>? GetReleaseNotes(int ProjectId); 
        public IQueryable<GetProjects_ViewModel>? GetRandomProjects(int Count);
        public Task<int> GetProjectsCount();
        public IQueryable<GetProjects_ViewModel>? FindProjects(string Keyword, int MinPrice, int MaxPrice, int CategoryId);
    }
}

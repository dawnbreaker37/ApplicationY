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
        public IQueryable<Project?> GetUsersAllProjectsAsync(int UserId, bool GetAdditionalInfo);
        public Task<int> GetUsersLastProjectIdAsync(int UserId);
    }
}

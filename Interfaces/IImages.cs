using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IImages : IBase<Image>
    {
        public Task<int> GetProjectImagesCountAsync(int ProjectId);
        public Task<int> RemoveImageAsync(int Id, int ProjectId);
        public IQueryable<GetProjectImages_ViewModel>? GetAllProjectImages(int ProjectId);
    }
}

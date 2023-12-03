using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IPost : IBase<Post>
    {
         public Task<int> CreatePostAsync(CreatePost_ViewModel Model);
        public Task<int> RemovePostAsync(int Id, int UserId);
    }
}

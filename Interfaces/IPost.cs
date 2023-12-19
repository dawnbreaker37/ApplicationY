using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IPost : IBase<Post>
    {
        public IQueryable<GetPost_ViewModel>? GetUserAllPosts(int UserId);
         public Task<int> CreatePostAsync(CreatePost_ViewModel Model);
        public Task<int> RemovePostAsync(int Id, int UserId);
        public Task<int> LikeThePostAsync(int Id, int UserId);
        public Task<int> DislikeThePostAsync(int Id, int UserId);
        public IQueryable<LikedPost>? GetUsersLikedPosts(int UserId);
    }
}

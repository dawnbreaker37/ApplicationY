using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Interfaces
{
    public interface IPost : IBase<Post>
    {
        public IQueryable<GetPost_ViewModel>? GetUserAllPosts(int UserId, bool GetAdditionalInfo);
        public IQueryable<GetPost_ViewModel>? GetAllAssociatedPosts(int ProjectId);
        public IQueryable<GetPost_ViewModel>? GetAllAssociatedPostsWAdditionalInfo(int ProjectId);
        public IQueryable<GetPost_ViewModel>? GetRandomPosts(int Count);
        public Task<int> GetAssociatedPostsCountAsync(int ProjectId);
        public Task<int> GetUsersAllPostsCountAsync(int UserId);
        public Task<int> CreatePostAsync(CreatePost_ViewModel Model);
        public Task<int> PinThePostAsync(int Id, int UserId);
        public Task<int> UnpinThePostAsync(int Id, int UserId);
        public Task<int> EditPostAsync(CreatePost_ViewModel Model);
        public Task<int> LockThePostAsync(int Id, int UserId);
        public Task<int> UnlockThePostAsync(int Id, int UserId);
        public Task<int> RemovePostAsync(int Id, int UserId);
        public Task<int> LikeThePostAsync(int Id, int UserId);
        public Task<int> DislikeThePostAsync(int Id, int UserId);
        public IQueryable<LikedPost>? GetUsersLikedPosts(int UserId);
    }
}

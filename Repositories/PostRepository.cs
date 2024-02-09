using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class PostRepository : Base<Post>, IPost
    {
        private readonly Context _context;
        public PostRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CreatePostAsync(CreatePost_ViewModel Model)
        {
            if (Model.UserId != 0 && !String.IsNullOrEmpty(Model.Text) && Model.Text.Length < 2500)
            {
                Post post = new Post()
                {
                    CreatedAt = DateTime.Now,
                    IsRemoved = false,
                    IsPrivate = Model.IsPrivate,
                    UserId = Model.UserId,
                    AllowMentions = Model.AllowMentions,
                    LinkedProjectId = Model.LinkedProjectId != 0 ? Model.LinkedProjectId : null,
                    Text = Model.Text
                };
                await _context.AddAsync(post);
                await _context.SaveChangesAsync();

                return post.Id;
            }
            else return 0;
        }

        public async Task<int> LikeThePostAsync(int Id, int UserId)
        {
            if (Id != 0 && UserId != 0)
            {
                bool HasBeenLikedYet = await _context.LikedPosts.AnyAsync(l => l.PostId == Id && l.UserId == UserId);
                if (HasBeenLikedYet)
                {
                    int Result = await _context.LikedPosts.AsNoTracking().Where(l => l.UserId == UserId && l.PostId == Id && l.IsRemoved).ExecuteUpdateAsync(l => l.SetProperty(l => l.IsRemoved, false));
                    if (Result != 0) return Result;
                }
                else
                {
                    LikedPost likedPost = new LikedPost
                    {
                        IsRemoved = false,
                        UserId = UserId,
                        PostId = Id
                    };
                    await _context.AddAsync(likedPost);
                    await _context.SaveChangesAsync();

                    return Id;
                }
            }
            return 0;
        }

        public async Task<int> DislikeThePostAsync(int Id, int UserId)
        {
            if(Id != 0 && UserId != 0)
            {
                int Result = await _context.LikedPosts.AsNoTracking().Where(l => l.PostId == Id && l.UserId == UserId && !l.IsRemoved).ExecuteUpdateAsync(l => l.SetProperty(l => l.IsRemoved, true));
                if (Result != 0) return Id;
            }
            return 0;
        }

        public IQueryable<GetPost_ViewModel>? GetUserAllPosts(int UserId, bool GetAdditionalInfo)
        {
            if (GetAdditionalInfo) return _context.Posts.AsNoTracking().Where(p => p.UserId == UserId && !p.IsRemoved).Select(p => new GetPost_ViewModel { Id = p.Id, CreatedAt = p.CreatedAt, LinkedProjectId = p.LinkedProjectId, Text = p.Text, UserId = p.UserId, AllowMentions = p.AllowMentions, IsRemoved = p.IsRemoved, Project = p.Project != null ? new Project { Name = p.Project.Name, Description = p.Project.Description, CreatedAt = p.Project.CreatedAt, Views = p.Project.Views } : null }).OrderByDescending(p => p.CreatedAt);
            else return _context.Posts.AsNoTracking().Where(p => p.UserId == UserId && !p.IsRemoved).Select(p => new GetPost_ViewModel { Id = p.Id, CreatedAt = p.CreatedAt, Text = p.Text, UserId = UserId, IsPrivate = p.IsPrivate, AllowMentions = p.AllowMentions, LinkedProjectId = p.LinkedProjectId }).OrderByDescending(p => p.CreatedAt);
        }

        public async Task<int> RemovePostAsync(int Id, int UserId)
        {
            if (Id != 0 || UserId != 0)
            {
                int Result = await _context.Posts.AsNoTracking().Where(p => p.Id == Id && p.UserId == UserId && !p.IsRemoved).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsRemoved, true));
                if (Result != 0) return Result;
            }
            return 0;
        }

        public IQueryable<LikedPost>? GetUsersLikedPosts(int UserId)
        {
            if (UserId != 0) return _context.LikedPosts.AsNoTracking().Where(l => l.UserId == UserId && !l.IsRemoved).Select(l => new LikedPost { PostId = l.PostId, UserId = l.UserId });
            else return null;
        }

        public async Task<int> EditPostAsync(CreatePost_ViewModel Model)
        {
            if(!String.IsNullOrEmpty(Model.Text) && Model.Text.Length <= 2500)
            {
                int Result = await _context.Posts.Where(p => p.Id == Model.Id && !p.IsRemoved).ExecuteUpdateAsync(p => p.SetProperty(p => p.Text, Model.Text));
                if (Result != 0) return Model.Id;
            }
            return 0;
        }

        public async Task<int> LockThePostAsync(int Id, int UserId)
        {
            if(Id != 0 || UserId != 0)
            {
                int Result = await _context.Posts.Where(p => p.Id == Id && p.UserId == UserId && !p.IsRemoved).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsPrivate, true));
                if (Result != 0) return Id;
            }
            return 0;
        }

        public async Task<int> UnlockThePostAsync(int Id, int UserId)
        {
            if (Id != 0 || UserId != 0)
            {
                int Result = await _context.Posts.Where(p => p.Id == Id && p.UserId == UserId && !p.IsRemoved).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsPrivate, false));
                if (Result != 0) return Id;
            }
            return 0;
        }

        public async Task<int> GetUsersAllPostsCountAsync(int UserId)
        {
            return await _context.Posts.AsNoTracking().CountAsync(p => p.UserId == UserId && !p.IsRemoved);
        }

        public IQueryable<GetPost_ViewModel>? GetAllAssociatedPosts(int ProjectId)
        {
            if (ProjectId != 0) return _context.Posts.AsNoTracking().Where(p => p.LinkedProjectId == ProjectId && !p.IsPrivate && !p.IsRemoved).Select(p => new GetPost_ViewModel { Id = p.Id, Text = p.Text, CreatedAt = p.CreatedAt, AllowMentions = p.AllowMentions }).OrderByDescending(p => p.CreatedAt);
            else return null;
        }

        public IQueryable<GetPost_ViewModel>? GetAllAssociatedPostsWAdditionalInfo(int ProjectId)
        {
            if (ProjectId != 0) return _context.Posts.AsNoTracking().Where(p => p.LinkedProjectId == ProjectId && !p.IsPrivate && !p.IsRemoved).Select(p => new GetPost_ViewModel { Id = p.Id, Text = p.Text, CreatedAt = p.CreatedAt, AllowMentions = p.AllowMentions, LikedCount = p.LikedPosts != null ? p.LikedPosts.Count(l => l.PostId == p.Id) : 0, CreatorName = p.User!.PseudoName, UserId = p.UserId }).OrderByDescending(p => p.CreatedAt);
            else return null;
        }

        public async Task<int> GetAssociatedPostsCountAsync(int ProjectId)
        {
            if (ProjectId != 0) return await _context.Posts.CountAsync(p => p.LinkedProjectId == ProjectId && !p.IsPrivate && !p.IsRemoved);
            else return 0;
        }

        public IQueryable<GetPost_ViewModel>? GetRandomPosts(int Count)
        {
            if (Count != 0) return _context.Posts.AsNoTracking().Where(p => !p.IsPrivate && !p.IsRemoved && p.CreatedAt >= DateTime.Now.AddDays(-14)).Select(p => new GetPost_ViewModel { Id = p.Id, CreatedAt = p.CreatedAt, Text = p.Text, AllowMentions = p.AllowMentions, CreatorName = p.User!.PseudoName, CreatorPhoto = p.User!.ProfilePhoto, UserId = p.UserId, Project = p.Project != null ? new Project { Id = p.Project.Id, Name = p.Project.Name, Description = p.Project.Description, CreatedAt = p.Project.CreatedAt, Views = p.Project.Views } : null, LinkedProjectId = p.LinkedProjectId }).Take(Count).OrderByDescending(p => Guid.NewGuid());
            else return null;
        }
    }
}

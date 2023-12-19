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
                    IsPrivate = false,
                    UserId = Model.UserId,
                    LinkedProjectId = Model.LinkedProjectId,
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
                    int Result = await _context.LikedPosts.Where(l => l.UserId == UserId && l.PostId == Id && l.IsRemoved).ExecuteUpdateAsync(l => l.SetProperty(l => l.IsRemoved, false));
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
                int Result = await _context.LikedPosts.Where(l => l.PostId == Id && l.UserId == UserId && !l.IsRemoved).ExecuteUpdateAsync(l => l.SetProperty(l => l.IsRemoved, true));
                if (Result != 0) return Id;
            }
            return 0;
        }

        public IQueryable<GetPost_ViewModel>? GetUserAllPosts(int UserId)
        {
            return _context.Posts.AsNoTracking().Where(p => p.UserId == UserId && !p.IsRemoved).Select(p => new GetPost_ViewModel { Id = p.Id, CreatedAt = p.CreatedAt, LinkedProjectId = p.LinkedProjectId, Text = p.Text, UserId = p.UserId, IsRemoved = p.IsRemoved,  Project = p.Project != null ? new Project { Name = p.Project.Name, Description = p.Project.Description, CreatedAt = p.Project.CreatedAt, Views = p.Project.Views } : null}).OrderByDescending(p => p.CreatedAt);
        }

        public async Task<int> RemovePostAsync(int Id, int UserId)
        {
            if (Id != 0 || UserId != 0)
            {
                int Result = await _context.Posts.Where(p => p.Id == Id && p.UserId == UserId && !p.IsRemoved).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsRemoved, true));
                if (Result != 0) return Result;
            }
            return 0;
        }

        public IQueryable<LikedPost>? GetUsersLikedPosts(int UserId)
        {
            if (UserId != 0) return _context.LikedPosts.Where(l => l.UserId == UserId && !l.IsRemoved).Select(l => new LikedPost { PostId = l.PostId, UserId = l.UserId });
            else return null;
        }
    }
}

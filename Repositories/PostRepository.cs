using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;

namespace ApplicationY.Repositories
{
    public class PostRepository : Base<Post>, IPost
    {
        private readonly Context _context;
        public PostRepository(Context context) : base(context)
        {
            _context = context;
        }

        public Task<int> CreatePostAsync(CreatePost_ViewModel Model)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemovePostAsync(int Id, int UserId)
        {
            throw new NotImplementedException();
        }
    }
}

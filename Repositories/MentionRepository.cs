using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class MentionRepository : Base<Mention>, IMention
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public MentionRepository(Context context, UserManager<User> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<string?> EditMentionAsync(SendComment_ViewModel MentionModel)
        {
            if(MentionModel != null)
            {
                int Result = await _context.Mentions.Where(m => m.Id == MentionModel.Id && m.PostId == MentionModel.ProjectId && m.UserId == MentionModel.UserId).ExecuteUpdateAsync(m => m.SetProperty(m => m.Text, MentionModel.Text).SetProperty(m => m.IsEdited, true));
                if (Result != 0) return MentionModel.Text;
            }
            return null;
        }

        public async Task<int> GetMentionsCountAsync(int Id)
        {
            if (Id != 0) return await _context.Mentions.AsNoTracking().CountAsync(m => m.PostId == Id && !m.IsRemoved);
            else return 0;
        }

        public IQueryable<Mention>? GetPostMentions(int Id, int SkipCount, int Count)
        {
            if (Id != 0) return _context.Mentions.AsNoTracking().Where(m => m.PostId == Id && !m.IsRemoved).Select(m => new Mention { Id = m.Id, PostId = m.PostId, IsEdited = m.IsEdited, SentAt = m.SentAt, Text = m.Text, User = new User { PseudoName = m.User!.PseudoName, SearchName = m.User.SearchName }, UserId = m.UserId }).Skip(SkipCount).Take(Count).OrderByDescending(m => m.SentAt);
            else return null;
        }

        public async Task<int> RemoveMentionAsync(int Id, int PostId, int UserId)
        {
            if(Id != 0 && UserId != 0)
            {
                int Result = await _context.Mentions.Where(p => !p.IsRemoved && p.Id == Id && p.UserId == UserId && p.PostId == PostId).ExecuteUpdateAsync(p => p.SetProperty(p => p.IsRemoved, true));
                if (Result != 0) return Id;
            }
            return 0;
        }

        public async Task<string?> SendMentionAsync(SendComment_ViewModel MentionModel)
        {
            bool AreMentionsAllowed = await _context.Posts.AsNoTracking().AnyAsync(m => m.Id == MentionModel.ProjectId && !m.IsRemoved && m.AllowMentions);
            if (AreMentionsAllowed) {
                if (MentionModel != null)
                {
                    Mention mention = new Mention()
                    {
                        IsRemoved = false,
                        PostId = MentionModel.ProjectId,
                        SentAt = DateTime.Now,
                        Text = MentionModel.Text,
                        UserId = MentionModel.UserId
                    };
                    await _context.AddAsync(mention);
                    await _context.SaveChangesAsync();

                    return MentionModel.Text;
                }
            }
            return null;
        }
    }
}

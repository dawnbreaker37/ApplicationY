using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class MessageRepository : Base<Message>, IMessage
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public MessageRepository(Context context, UserManager<User> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> CheckAsync(int MessageId, int UserId)
        {
            if(MessageId != 0)
            {
                if(MessageId == -256)
                {
                    int Result = await _context.Messages.Where(u => u.UserId == UserId && !u.IsRemoved).ExecuteUpdateAsync(u => u.SetProperty(u => u.IsChecked, true));
                    if (Result != 0) return -256;
                }
                else
                {
                    int Result = await _context.Messages.Where(u => u.UserId == UserId && u.Id == MessageId && !u.IsRemoved).ExecuteUpdateAsync(u => u.SetProperty(u => u.IsChecked, true));
                    if(Result != 0) return Result;
                }
            }
            return 0;
        }

        public IQueryable<Message>? GetAllMessagesOfProject(int ProjectId)
        {
            if (ProjectId != 0)
            {
                return _context.Messages.Where(p => p.ProjectId == ProjectId && !p.IsRemoved).Select(p => new Message { Id = p.Id, UserId = p.UserId, IsChecked = p.IsChecked, SentAt = p.SentAt, Text = p.Text }).OrderByDescending(p => p.SentAt).AsNoTracking();
            }
            else return null;
        }

        public IQueryable<GetMessages_ViewModel>? GetAllMyMessages(int UserId, bool GetSentMessages)
        {
            if (UserId != 0)
            {
                if (!GetSentMessages) return _context.Messages.Where(u => u.UserId == UserId && !u.IsRemoved).Select(u => new GetMessages_ViewModel { Id = u.Id, IsChecked = u.IsChecked, IsFromSupports = u.IsFromSupports, ProjectId = u.ProjectId, SentAt = u.SentAt, Text = u.Text, SenderId = u.SenderId, SenderName = u.User!.PseudoName }).OrderByDescending(u => u.SentAt).AsNoTracking();
                else return _context.Messages.AsNoTracking().Where(u => u.SenderId == UserId && !u.IsRemoved).Select(u => new GetMessages_ViewModel { Id = u.Id, IsChecked = u.IsChecked, ProjectName = u.Project!.Name, SentAt = u.SentAt, Text = u.Text, UserId = u.UserId }).OrderByDescending(u => u.SentAt);
            }
            else return null;
        }

        public IQueryable<GetCommentaries_ViewModel>? GetComments(int ProjectId, int SkipCount, int Count)
        {
            if (ProjectId != 0) return _context.Comments.AsNoTracking().Where(c => c.ProjectId == ProjectId && !c.IsRemoved).Select(c => new GetCommentaries_ViewModel { Id = c.Id, SenderId = c.UserId, RepliesCount = c.Replies != null ? c.Replies.Count : 0, SentAt = c.SentAt, SenderName = c.User!.PseudoName, Text = c.Text }).Skip(SkipCount).Take(Count).OrderByDescending(c => c.SentAt);
            else return null;
        }

        public async Task<GetCommentaries_ViewModel?> GetLastCommentsInfoAsync(int ProjectId)
        {
            if (ProjectId != 0) return await _context.Comments.AsNoTracking().OrderByDescending(c => c.SentAt).Select(c => new GetCommentaries_ViewModel { ProjectId = c.ProjectId, SenderName = c.User!.PseudoName, Text = c.Text }).FirstOrDefaultAsync(c => c.ProjectId == ProjectId);
            else return null;
        }

        public IQueryable<GetReplies_ViewModel>? GetReplies(int Id, int SkipCount, int LoadCount)
        {
            if (Id != 0) return _context.Replies.Where(r => r.CommentId == Id && !r.IsRemoved).AsNoTracking().Select(r => new GetReplies_ViewModel { Id = r.Id, SentAt = r.SentAt, Text = r.Text, Username = r.User!.PseudoName, UserId = r.UserId }).Skip(SkipCount).Take(LoadCount).OrderByDescending(r => r.SentAt);
            else return null;
        }

        public async Task<GetMessages_ViewModel?> GetMessageInfo(int Id)
        {
            if (Id != 0)
            {
                await _context.Messages.Where(m => m.Id == Id && !m.IsRemoved).ExecuteUpdateAsync(m => m.SetProperty(m => m.IsChecked, true));
                return await _context.Messages.AsNoTracking().Select(m => new GetMessages_ViewModel { Id = m.Id, IsChecked = m.IsChecked, IsRemoved = m.IsRemoved, SentAt = m.SentAt, ProjectName = m.Project!.Name }).FirstOrDefaultAsync(m => m.Id == Id && !m.IsRemoved);
            }
            else return null;
        }

        public async Task<int> GetProjectCommentsCountAsync(int ProjectId)
        {
            if (ProjectId != 0) return await _context.Comments.AsNoTracking().CountAsync(c => c.ProjectId == ProjectId && !c.IsRemoved);
            else return 0;
        }

        public async Task<int> RemoveAsync(int Id, int UserId)
        {
            if(Id != 0 && UserId != 0)
            {
                int Result = 0;
                if (Id != -256)
                {
                    Result = await _context.Messages.Where(m => m.Id == Id && m.SenderId == UserId && !m.IsRemoved).ExecuteUpdateAsync(m => m.SetProperty(m => m.IsRemoved, true));
                    if (Result != 0) return Id;
                }
                else
                {
                    Result = await _context.Messages.Where(m => m.SenderId == UserId && !m.IsRemoved).ExecuteUpdateAsync(m => m.SetProperty(m => m.IsRemoved, true));
                    if (Result != 0) return -256;
                }
            }
            return 0;
        }

        public async Task<bool> SendAsync(SendMessage_ViewModel Model)
        {
            if(Model.UserId != 0 && Model.SenderId != 0 && !String.IsNullOrEmpty(Model.Text))
            {
                Message message;
                bool IsSenderAnAdmin = await _context.UserRoles.AsNoTracking().AnyAsync(u => u.UserId == Model.UserId && u.RoleId != 0);
                if (IsSenderAnAdmin)
                {
                    Model.ProjectId = Model.ProjectId == 0 ? null : Model.ProjectId;
                    message = new()
                    {
                        IsChecked = false,
                        IsRemoved = false,
                        ProjectId = Model.ProjectId,
                        Text = Model.Text,
                        IsFromSupports = true,
                        SentAt = DateTime.Now,
                        SenderId = Model.SenderId,
                        UserId = Model.UserId
                    };
                }
                else
                {
                    Model.ProjectId = Model.ProjectId == 0 ? null : Model.ProjectId;
                    message = new()
                    {
                        IsChecked = false,
                        IsRemoved = false,
                        ProjectId = Model.ProjectId,
                        Text = Model.Text,
                        IsFromSupports = false,
                        SentAt = DateTime.Now,
                        SenderId = -256,
                        UserId = Model.UserId
                    };
                }
                await _context.AddAsync(message);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<int> SendCommentAsync(SendComment_ViewModel Model)
        {
            if (Model.ProjectId != 0 && Model.UserId != 0)
            {
                Comment comment = new Comment
                {
                    IsRemoved = false,
                    ProjectId = Model.ProjectId,
                    SentAt = DateTime.Now,
                    Text = Model.Text,
                    UserId = Model.UserId
                };
                await _context.AddAsync(comment);
                await _context.SaveChangesAsync();

                return Model.ProjectId;
            }
            else return 0;
        }

        public async Task<bool> SendCommentReplyAsync(SendReply_ViewModel Model)
        {
           if(Model.UserId != 0 && Model.CommentId != 0 && !String.IsNullOrEmpty(Model.Text))
            {
                Reply reply = new Reply
                {
                    IsRemoved = false,
                    CommentId = Model.CommentId,
                    IsChecked = false,
                    SentAt = DateTime.Now,
                    Text = Model.Text,
                    UserId = Model.UserId
                };
                await _context.AddAsync(reply);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<int> GetCommentRepliesCountAsync(int CommentId)
        {
            if (CommentId != 0) return await _context.Replies.AsNoTracking().CountAsync(r => r.CommentId == CommentId && !r.IsRemoved);
            else return 0;
        }
    }
}

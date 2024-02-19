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

        public MessageRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CheckAsync(int MessageId, int UserId)
        {
            if(MessageId != 0)
            {
                if(MessageId == -128)
                {
                    int Result = await _context.Messages.AsNoTracking().Where(u => u.UserId == UserId && !u.IsRemoved).ExecuteUpdateAsync(u => u.SetProperty(u => u.IsChecked, true));
                    if (Result != 0) return -128;
                }
                else if(MessageId == -256)
                {
                    int Result = await _context.Messages.AsNoTracking().Where(u => u.UserId == UserId && u.Id == MessageId).ExecuteUpdateAsync(u => u.SetProperty(u => u.IsChecked, true));
                    if (Result != 0) return -256;
                }
                else
                {
                    int Result = await _context.Messages.AsNoTracking().Where(u => u.UserId == UserId && u.Id == MessageId && !u.IsRemoved).ExecuteUpdateAsync(u => u.SetProperty(u => u.IsChecked, true));
                    if(Result != 0) return Result;
                }
            }
            return 0;
        }

        public IQueryable<Message>? GetAllMessagesOfProject(int ProjectId)
        {
            if (ProjectId != 0)
            {
                return _context.Messages.AsNoTracking().Where(p => p.ProjectId == ProjectId && !p.IsRemoved).Select(p => new Message { Id = p.Id, UserId = p.UserId, IsChecked = p.IsChecked, SentAt = p.SentAt, Text = p.Text }).OrderByDescending(p => p.SentAt).AsNoTracking();
            }
            else return null;
        }

        public IQueryable<GetMessages_ViewModel>? GetSentMessages(int UserId, bool GetRemoved)
        {
            if (UserId != 0)
            {
                if (GetRemoved) return _context.Messages.AsNoTracking().Where(m => m.SenderId == UserId).Select(m => new GetMessages_ViewModel { Id = m.Id, Text = m.Text, SentAt = m.SentAt, IsChecked = m.IsChecked }).OrderByDescending(m => m.SentAt);
                else return _context.Messages.AsNoTracking().Where(m => m.SenderId == UserId && !m.IsRemoved).Select(m => new GetMessages_ViewModel { Id = m.Id, Text = m.Text, SentAt = m.SentAt, IsChecked = m.IsChecked }).OrderByDescending(m => m.SentAt);
            }
            else return null;
        }

        public IQueryable<GetMessages_ViewModel>? GetReceivedMessages(int UserId, bool GetRemoved)
        {
            if (UserId != 0)
            {
                if (GetRemoved) return _context.Messages.AsNoTracking().Where(m => m.UserId == UserId).Select(m => new GetMessages_ViewModel { Id = m.Id, SenderName = m.User!.PseudoName, SenderId = m.SenderId, Text = m.Text, SentAt = m.SentAt, IsChecked = m.IsChecked }).OrderByDescending(m => m.SentAt);
                else return _context.Messages.AsNoTracking().Where(m => m.UserId == UserId && !m.IsRemoved).Select(m => new GetMessages_ViewModel { Id = m.Id, SenderName = m.User!.PseudoName, SenderId = m.SenderId, Text = m.Text, SentAt = m.SentAt, IsChecked = m.IsChecked }).OrderByDescending(m => m.SentAt);
            }
            else return null;
        }

        public IQueryable<GetCommentaries_ViewModel>? GetComments(int ProjectId, int SkipCount, int Count)
        {
            if (ProjectId != 0) return _context.Comments.AsNoTracking().Where(c => c.ProjectId == ProjectId && !c.IsRemoved).Select(c => new GetCommentaries_ViewModel { Id = c.Id, IsEdited = c.IsEdited, SenderId = c.UserId, RepliesCount = c.Replies != null ? c.Replies.Count : 0, SentAt = c.SentAt, SenderName = c.User!.PseudoName, Text = c.Text }).Skip(SkipCount).Take(Count).OrderBy(c => c.SentAt);
            else return null;
        }

        public async Task<GetCommentaries_ViewModel?> GetLastCommentsInfoAsync(int ProjectId)
        {
            if (ProjectId != 0) return await _context.Comments.AsNoTracking().OrderByDescending(c => c.SentAt).Select(c => new GetCommentaries_ViewModel { ProjectId = c.ProjectId, SenderName = c.User!.PseudoName, Text = c.Text }).FirstOrDefaultAsync(c => c.ProjectId == ProjectId);
            else return null;
        }

        public IQueryable<GetReplies_ViewModel>? GetReplies(int Id, int SkipCount, int LoadCount)
        {
            if (Id != 0) return _context.Replies.Where(r => r.CommentId == Id && !r.IsRemoved).AsNoTracking().Select(r => new GetReplies_ViewModel { Id = r.Id, SentAt = r.SentAt, Text = r.Text, Username = r.User!.PseudoName, UserId = r.UserId }).Skip(SkipCount).Take(LoadCount).OrderBy(r => r.SentAt);
            else return null;
        }

        public async Task<GetMessages_ViewModel?> GetMessageInfo(int Id, bool IsFromSender)
        {
            if (Id != 0)
            {
                if (!IsFromSender)
                {
                    await _context.Messages.AsNoTracking().Where(m => m.Id == Id && !m.IsRemoved).ExecuteUpdateAsync(m => m.SetProperty(m => m.IsChecked, true));
                    return await _context.Messages.AsNoTracking().Select(m => new GetMessages_ViewModel { Id = m.Id, IsChecked = m.IsChecked, IsRemoved = m.IsRemoved, SentAt = m.SentAt, ProjectName = m.Project != null ? m.Project!.Name : null }).FirstOrDefaultAsync(m => m.Id == Id && !m.IsRemoved);
                }
                return await _context.Messages.AsNoTracking().Select(m => new GetMessages_ViewModel { Id = m.Id, IsChecked = m.IsChecked, IsRemoved = m.IsRemoved, SentAt = m.SentAt, ProjectName = m.Project != null ? m.Project!.Name : null, UserId = m.UserId }).FirstOrDefaultAsync(m => m.Id == Id && !m.IsRemoved);
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
                if(Id == -128)
                {
                    int Result = await _context.Messages.AsNoTracking().Where(m => m.SenderId == UserId && !m.IsRemoved).ExecuteUpdateAsync(m => m.SetProperty(m => m.IsRemoved, true));
                    if (Result != 0) return -128;
                }
                else
                {
                    int Result = await _context.Messages.AsNoTracking().Where(m => m.SenderId == UserId && m.Id == Id && !m.IsRemoved).ExecuteUpdateAsync(m => m.SetProperty(m => m.IsRemoved, true));
                    if (Result != 0) return Id;
                }
            }
            return 0;
        }

        public async Task<int> RemoveForAdminsAsync(int Id)
        {
            if(Id != 0)
            {
                if(Id != -128)
                {
                    int Result = await _context.Messages.AsNoTracking().Where(m => m.Id == Id && m.UserId == -256).ExecuteUpdateAsync(m => m.SetProperty(m => m.IsRemoved, true));
                    if(Result != 0) return Id;
                }
                else
                {
                    int Result = await _context.Messages.AsNoTracking().Where(m => m.UserId == -256).ExecuteUpdateAsync(m => m.SetProperty(m => m.IsRemoved, true));
                    if (Result != 0) return Id;
                }
            }
            return 0;
        }

        public async Task<bool> SendAsync(SendMessage_ViewModel Model)
        {
            if(Model.UserId != 0 && !String.IsNullOrEmpty(Model.Text) && Model.Text.Length <= 2500)
            {
                Message message;
                bool IsSenderAnAdmin = await _context.UserRoles.AsNoTracking().AnyAsync(u => u.UserId == Model.SenderId && u.RoleId != 0);
                
                Model.ProjectId = Model.ProjectId == 0 ? null : Model.ProjectId;
                Model.SenderId = Model.SenderId != 0 ? Model.SenderId : null;
                Model.SenderEmail = Model.SenderId == null ? Model.SenderEmail : null;

                if (Model.SenderEmail != null || Model.SenderId != null)
                {
                    if (IsSenderAnAdmin)
                    {
                        message = new()
                        {
                            IsChecked = false,
                            IsRemoved = false,
                            ProjectId = Model.ProjectId,
                            Text = Model.Text,
                            IsFromSupports = true,
                            SentAt = DateTime.Now,
                            SenderId = Model.SenderId,
                            SenderEmail = Model.SenderEmail,
                            UserId = Model.UserId
                        };
                    }
                    else
                    {
                        message = new()
                        {
                            IsChecked = false,
                            IsRemoved = false,
                            ProjectId = Model.ProjectId,
                            Text = Model.Text,
                            IsFromSupports = false,
                            SentAt = DateTime.Now,
                            SenderId = Model.SenderId,
                            SenderEmail = Model.SenderEmail,
                            UserId = Model.UserId
                        };
                    }
                    await _context.AddAsync(message);
                    await _context.SaveChangesAsync();

                    return true;
                }
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

        public async Task<string?> EditCommentAsync(SendComment_ViewModel Model)
        {
            if(Model != null)
            {
                int Result = await _context.Comments.AsNoTracking().Where(c => c.Id == Model.Id && c.UserId == Model.UserId && c.ProjectId == Model.ProjectId).ExecuteUpdateAsync(c => c.SetProperty(c => c.Text, Model.Text).SetProperty(c => c.IsEdited, true));
                if (Result != 0) return Model.Text;
            }
            return null;
        }

        public async Task<int> RemoveCommentAsync(int Id, int ProjectId, int UserId)
        {
            if(Id != 0 && UserId != 0 && ProjectId != 0)
            {
                int Result = await _context.Comments.AsNoTracking().Where(c => c.Id == Id && c.ProjectId == ProjectId && c.UserId == UserId).ExecuteUpdateAsync(c => c.SetProperty(c => c.IsRemoved, true));
                if(Result != 0) return Id;
            }
            return 0;
        }

        public async Task<int> GetReceivedMessagesCount(int AccesserId, int UserId, bool GetRemovedMessagesCountToo)
        {
            if (UserId != 0)
            {
                if (GetRemovedMessagesCountToo && UserId == -256)
                {
                    bool IsTheAccessorAnAdmin = await _context.UserRoles.AsNoTracking().AnyAsync(u => u.UserId == AccesserId && u.RoleId > 0);
                    if (IsTheAccessorAnAdmin) return await _context.Messages.AsNoTracking().CountAsync(m => m.UserId == -256);
                    else return 0;
                }
                else return await _context.Messages.AsNoTracking().CountAsync(m => m.UserId == UserId && !m.IsRemoved);
            }
            else return 0;
        }

        public async Task<IQueryable<GetMessages_ViewModel>?> GetAdminsReceivedMessagesAsync(int AccessorId, bool GetRemovedToo, int SkippedCount, int LoadCount)
        {
            if(AccessorId != 0)
            {
                bool IsTheAccessorAnAdmin = await _context.UserRoles.AsNoTracking().AnyAsync(c => c.UserId == AccessorId && c.RoleId > 0);
                if(IsTheAccessorAnAdmin)
                {
                    if (GetRemovedToo) return _context.Messages.AsNoTracking().Where(c => c.UserId == -256).Select(c => new GetMessages_ViewModel { Id = c.Id, Text = c.Text, IsChecked = c.IsChecked, IsRemoved = c.IsRemoved, IsFromSupports = c.IsFromSupports, SenderId = c.SenderId, SenderEmail = c.SenderId == null ? c.SenderEmail : null,  SenderName = c.SenderId != null ? c.User!.PseudoName : null, SentAt = c.SentAt }).OrderByDescending(c => c.SentAt).Skip(SkippedCount).Take(LoadCount);
                    else return _context.Messages.AsNoTracking().Where(c => c.UserId == -256 && !c.IsRemoved).Select(c => new GetMessages_ViewModel { Id = c.Id, Text = c.Text, IsChecked = c.IsChecked, IsFromSupports = c.IsFromSupports, SenderId = c.SenderId, SenderName = c.SenderId != null ? c.User!.PseudoName : null, SenderEmail = c.SenderId == null ? c.SenderEmail : null, SentAt = c.SentAt }).OrderByDescending(c => c.SentAt).Skip(SkippedCount).Take(LoadCount);
                }
            }
            return null;
        }
    }
}

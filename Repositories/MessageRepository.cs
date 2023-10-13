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

        public IQueryable<GetMessages_ViewModel>? GetAllMyMessages(int UserId)
        {
            if (UserId != 0)
            {
                return _context.Messages.Where(u => u.UserId == UserId && !u.IsRemoved).Select(u => new GetMessages_ViewModel { Id = u.Id, IsChecked = u.IsChecked, ProjectId = u.ProjectId, SentAt = u.SentAt, Text = u.Text, SenderId = u.SenderId, SenderName = u.User!.PseudoName }).OrderByDescending(u => u.SentAt).AsNoTracking();
            }
            else return null;
        }

        public async Task<bool> RemoveAsync(int Id, int UserId)
        {
            if(Id != 0 && UserId != 0)
            {
                int Result = await _context.Messages.Where(m => m.Id == Id && m.UserId == UserId && !m.IsRemoved).ExecuteUpdateAsync(m => m.SetProperty(m => m.IsRemoved, true));
                if (Result != 0) return true;
            }
            return false;
        }

        public async Task<bool> SendAsync(SendMessage_ViewModel Model)
        {
            if(Model.UserId != 0 && Model.ProjectId != 0 && !String.IsNullOrEmpty(Model.Text))
            {
                Message message = new()
                {
                    IsChecked = false,
                    IsRemoved = false,
                    ProjectId = Model.ProjectId,
                    Text = Model.Text,
                    SentAt = DateTime.Now,
                    UserId = Model.UserId
                };
                await _context.AddAsync(message);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
    }
}

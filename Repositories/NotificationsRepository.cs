using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Repositories
{
    public class NotificationsRepository : Base<Notification>, INotifications
    {
        private readonly Context _context;
        public NotificationsRepository(Context context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Notification> GetAll(int Id)
        {
            //&& ((n.SentAt.Year == DateTime.Now.Year && DateTime.Now.DayOfYear - 14 >= n.SentAt.DayOfYear) || (n.SentAt.DayOfYear >= 349))
            return _context.Notifications.AsNoTracking().Where(n => (n.UserId == Id) && (!n.IsRemoved)).Select(n => new Notification { Id = n.Id, Title = n.Title, Description = n.Description, SentAt = n.SentAt, UserId = n.UserId }).OrderByDescending(n => n.SentAt);
        }

        public async Task<bool> RemoveAllNotifications(int Id)
        {
            if(Id != 0)
            {
                int Result = await _context.Notifications.Where(u => u.UserId == Id).ExecuteUpdateAsync(n => n.SetProperty(n => n.IsRemoved, true));
                if (Result != 0) return true;
            }
            return false;
        }

        public async Task<int> RemoveNotification(int Id, int UserId)
        {
            if(Id != 0 && UserId != 0)
            {
                int Result = await _context.Notifications.Where(n => n.UserId == UserId && n.Id == Id).ExecuteUpdateAsync(n => n.SetProperty(n => n.IsRemoved, true));
                if (Result != 0) return await _context.Notifications.CountAsync(n => n.UserId == UserId && !n.IsRemoved);
            }
            return -256;
        }

        public async Task<bool> SendNotification(SendNotification_ViewModel Model)
        {
            if(Model.UserId != 0)
            {
                Notification notification = new Notification
                {
                    Title = Model.Title,
                    Description = Model.Description,
                    UserId = Model.UserId,
                    SentAt = DateTime.Now
                };
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
    }
}

using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ApplicationY.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly INotifications _notificationsRepository;

        public NotificationsController(Context context, UserManager<User> userManager, INotifications notificationsRepository)
        {
            _context = context;
            _userManager = userManager;
            _notificationsRepository = notificationsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications(int Id)
        {
            IQueryable<Notification> Notification_Preview = _notificationsRepository.GetAll(Id);
            if (Notification_Preview != null)
            {
                List<Notification> Notifications = await Notification_Preview.ToListAsync();
                int Count = Notifications.Count;

                if (Count > 0) return Json(new { success = true, userId = Id, notifications = Notifications, count = Count });
                else return Json(new { success = true, count = 0 });
            }
            else return Json(new { success = false, alert = "An error occured while trying to get your notifications. Please, try again later" });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int Id, int UserId)
        {
            if (User.Identity.IsAuthenticated)
            {
                int Result = await _notificationsRepository.RemoveNotification(Id, UserId);
                if (Result != -256) return Json(new { success = true, id = Id, count = Result });
                else return Json(new { success = false, alert = "An error occured while trying to remove the notification. Please, try again later" });
            }
            else return Json(new { success = false, alert = "You've no permission to remove this notification. Please, sign in or sign up to remove and edit your notifications" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAllNotifications(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                bool Result = await _notificationsRepository.RemoveAllNotifications(Id);
                if (Result) return Json(new { success = true, alert = "All notifications have successfully removed" });
                else return Json(new { success = false, alert = "An error occured while trying to remove your notifications. Please, try again later" });
            }
            return Json(new { success = false, alert = "You haven't got access to remove any notification. Please, sign in or sign up to get that access" });
        }
    }
}

using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationY.Controllers
{
    public class SessionController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly ISessionInfo _sessionRepository;

        public SessionController(Context context, UserManager<User> userManager, ISessionInfo sessionRepository)
        {
            _context = context;
            _userManager = userManager;
            _sessionRepository = sessionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> TerminateAllSessions()
        {
            if(User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if(UserInfo != null)
                {
                    bool Result = await _sessionRepository.TerminateAllSessionsAsync(UserInfo.Id);
                    if (Result) return Json(new { success = true, alert = "All other sessions were successfully terminated" });
                }
            }
            return Json(new { success = false, alert = "Unable to terminate all other sessions" });
        }
    }
}

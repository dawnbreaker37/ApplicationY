using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using ApplicationY.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Controllers
{
    public class SettingsController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IOthers _othersRepository;
        private readonly IMessage _messagesRepository;
        public SettingsController(Context context, UserManager<User> userManager, IOthers othersRepository, IMessage messagesRepository)
        {
            _context = context;
            _userManager = userManager;
            _othersRepository = othersRepository;
            _messagesRepository = messagesRepository;   
        }

        public async Task<IActionResult> MainAdministrationPanel()
        {
            if(User.Identity.IsAuthenticated)
            {
                User? UserInfo = await _userManager.GetUserAsync(User);
                if(UserInfo != null)
                {
                    int UserRole = await _othersRepository.GetUserRoleAsync(UserInfo.Id);
                    if(UserRole >= 3)
                    {
                        List<GetUserRole_ViewModel>? GetAllRoles_Result = null;
                        IQueryable<GetUserRole_ViewModel>? GetAllRoles = _othersRepository.GetAllRoles(UserRole);
                        GetUserRole_ViewModel? FullRoleInfo = await _othersRepository.GetUserFullRoleInfoAsync(UserInfo.Id, UserRole);
                        if(GetAllRoles != null)
                        {
                            GetAllRoles_Result = await GetAllRoles.ToListAsync();
                        }
                        int MessagesCount = await _messagesRepository.GetReceivedMessagesCount(UserInfo.Id, -256, false);

                        ViewBag.UserInfo = UserInfo;
                        ViewBag.FullRoleInfo = FullRoleInfo;
                        ViewBag.RoleId = UserRole;
                        ViewBag.MessagesCount = MessagesCount;
                        ViewBag.AllRoles = GetAllRoles_Result;

                        return View();
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> GetRemovedProjectInfo(int Id)
        {
            DisabledProject? ProjectInfo = await _othersRepository.DisabledProjectInfoAsync(Id);
            if (ProjectInfo != null) return Json(new { success = true, result = ProjectInfo });
            else return Json(new { success = false, alert = "We're sorry but we aren't able to show you the info of this project. You may try again later, thank you! <br/> If you have some questions or issues, please send a message to our email or use in-app chat" });
        }
    }
}

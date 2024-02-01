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
        public SettingsController(Context context, UserManager<User> userManager, IOthers othersRepository)
        {
            _context = context;
            _userManager = userManager;
            _othersRepository = othersRepository;
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

                        ViewBag.UserInfo = UserInfo;
                        ViewBag.FullRoleInfo = FullRoleInfo;
                        ViewBag.RoleId = UserRole;
                        ViewBag.AllRoles = GetAllRoles_Result;

                        return View();
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}

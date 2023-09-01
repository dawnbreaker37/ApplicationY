using ApplicationY.Interfaces;
using ApplicationY.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ApplicationY.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUser _user;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUser user)
        {
            _user = user;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
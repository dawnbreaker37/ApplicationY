using ApplicationY.Data;
using ApplicationY.Interfaces;
using ApplicationY.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationY.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly Context _context;
        private readonly ICategory _categoriesRepository;

        public CategoriesController(Context context, ICategory categoriesRepository)
        {
            _context = context;
            _categoriesRepository = categoriesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            IQueryable<Category>? AllCategories_Preview = _categoriesRepository.GetAll();
            if(AllCategories_Preview != null)
            {
                List<Category>? Categories = await AllCategories_Preview.ToListAsync();
                int CategoriesCount = Categories.Count;

                if (Categories != null) return Json(new { success = true, categories = Categories, count = CategoriesCount });
            }
             return Json(new { success = false, count = 0 });
        }
    }
}

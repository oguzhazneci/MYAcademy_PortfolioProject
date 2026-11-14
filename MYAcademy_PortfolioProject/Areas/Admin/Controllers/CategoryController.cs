using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly PortfolioContext _context;

        public CategoryController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult CategoryList()
        {
            var values = _context.Categories
                .Include(x => x.Projects)
                .OrderBy(x => x.CategoryName)
                .ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View(new Category());
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Categories.Add(category);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Kategori eklendi.";
            return RedirectToAction(nameof(CategoryList));
        }

        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            var value = _context.Categories.Find(id);
            if (value == null)
            {
                return NotFound();
            }
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Categories.Update(category);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Kategori güncellendi.";
            return RedirectToAction(nameof(CategoryList));
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var value = _context.Categories.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            // mig3'te Cascade Delete tanımlı: kategori silinirse projeleri de silinir. Buna izin vermiyoruz.
            if (_context.Projects.Any(x => x.CategoryId == id))
            {
                TempData["ErrorMessage"] = $"\"{value.CategoryName}\" kategorisinde projeler var. Önce bu projeleri silin veya başka bir kategoriye taşıyın.";
                return RedirectToAction(nameof(CategoryList));
            }

            _context.Categories.Remove(value);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Kategori silindi.";
            return RedirectToAction(nameof(CategoryList));
        }
    }
}
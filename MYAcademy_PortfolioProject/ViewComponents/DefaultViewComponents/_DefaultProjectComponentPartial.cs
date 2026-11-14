using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Models;

namespace MYAcademy_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultProjectComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultProjectComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Pasif projeler sitede listelenmez; en yeni proje en üstte.
            var projects = _context.Projects
                .Include(x => x.Category)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            if (!projects.Any())
            {
                return Content(string.Empty);
            }

            // Filtre butonlarını projelerin kendi kategorilerinden türetiyoruz.
            // Böylece içinde aktif proje olmayan bir kategori için "boş sonuç" veren buton oluşmaz.
            var categories = projects
                .Select(x => x.Category!)
                .DistinctBy(x => x.CategoryId)
                .OrderBy(x => x.CategoryName)
                .ToList();

            var model = new ProjectSectionViewModel
            {
                Projects = projects,
                Categories = categories
            };

            return View(model);
        }
    }
}
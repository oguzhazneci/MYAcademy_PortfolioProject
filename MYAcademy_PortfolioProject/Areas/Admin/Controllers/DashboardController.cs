using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Models;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly PortfolioContext _context;

        public DashboardController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                // Count: tablodaki satır sayısı (SQL: SELECT COUNT(*))
                TotalProjectCount = _context.Projects.Count(),

                // Count + koşul: Where(...).Count() ile aynı sonucu verir, daha kısa yazımı
                ActiveProjectCount = _context.Projects.Count(x => x.IsActive),

                SkillCount = _context.SkillItems.Count(),
                ExperienceCount = _context.Experiences.Count(),
                EducationCount = _context.Educations.Count(),
                TotalMessageCount = _context.Messages.Count(),
                UnreadMessageCount = _context.Messages.Count(x => !x.IsRead),

                // GroupBy: projeleri kategori adına göre grupla, her grubun eleman sayısını al
                // (SQL: SELECT c.CategoryName, COUNT(*) ... GROUP BY c.CategoryName)
                CategorySummary = _context.Projects
                    .GroupBy(x => x.Category!.CategoryName)
                    .Select(g => new CategoryProjectCount
                    {
                        CategoryName = g.Key,
                        ProjectCount = g.Count()
                    })
                    .OrderByDescending(x => x.ProjectCount)
                    .ToList(),

                // OrderByDescending + Take: en yeniden eskiye sırala, ilk 5'ini al (SQL: SELECT TOP 5 ... ORDER BY ... DESC)
                LastMessages = _context.Messages
                    .OrderByDescending(x => x.SendDate)
                    .Take(5)
                    .ToList(),

                LastProjects = _context.Projects
                    .Include(x => x.Category)
                    .OrderByDescending(x => x.CreatedDate)
                    .Take(3)
                    .ToList()
            };

            return View(model);
        }
    }
}
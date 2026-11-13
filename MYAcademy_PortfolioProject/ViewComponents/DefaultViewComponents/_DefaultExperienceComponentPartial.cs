    using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;

namespace MYAcademy_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultExperienceComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultExperienceComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Sadece aktif deneyimler, en güncelden en eskiye.
            var values = _context.Experiences
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.StartDate)
                .ToList();

            // Gösterilecek deneyim yoksa bölümü hiç basma (boş bir zaman çizelgesi göstermektense).
            if (!values.Any())
            {
                return Content(string.Empty);
            }

            return View(values);
        }
    }
}

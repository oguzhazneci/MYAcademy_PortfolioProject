using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;

namespace MYAcademy_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultEducationComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultEducationComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Sadece aktif kayıtlar, en güncelden en eskiye.
            var values = _context.Educations
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.StartYear)
                .ToList();

            if (!values.Any())
            {
                return Content(string.Empty);
            }

            return View(values);
        }
    }
}
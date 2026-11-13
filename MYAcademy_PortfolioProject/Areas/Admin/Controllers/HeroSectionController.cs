using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HeroSectionController : Controller
    {
        private readonly PortfolioContext _context;

        public HeroSectionController(PortfolioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult UpdateHeroSection()
        {
            var value = _context.HeroSections.FirstOrDefault() ?? new HeroSection();
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateHeroSection(HeroSection heroSection)
        {
            if (!ModelState.IsValid)
            {
                return View(heroSection);
            }

            if (heroSection.HeroSectionId == 0)
            {
                _context.HeroSections.Add(heroSection);
            }
            else
            {
                _context.HeroSections.Update(heroSection);
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Ana sayfa (Hero) bilgileri kaydedildi.";
            return RedirectToAction(nameof(UpdateHeroSection));
        }
    }
}
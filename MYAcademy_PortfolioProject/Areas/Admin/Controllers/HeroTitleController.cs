using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HeroTitleController : Controller
    {
        private readonly PortfolioContext _context;

        public HeroTitleController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult HeroTitleList()
        {
            var values = _context.HeroTitles.OrderBy(x => x.Order).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateHeroTitle()
        {
            int nextOrder = (_context.HeroTitles.Max(x => (int?)x.Order) ?? 0);
            var model = new HeroTitle
            { Order = nextOrder + 1, IsActive = true };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateHeroTitle(HeroTitle heroTitle)
        {
            if (!ModelState.IsValid)
            {
                return View(heroTitle);
            }

            _context.HeroTitles.Add(heroTitle);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Yazı eklendi.";
            return RedirectToAction(nameof(HeroTitleList));
        }

        [HttpGet]
        public IActionResult UpdateHeroTitle(int id)
        {
            var value = _context.HeroTitles.Find(id);

            if (value == null)
            {
                return NotFound();
            }
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateHeroTitle(HeroTitle heroTitle)
        {
            if(!ModelState.IsValid)
            {
                return View(heroTitle);
            }

            _context.HeroTitles.Update(heroTitle);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Yazı güncellendi";
            return RedirectToAction(nameof(HeroTitleList));
        }

        [HttpPost]
        public IActionResult ToggleHeroTitleStatus(int id)
        {
            var value = _context.HeroTitles.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            value.IsActive = !value.IsActive;
            _context.SaveChanges();
            TempData["SuccessMessage"] = value.IsActive ? "Yazı aktif edildi." : "Yazı pasife alındı.";
            return RedirectToAction(nameof(HeroTitleList));
        }

        [HttpPost]
        public IActionResult DeleteHeroTitle(int id)
        {
            var value = _context.HeroTitles.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            _context.HeroTitles.Remove(value);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Yazı silindi.";
            return RedirectToAction(nameof(HeroTitleList));
        }
    }
}

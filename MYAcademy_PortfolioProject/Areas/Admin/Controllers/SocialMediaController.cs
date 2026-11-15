using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SocialMediaController : Controller
    {
        private readonly PortfolioContext _context;

        public SocialMediaController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult SocialMediaList()
        {
            var values = _context.SocialMedias.OrderBy(x => x.Order).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateSocialMedia()
        {
            int nextOrder = (_context.SocialMedias.Max(x => (int?)x.Order) ?? 0) + 1;
            return View(new SocialMedia { Order = nextOrder, IsActive = true });
        }

        [HttpPost]
        public IActionResult CreateSocialMedia(SocialMedia socialMedia)
        {
            if (!ModelState.IsValid)
            {
                return View(socialMedia);
            }

            _context.SocialMedias.Add(socialMedia);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Sosyal medya bağlantısı eklendi.";
            return RedirectToAction(nameof(SocialMediaList));
        }

        [HttpGet]
        public IActionResult UpdateSocialMedia(int id)
        {
            var value = _context.SocialMedias.Find(id);
            if (value == null)
            {
                return NotFound();
            }
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateSocialMedia(SocialMedia socialMedia)
        {
            if (!ModelState.IsValid)
            {
                return View(socialMedia);
            }

            // Bütün alanlar formda olduğu için Update() burada güvenli.
            _context.SocialMedias.Update(socialMedia);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Sosyal medya bağlantısı güncellendi.";
            return RedirectToAction(nameof(SocialMediaList));
        }

        [HttpPost]
        public IActionResult ToggleSocialMediaStatus(int id)
        {
            var value = _context.SocialMedias.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            value.IsActive = !value.IsActive;
            _context.SaveChanges();
            TempData["SuccessMessage"] = value.IsActive ? "Bağlantı aktif edildi." : "Bağlantı pasife alındı.";
            return RedirectToAction(nameof(SocialMediaList));
        }

        [HttpPost]
        public IActionResult DeleteSocialMedia(int id)
        {
            var value = _context.SocialMedias.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            _context.SocialMedias.Remove(value);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Sosyal medya bağlantısı silindi.";
            return RedirectToAction(nameof(SocialMediaList));
        }
    }
}
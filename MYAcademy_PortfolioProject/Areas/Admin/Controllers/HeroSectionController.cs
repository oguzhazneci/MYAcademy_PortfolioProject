using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;
using MYAcademy_PortfolioProject.Services;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HeroSectionController : Controller
    {
        private readonly PortfolioContext _context;
        private readonly ImageUploadService _imageUploadService;

        public HeroSectionController(PortfolioContext context, ImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        [HttpGet]
        public IActionResult UpdateHeroSection()
        {
            var value = _context.HeroSections.FirstOrDefault() ?? new HeroSection();
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateHeroSection(HeroSection heroSection, IFormFile? imageFile)
        {
            // Görsel artık formdan metin olarak gelmiyor. Entity'deki [Required] kuralı bu alan için
            // yanlış alarm verir; o kontrolü ModelState'ten çıkarıp aşağıda kendimiz yapıyoruz.
            ModelState.Remove(nameof(HeroSection.MainImageUrl));

            // Mevcut görselin yolunu öğrenmek için kaydı okuyoruz.
            // AsNoTracking: EF bu nesneyi takip etmesin; aşağıdaki Update(heroSection) ile çakışmasın.
            string? currentImageUrl = _context.HeroSections
                .AsNoTracking()
                .Where(x => x.HeroSectionId == heroSection.HeroSectionId)
                .Select(x => x.MainImageUrl)
                .FirstOrDefault();

            if (imageFile != null)
            {
                string? error = _imageUploadService.Validate(imageFile);
                if (error != null)
                {
                    ModelState.AddModelError("imageFile", error);
                }
            }
            else if (string.IsNullOrEmpty(currentImageUrl))
            {
                // İlk kayıtta (veya daha önce hiç görsel yoksa) görsel zorunlu.
                ModelState.AddModelError("imageFile", "Lütfen bir görsel seçin.");
            }

            if (!ModelState.IsValid)
            {
                heroSection.MainImageUrl = currentImageUrl!;   // önizleme kaybolmasın
                return View(heroSection);
            }

            string? oldImageUrl = null;
            if (imageFile != null)
            {
                oldImageUrl = currentImageUrl;
                heroSection.MainImageUrl = _imageUploadService.Save(imageFile, "profile");
            }
            else
            {
                heroSection.MainImageUrl = currentImageUrl!;   // yeni görsel seçilmediyse mevcut korunur
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

            // Eski görsel sadece /uploads/ altındaysa silinir (elle konan /images/ dosyalarına dokunulmaz).
            _imageUploadService.Delete(oldImageUrl);

            TempData["SuccessMessage"] = "Ana sayfa (Hero) bilgileri kaydedildi.";
            return RedirectToAction(nameof(UpdateHeroSection));
        }
    }
}
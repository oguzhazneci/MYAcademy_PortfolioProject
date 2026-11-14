using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;
using MYAcademy_PortfolioProject.Services;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly PortfolioContext _context;
        private readonly ImageUploadService _imageUploadService;

        public AboutController(PortfolioContext context, ImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        [HttpGet]
        public IActionResult UpdateAbout()
        {
            var value = _context.Abouts.FirstOrDefault() ?? new About();
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateAbout(About about, IFormFile? imageFile)
        {
            // Görsel artık formdan metin olarak gelmiyor. Entity'deki [Required] kuralı bu alan için
            // yanlış alarm verir; o kontrolü ModelState'ten çıkarıp aşağıda kendimiz yapıyoruz.
            ModelState.Remove(nameof(About.ImageUrl));

            // Mevcut görselin yolunu öğrenmek için kaydı okuyoruz.
            // AsNoTracking: EF bu nesneyi takip etmesin; aşağıdaki Update(about) ile çakışmasın.
            string? currentImageUrl = _context.Abouts
                .AsNoTracking()
                .Where(x => x.AboutId == about.AboutId)
                .Select(x => x.ImageUrl)
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
                about.ImageUrl = currentImageUrl!;   // önizleme kaybolmasın
                return View(about);
            }

            string? oldImageUrl = null;
            if (imageFile != null)
            {
                oldImageUrl = currentImageUrl;
                about.ImageUrl = _imageUploadService.Save(imageFile, "profile");
            }
            else
            {
                about.ImageUrl = currentImageUrl!;   // yeni görsel seçilmediyse mevcut korunur
            }

            if (about.AboutId == 0)
            {
                _context.Abouts.Add(about);
            }
            else
            {
                _context.Abouts.Update(about);
            }

            _context.SaveChanges();

            // Eski görsel sadece /uploads/ altındaysa silinir (elle konan /images/ dosyalarına dokunulmaz).
            _imageUploadService.Delete(oldImageUrl);

            TempData["SuccessMessage"] = "Hakkımda bilgileri kaydedildi.";
            return RedirectToAction(nameof(UpdateAbout));
        }
    }
}
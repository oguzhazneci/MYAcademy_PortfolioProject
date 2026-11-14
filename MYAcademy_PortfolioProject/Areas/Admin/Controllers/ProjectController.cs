using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;
using MYAcademy_PortfolioProject.Services;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly PortfolioContext _context;
        private readonly ImageUploadService _imageUploadService;

        public ProjectController(PortfolioContext context, ImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        // Kategori dropdown'unu hazırlar. selectedId verilirse o kategori seçili gelir.
        private SelectList GetCategorySelectList(int? selectedId = null)
        {
            var categories = _context.Categories.OrderBy(x => x.CategoryName).ToList();
            return new SelectList(categories, "CategoryId", "CategoryName", selectedId);
        }

        // Adres çubuğundan gelir: /Admin/Project/ProjectList?categoryId=2&sort=title
        public IActionResult ProjectList(int? categoryId, string sort = "newest")
        {
            // IQueryable: sorgu henüz çalışmadı; koşullara göre parça parça ekleyip en sonda tek seferde çalıştırıyoruz.
            IQueryable<Project> query = _context.Projects.Include(x => x.Category);

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            query = sort switch
            {
                "oldest" => query.OrderBy(x => x.CreatedDate),
                "title" => query.OrderBy(x => x.ProjectTitle),
                _ => query.OrderByDescending(x => x.CreatedDate)   // varsayılan: en yeni
            };

            // Filtre formunun seçili değerleri koruyabilmesi için mevcut durumu view'a geri gönderiyoruz.
            ViewBag.Categories = GetCategorySelectList(categoryId);
            ViewBag.Sort = sort;

            return View(query.ToList());
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            ViewBag.Categories = GetCategorySelectList();
            return View(new Project { IsActive = true });
        }

        [HttpPost]
        public IActionResult CreateProject(Project project, IFormFile? imageFile)
        {
            if (imageFile == null)
            {
                ModelState.AddModelError("imageFile", "Proje görseli zorunludur.");
            }
            else
            {
                string? error = _imageUploadService.Validate(imageFile);
                if (error != null)
                {
                    ModelState.AddModelError("imageFile", error);
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetCategorySelectList();
                return View(project);
            }

            // Doğrulamadan geçtiyse dosyayı kaydet; veritabanına sadece yolunu yaz.
            project.ImageUrl = _imageUploadService.Save(imageFile!, "projects");

            // Kullanıcıdan alınmayan alan: sunucu atıyor.
            project.CreatedDate = DateTime.Now;

            _context.Projects.Add(project);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Proje eklendi.";
            return RedirectToAction(nameof(ProjectList));
        }

        [HttpGet]
        public IActionResult UpdateProject(int id)
        {
            var value = _context.Projects.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            ViewBag.Categories = GetCategorySelectList();
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateProject(Project project, IFormFile? imageFile)
        {
            var value = _context.Projects.Find(project.ProjectId);
            if (value == null)
            {
                return NotFound();
            }

            // Güncellerken görsel isteğe bağlı: seçildiyse doğrula, seçilmediyse mevcut görsel kalır.
            if (imageFile != null)
            {
                string? error = _imageUploadService.Validate(imageFile);
                if (error != null)
                {
                    ModelState.AddModelError("imageFile", error);
                }
            }

            if (!ModelState.IsValid)
            {
                // Form tekrar gösterilirken mevcut görselin önizlemesi kaybolmasın.
                project.ImageUrl = value.ImageUrl;
                ViewBag.Categories = GetCategorySelectList();
                return View(project);
            }

            value.ProjectTitle = project.ProjectTitle;
            value.Description = project.Description;
            value.Technologies = project.Technologies;
            value.ProjectUrl = project.ProjectUrl;
            value.CategoryId = project.CategoryId;
            value.IsActive = project.IsActive;

            string? oldImageUrl = null;
            if (imageFile != null)
            {
                oldImageUrl = value.ImageUrl;
                value.ImageUrl = _imageUploadService.Save(imageFile, "projects");
            }

            // Update() çağırmaya gerek yok: EF, Find ile getirdiği nesnedeki değişiklikleri kendisi takip ediyor.
            _context.SaveChanges();

            // Eski dosyayı ancak veritabanı başarıyla güncellendikten SONRA siliyoruz.
            _imageUploadService.Delete(oldImageUrl);

            TempData["SuccessMessage"] = "Proje güncellendi.";
            return RedirectToAction(nameof(ProjectList));
        }

        [HttpPost]
        public IActionResult ToggleProjectStatus(int id)
        {
            var value = _context.Projects.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            value.IsActive = !value.IsActive;
            _context.SaveChanges();
            TempData["SuccessMessage"] = value.IsActive ? "Proje aktif edildi." : "Proje pasife alındı.";
            return RedirectToAction(nameof(ProjectList));
        }

        [HttpPost]
        public IActionResult DeleteProject(int id)
        {
            var value = _context.Projects.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(value);
            _context.SaveChanges();

            // Kayıt silindi; artık kimsenin göstermeyeceği görsel dosyasını da sil.
            _imageUploadService.Delete(value.ImageUrl);

            TempData["SuccessMessage"] = "Proje silindi.";
            return RedirectToAction(nameof(ProjectList));
        }
    }
}
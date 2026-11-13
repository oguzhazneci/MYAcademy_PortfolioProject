using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExperienceController : Controller
    {
        private readonly PortfolioContext _context;

        public ExperienceController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult ExperienceList()
        {
            // En güncel deneyim en üstte.
            var values = _context.Experiences.OrderByDescending(x => x.StartDate).ToList();
            return View(values);
        }

        // Data Annotations tek bir alanı kontrol eder; iki alanı birbiriyle karşılaştıran kuralı biz ekliyoruz.
        private void ValidateDates(Experience experience)
        {
            if (experience.StartDate.HasValue && experience.EndDate.HasValue
                && experience.EndDate < experience.StartDate)
            {
                ModelState.AddModelError(nameof(Experience.EndDate), "Bitiş tarihi, başlangıç tarihinden önce olamaz.");
            }
        }

        [HttpGet]
        public IActionResult CreateExperience()
        {
            return View(new Experience { IsActive = true });
        }

        [HttpPost]
        public IActionResult CreateExperience(Experience experience)
        {
            ValidateDates(experience);
            if (!ModelState.IsValid)
            {
                return View(experience);
            }

            _context.Experiences.Add(experience);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Deneyim eklendi.";
            return RedirectToAction(nameof(ExperienceList));
        }

        [HttpGet]
        public IActionResult UpdateExperience(int id)
        {
            var value = _context.Experiences.Find(id);
            if (value == null)
            {
                return NotFound();
            }
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateExperience(Experience experience)
        {
            ValidateDates(experience);
            if (!ModelState.IsValid)
            {
                return View(experience);
            }

            _context.Experiences.Update(experience);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Deneyim güncellendi.";
            return RedirectToAction(nameof(ExperienceList));
        }

        [HttpPost]
        public IActionResult ToggleExperienceStatus(int id)
        {
            var value = _context.Experiences.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            value.IsActive = !value.IsActive;
            _context.SaveChanges();
            TempData["SuccessMessage"] = value.IsActive ? "Deneyim aktif edildi." : "Deneyim pasife alındı.";
            return RedirectToAction(nameof(ExperienceList));
        }

        [HttpPost]
        public IActionResult DeleteExperience(int id)
        {
            var value = _context.Experiences.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            _context.Experiences.Remove(value);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Deneyim silindi.";
            return RedirectToAction(nameof(ExperienceList));
        }
    }
}
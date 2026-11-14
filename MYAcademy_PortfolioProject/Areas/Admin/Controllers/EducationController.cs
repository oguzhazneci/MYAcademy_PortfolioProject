using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EducationController : Controller
    {
        private readonly PortfolioContext _context;

        public EducationController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult EducationList()
        {
            var values = _context.Educations.OrderByDescending(x => x.StartYear).ToList();
            return View(values);
        }

        // Bitiş yılı, başlangıç yılından önce olamaz (iki alanı karşılaştıran kural).
        private void ValidateYears(Education education)
        {
            if (education.StartYear.HasValue && education.EndYear.HasValue
                && education.EndYear < education.StartYear)
            {
                ModelState.AddModelError(nameof(Education.EndYear), "Bitiş yılı, başlangıç yılından önce olamaz.");
            }
        }

        [HttpGet]
        public IActionResult CreateEducation()
        {
            return View(new Education { IsActive = true });
        }

        [HttpPost]
        public IActionResult CreateEducation(Education education)
        {
            ValidateYears(education);
            if (!ModelState.IsValid)
            {
                return View(education);
            }

            _context.Educations.Add(education);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Eğitim bilgisi eklendi.";
            return RedirectToAction(nameof(EducationList));
        }

        [HttpGet]
        public IActionResult UpdateEducation(int id)
        {
            var value = _context.Educations.Find(id);
            if (value == null)
            {
                return NotFound();
            }
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateEducation(Education education)
        {
            ValidateYears(education);
            if (!ModelState.IsValid)
            {
                return View(education);
            }

            _context.Educations.Update(education);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Eğitim bilgisi güncellendi.";
            return RedirectToAction(nameof(EducationList));
        }

        [HttpPost]
        public IActionResult ToggleEducationStatus(int id)
        {
            var value = _context.Educations.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            value.IsActive = !value.IsActive;
            _context.SaveChanges();
            TempData["SuccessMessage"] = value.IsActive ? "Eğitim bilgisi aktif edildi." : "Eğitim bilgisi pasife alındı.";
            return RedirectToAction(nameof(EducationList));
        }

        [HttpPost]
        public IActionResult DeleteEducation(int id)
        {
            var value = _context.Educations.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            _context.Educations.Remove(value);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Eğitim bilgisi silindi.";
            return RedirectToAction(nameof(EducationList));
        }
    }
}
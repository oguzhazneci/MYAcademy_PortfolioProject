using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SkillController : Controller
    {
        private readonly PortfolioContext _context;

        public SkillController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult SkillList()
        {
            // Include: her kategorinin yeteneklerini de aynı sorguda getir (listede yetenek sayısını göstereceğiz).
            var values = _context.Skills.Include(x => x.SkillItems).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateSkill()
        {
            return View(new Skill());
        }

        [HttpPost]
        public IActionResult CreateSkill(Skill skill)
        {
            if (!ModelState.IsValid)
            {
                return View(skill);
            }

            _context.Skills.Add(skill);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Kategori eklendi.";
            return RedirectToAction(nameof(SkillList));
        }

        [HttpGet]
        public IActionResult UpdateSkill(int id)
        {
            var value = _context.Skills.Find(id);
            if (value == null)
            {
                return NotFound();
            }
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateSkill(Skill skill)
        {
            if (!ModelState.IsValid)
            {
                return View(skill);
            }

            _context.Skills.Update(skill);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Kategori güncellendi.";
            return RedirectToAction(nameof(SkillList));
        }

        [HttpPost]
        public IActionResult DeleteSkill(int id)
        {
            var value = _context.Skills.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            // Veritabanında Cascade Delete var: kategori silinirse içindeki tüm yetenekler de sessizce silinir.
            // Bunu engellemek için içinde yetenek olan kategorinin silinmesine izin vermiyoruz.
            if (_context.SkillItems.Any(x => x.SkillId == id))
            {
                TempData["ErrorMessage"] = $"\"{value.Title}\" kategorisinde yetenekler var. Önce bu yetenekleri silin veya başka bir kategoriye taşıyın.";
                return RedirectToAction(nameof(SkillList));
            }

            _context.Skills.Remove(value);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Kategori silindi.";
            return RedirectToAction(nameof(SkillList));
        }
    }
}
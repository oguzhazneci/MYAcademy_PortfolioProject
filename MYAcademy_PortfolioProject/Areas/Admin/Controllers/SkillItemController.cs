using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SkillItemController : Controller
    {
        private readonly PortfolioContext _context;

        public SkillItemController(PortfolioContext context)
        {
            _context = context;
        }

        // Dropdown için kategori listesini hazırlar. Formu gösteren her yerde (GET'ler ve hatalı POST'lar) çağrılmalı.
        private void LoadSkillDropdown()
        {
            var skills = _context.Skills.OrderBy(x => x.Title).ToList();

            // SelectList(kaynak, değer olarak kullanılacak alan, ekranda görünecek alan)
            ViewBag.Skills = new SelectList(skills, "SkillId", "Title");
        }

        public IActionResult SkillItemList()
        {
            // Include ile her yeteneğin kategorisini de getiriyoruz; önce kategoriye, sonra seviyeye göre sıralıyoruz.
            var values = _context.SkillItems
                .Include(x => x.Skill)
                .OrderBy(x => x.Skill!.Title)
                .ThenByDescending(x => x.Level)
                .ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateSkillItem()
        {
            LoadSkillDropdown();
            return View(new SkillItem { Level = 50 });
        }

        [HttpPost]
        public IActionResult CreateSkillItem(SkillItem skillItem)
        {
            if (!ModelState.IsValid)
            {
                // Form tekrar gösterilecek; dropdown verisi de tekrar hazırlanmalı, yoksa sayfa hata verir.
                LoadSkillDropdown();
                return View(skillItem);
            }

            _context.SkillItems.Add(skillItem);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Yetenek eklendi.";
            return RedirectToAction(nameof(SkillItemList));
        }

        [HttpGet]
        public IActionResult UpdateSkillItem(int id)
        {
            var value = _context.SkillItems.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            LoadSkillDropdown();
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateSkillItem(SkillItem skillItem)
        {
            if (!ModelState.IsValid)
            {
                LoadSkillDropdown();
                return View(skillItem);
            }

            _context.SkillItems.Update(skillItem);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Yetenek güncellendi.";
            return RedirectToAction(nameof(SkillItemList));
        }

        [HttpPost]
        public IActionResult DeleteSkillItem(int id)
        {
            var value = _context.SkillItems.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            _context.SkillItems.Remove(value);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Yetenek silindi.";
            return RedirectToAction(nameof(SkillItemList));
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_PortfolioProject.Context;

namespace MYAcademy_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultSkillComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultSkillComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Skills
                // Filtreli Include: yetenekleri getirirken kendi içlerinde seviyeye göre (yüksekten düşüğe) sırala.
                .Include(x => x.SkillItems!.OrderByDescending(i => i.Level))
                // İçinde hiç yetenek olmayan kategori sitede boş kart olarak görünmesin.
                .Where(x => x.SkillItems!.Any())
                // Kartlar eklenme sırasıyla dizilsin.
                .OrderBy(x => x.SkillId)
                .ToList();

            return View(values);
        }
    }
}
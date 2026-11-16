using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_PortfolioProject.Context;

namespace MYAcademy_PortfolioProject.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _AdminLayoutNavbarComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _AdminLayoutNavbarComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Zil ikonundaki nokta sadece okunmamış mesaj varsa görünsün.
            ViewBag.HasUnread = _context.Messages.Any(x => !x.IsRead);

            // Panelde görünen ad ve baş harfler Hakkımda kaydından gelir (sabit "Admin Kullanıcı" yerine).
            var about = _context.Abouts.AsNoTracking().FirstOrDefault();
            ViewBag.FullName = about?.FullName ?? "Yönetici";
            ViewBag.Initials = about?.Initials ?? "A";

            return View();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_PortfolioProject.Context;

namespace MYAcademy_PortfolioProject.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _AdminLayoutSidebarComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _AdminLayoutSidebarComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Sidebar her admin sayfasında çizildiği için bu sayı her sayfada güncel olur.
            ViewBag.UnreadCount = _context.Messages.Count(x => !x.IsRead);

            // Panelde görünen ad ve baş harfler Hakkımda kaydından gelir (sabit "Admin Kullanıcı" yerine).
            var about = _context.Abouts.AsNoTracking().FirstOrDefault();
            ViewBag.FullName = about?.FullName ?? "Yönetici";
            ViewBag.Initials = about?.Initials ?? "A";

            return View();
        }
    }
}
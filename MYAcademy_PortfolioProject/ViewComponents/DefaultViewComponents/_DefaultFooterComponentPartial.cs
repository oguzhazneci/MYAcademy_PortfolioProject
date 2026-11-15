using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;

namespace MYAcademy_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultFooterComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultFooterComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Footer'daki isim Hakkımda'dan gelir; kayıt yoksa boş kalmasın diye yedek bir metin kullanılır.
            ViewBag.fullName = _context.Abouts.Select(x => x.FullName).FirstOrDefault() ?? "Portfolyo";
            return View();
        }
    }
}
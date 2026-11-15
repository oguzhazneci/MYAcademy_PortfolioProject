using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;

namespace MYAcademy_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultSocialMediaComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultSocialMediaComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        // style parametresi: "pill" → yuvarlak ikon butonlar (Hero, İletişim)
        //                    "footer" → küçük, yazılı linkler (Footer)
        public IViewComponentResult Invoke(string style = "pill")
        {
            var values = _context.SocialMedias
                .Where(x => x.IsActive)
                .OrderBy(x => x.Order)
                .ToList();

            if (!values.Any())
            {
                return Content(string.Empty);
            }

            // Aynı veri, iki farklı görünüm: Default.cshtml veya Footer.cshtml
            string viewName = style == "footer" ? "Footer" : "Default";
            return View(viewName, values);
        }
    }
}
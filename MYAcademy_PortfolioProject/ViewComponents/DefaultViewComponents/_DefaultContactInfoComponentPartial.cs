using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;

namespace MYAcademy_PortfolioProject.ViewComponents.DefaultViewComponents
{
    public class _DefaultContactInfoComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;

        public _DefaultContactInfoComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var value = _context.Contacts.FirstOrDefault();
            if (value == null)
            {
                return Content(string.Empty);
            }
            return View(value);
        }
    }
}
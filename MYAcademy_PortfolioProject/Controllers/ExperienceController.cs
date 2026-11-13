using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Controllers
{
    public class ExperienceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

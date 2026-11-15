using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly PortfolioContext _context;

        public ContactController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult ContactList()
        {
            return RedirectToAction(nameof(UpdateContact));
        }

        [HttpGet]
        public IActionResult CreateContact()
        {
            return RedirectToAction(nameof(UpdateContact));
        }

        [HttpGet]
        public IActionResult UpdateContact()
        {
            var value = _context.Contacts.FirstOrDefault() ?? new Contact();
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }

            if (contact.ContactId == 0)
            {
                _context.Contacts.Add(contact);
            }
            else
            {
                _context.Contacts.Update(contact);
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "İletişim bilgileri kaydedildi.";
            return RedirectToAction(nameof(UpdateContact));
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;
using MYAcademy_PortfolioProject.Models;
using Microsoft.AspNetCore.RateLimiting;

namespace MYAcademy_PortfolioProject.Controllers
{
    public class DefaultController : Controller
    {
        private readonly PortfolioContext _context;
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(PortfolioContext context, ILogger<DefaultController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Sayfa ilk açıldığında iletişim formu boş bir modelle çizilir.
            return View(new SendMessageViewModel());
        }

        // Birisi /Default/SendMessage adresini tarayıcıya yazarsa (veya hatalı formdan sonra F5 yerine adresi açarsa)
        // 404 görmesin; iletişim bölümüne yönlendirilsin.
        [HttpGet]
        public IActionResult SendMessage()
        {
            return RedirectToAction(nameof(Index), null, null, "contact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]   // Formun gerçekten bu siteden gönderildiğini doğrular (CSRF koruması).
        [EnableRateLimiting("contact-form")]   // Program.cs'teki politika: IP başına 10 dakikada 3 gönderim
        public IActionResult SendMessage(SendMessageViewModel model)
        {
            // Honeypot doluysa gönderen büyük ihtimalle bir bot.
            // Hata vermiyoruz, başarılı gibi davranıyoruz: bot neyin onu ele verdiğini öğrenmesin.
            if (!string.IsNullOrEmpty(model.Website))
            {
                _logger.LogWarning("İletişim formunda spam engellendi (honeypot dolu). IP: {Ip}",
                    HttpContext.Connection.RemoteIpAddress);

                TempData["ContactSuccess"] = "Mesajınız iletildi. En kısa sürede size dönüş yapacağım.";
                return RedirectToAction(nameof(Index), null, null, "contact");
            }

            if (!ModelState.IsValid)
            {
                // Hatalarla birlikte aynı sayfayı geri çiz: ziyaretçinin yazdıkları kaybolmasın.
                return View(nameof(Index), model);
            }

            // ViewModel → Entity: ziyaretçiden gelenleri kopyala, geri kalanını sunucu belirler.
            var message = new Message
            {
                SenderName = model.SenderName.Trim(),
                SenderEmail = model.SenderEmail.Trim(),
                Subject = string.IsNullOrWhiteSpace(model.Subject) ? "(Konu belirtilmedi)" : model.Subject.Trim(),
                MessageDetail = model.MessageDetail.Trim(),
                SendDate = DateTime.Now,
                IsRead = false
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            TempData["ContactSuccess"] = "Mesajınız iletildi. En kısa sürede size dönüş yapacağım.";

            // Post-Redirect-Get + "#contact": sayfa yenilenince ziyaretçi doğrudan iletişim bölümüne iner.
            return RedirectToAction(nameof(Index), null, null, "contact");
        }
    }
}
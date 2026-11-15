using Microsoft.AspNetCore.Mvc;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MessageController : Controller
    {
        private readonly PortfolioContext _context;
        public MessageController(PortfolioContext context)
        {
            _context = context;
        }

        // /Admin/Message/MessageList?search=ahmet&status=unread
        public IActionResult MessageList(string? search, string status = "all")
        {
            IQueryable<Message> query = _context.Messages;

            // Arama: gönderen adı, e-postası veya konu içinde geçiyorsa (SQL'de LIKE '%...%').
            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim();
                query = query.Where(x => x.SenderName.Contains(term)
                                      || x.SenderEmail.Contains(term)
                                      || x.Subject.Contains(term));
            }

            // Durum filtresi
            if (status == "unread")
            {
                query = query.Where(x => !x.IsRead);
            }
            else if (status == "read")
            {
                query = query.Where(x => x.IsRead);
            }

            // Filtre formunun seçimleri koruyabilmesi ve özet bilgi için
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.TotalCount = _context.Messages.Count();
            ViewBag.UnreadCount = _context.Messages.Count(x => !x.IsRead);

            // En yeni mesaj en üstte
            var values = query.OrderByDescending(x => x.SendDate).ToList();
            return View(values);
        }

        // Ziyaretçi mesajları artık sitedeki iletişim formundan geliyor; admin'in mesaj "oluşturması"
        // ve gelen bir mesajın içeriğini "düzenlemesi" anlamlı değil
        // Eski adreslere gelen olursa listeye yönlendiriyoruz.
        [HttpGet]
        public IActionResult CreateMessage()
        {
            return RedirectToAction(nameof(MessageList));
        }

        [HttpGet]
        public IActionResult UpdateMessage(int id)
        {
            return RedirectToAction(nameof(MessageList));
        }

        // Modal içinden arka planda (fetch ile) çağrılır: mesajı okundu yapar ve detayın HTML'ini döner.
        // Durum değiştirdiği için POST; JavaScript'ten çağrıldığı için CSRF anahtarı istek başlığında gelir.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OpenMessage(int id)
        {
            var value = _context.Messages.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            if (!value.IsRead)
            {
                value.IsRead = true;
                _context.SaveChanges();
            }

            // Bütün sayfa değil, sadece modalın içine yerleşecek parça.
            return PartialView("_MessageDetailPartial", value);
        }

        [HttpPost]
        public IActionResult ToggleReadStatus(int id, string? search, string status = "all")
        {
            var value = _context.Messages.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            value.IsRead = !value.IsRead;
            _context.SaveChanges();
            TempData["SuccessMessage"] = value.IsRead ? "Mesaj okundu olarak işaretlendi." : "Mesaj okunmadı olarak işaretlendi.";

            // İşlemden sonra kullanıcı, bıraktığı arama/filtre ekranına geri döner.
            return RedirectToAction(nameof(MessageList), new { search, status });
        }

        [HttpPost]
        public IActionResult DeleteMessage(int id, string? search, string status = "all")
        {
            var value = _context.Messages.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(value);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Mesaj silindi.";
            return RedirectToAction(nameof(MessageList), new { search, status });
        }
    }
}
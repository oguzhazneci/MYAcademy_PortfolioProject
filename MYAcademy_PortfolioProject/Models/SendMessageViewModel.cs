using System.ComponentModel.DataAnnotations;

namespace MYAcademy_PortfolioProject.Models
{
    // Ziyaretçinin iletişim formundan gönderebileceği alanlar — ve SADECE bunlar.
    // Message entity'sini doğrudan kullanmıyoruz; IsRead, SendDate gibi alanları ziyaretçi belirleyemez.
    public class SendMessageViewModel
    {
        [Required(ErrorMessage = "Lütfen adınızı ve soyadınızı yazın.")]
        [StringLength(100, ErrorMessage = "Ad soyad en fazla 100 karakter olabilir.")]
        public string SenderName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen e-posta adresinizi yazın.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        [StringLength(150, ErrorMessage = "E-posta en fazla 150 karakter olabilir.")]
        public string SenderEmail { get; set; } = string.Empty;

        // İsteğe bağlı: boş bırakılırsa sunucu "(Konu belirtilmedi)" yazar.
        [StringLength(150, ErrorMessage = "Konu en fazla 150 karakter olabilir.")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Lütfen mesajınızı yazın.")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Mesajınız 10 ile 2000 karakter arasında olmalıdır.")]
        public string MessageDetail { get; set; } = string.Empty;
        // Honeypot (bal küpü): gerçek ziyaretçi bu alanı görmez, dolayısıyla hep boş gelir.
        // Formdaki her alanı otomatik dolduran spam botları ise buna da bir şey yazar → mesaj kaydedilmez.
        // Bilerek "Website" gibi doğal bir ad verildi: botlar "honeypot" adlı alanı atlamayı öğrenmiş olabilir.
        public string? Website { get; set; }
    }
}
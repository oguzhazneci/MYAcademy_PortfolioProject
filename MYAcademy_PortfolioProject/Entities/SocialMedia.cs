using System.ComponentModel.DataAnnotations;

namespace MYAcademy_PortfolioProject.Entities
{
    public class SocialMedia
    {
        public int SocialMediaId { get; set; }

        // Örn: "GitHub", "LinkedIn". Sitede ikonun üzerine gelince ve footer'da görünür.
        [Required(ErrorMessage = "Platform adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Platform adı en fazla 50 karakter olabilir.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bağlantı zorunludur.")]
        [StringLength(500, ErrorMessage = "Bağlantı en fazla 500 karakter olabilir.")]
        [Url(ErrorMessage = "Geçerli bir bağlantı girin (https:// ile başlamalı).")]
        public string Url { get; set; }

        // Bootstrap Icons sınıf adı (örn: bi-github). Skill'deki "IconUrl"un aksine adı içeriğini doğru anlatıyor.
        [Required(ErrorMessage = "İkon zorunludur.")]
        [StringLength(50, ErrorMessage = "İkon adı en fazla 50 karakter olabilir.")]
        public string IconClass { get; set; }

        [Range(1, 99, ErrorMessage = "Sıra 1 ile 99 arasında olmalıdır.")]
        public int Order { get; set; }

        public bool IsActive { get; set; }
    }
}
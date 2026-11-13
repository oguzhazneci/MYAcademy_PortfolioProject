using System.ComponentModel.DataAnnotations;

namespace MYAcademy_PortfolioProject.Entities
{
    public class HeroSection
    {
        public int HeroSectionId { get; set; }

        [Required(ErrorMessage = "Selamlama metni zorunludur.")]
        public string Greeting { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Görsel adresi zorunludur.")]
        public string MainImageUrl { get; set; }
    }
}
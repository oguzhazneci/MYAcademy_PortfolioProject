using System.ComponentModel.DataAnnotations;

namespace MYAcademy_PortfolioProject.Entities
{
    public class HeroTitle
    {
        public int HeroTitleId { get; set; }

        [Required(ErrorMessage = "Yazı alanı zorunludur.")]
        public string Title { get; set; }

        [Range(1, 99, ErrorMessage = "Sıra 1 ile 99 arasında olmalıdır.")]
        public int Order { get; set; }

        public bool IsActive { get; set; }
    }
}
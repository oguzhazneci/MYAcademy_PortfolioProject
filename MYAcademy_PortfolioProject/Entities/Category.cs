using System.ComponentModel.DataAnnotations;

namespace MYAcademy_PortfolioProject.Entities
{
    // PDF'te "ProjectCategory" olarak geçen tablo; projede hocayla birlikte "Category" adıyla oluşturulmuş.
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        public string CategoryName { get; set; }

        // "?" → Kategori formundan proje listesi gelmez.
        public List<Project>? Projects { get; set; }
    }
}
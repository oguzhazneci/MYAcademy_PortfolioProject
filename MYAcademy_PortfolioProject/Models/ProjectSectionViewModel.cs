using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Models
{
    // Projeler bölümünün ihtiyaç duyduğu iki farklı veriyi tek bir pakette taşır.
    public class ProjectSectionViewModel
    {
        public List<Project> Projects { get; set; } = new();

        // Filtre butonları: sadece en az bir aktif projesi olan kategoriler.
        public List<Category> Categories { get; set; } = new();
    }
}
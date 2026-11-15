using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Models
{
    // Gösterge Paneli'nin ihtiyaç duyduğu, farklı tablolardan gelen bütün veriler tek pakette.
    public class DashboardViewModel
    {
        // İstatistik kartları
        public int TotalProjectCount { get; set; }
        public int ActiveProjectCount { get; set; }
        public int SkillCount { get; set; }
        public int ExperienceCount { get; set; }
        public int EducationCount { get; set; }
        public int TotalMessageCount { get; set; }
        public int UnreadMessageCount { get; set; }

        // Okunma oranı: veritabanında tutulmaz, diğer iki sayıdan hesaplanır.
        public int ReadRate => TotalMessageCount == 0
            ? 0
            : (TotalMessageCount - UnreadMessageCount) * 100 / TotalMessageCount;

        // Kategori özeti (GroupBy sonucu)
        public List<CategoryProjectCount> CategorySummary { get; set; } = new();

        // Son 5 mesaj ve son 3 proje
        public List<Message> LastMessages { get; set; } = new();
        public List<Project> LastProjects { get; set; } = new();
    }

    // GroupBy sorgusunun her satırı: "bu kategoride şu kadar proje var"
    public class CategoryProjectCount
    {
        public string CategoryName { get; set; } = string.Empty;
        public int ProjectCount { get; set; }
    }
}
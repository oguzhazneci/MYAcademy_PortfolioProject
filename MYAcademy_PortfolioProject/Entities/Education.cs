using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYAcademy_PortfolioProject.Entities
{
    public class Education
    {
        public int EducationId { get; set; }

        // Örn: "Lisans", "Yüksek Lisans", "Sertifika"
        [Required(ErrorMessage = "Derece zorunludur.")]
        [StringLength(50, ErrorMessage = "Derece en fazla 50 karakter olabilir.")]
        public string DegreeType { get; set; }

        // Örn: "Bilgisayar Mühendisliği"
        [Required(ErrorMessage = "Bölüm / program adı zorunludur.")]
        [StringLength(150, ErrorMessage = "Bölüm / program adı en fazla 150 karakter olabilir.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Okul / kurum adı zorunludur.")]
        [StringLength(150, ErrorMessage = "Okul / kurum adı en fazla 150 karakter olabilir.")]
        public string SchoolName { get; set; }

        // İsteğe bağlı kısa açıklama.
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Başlangıç yılı zorunludur.")]
        [Range(1950, 2100, ErrorMessage = "Geçerli bir yıl girin.")]
        public int? StartYear { get; set; }

        // Boş bırakılırsa "Devam ediyor" anlamına gelir.
        [Range(1950, 2100, ErrorMessage = "Geçerli bir yıl girin.")]
        public int? EndYear { get; set; }

        public bool IsActive { get; set; }

        // Veritabanında sütunu yoktur: "2017 – 2022" veya "2024 – Devam ediyor"
        [NotMapped]
        public string YearRangeText => $"{StartYear} – {(EndYear.HasValue ? EndYear.ToString() : "Devam ediyor")}";
    }
}
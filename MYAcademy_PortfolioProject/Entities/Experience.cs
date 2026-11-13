using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace MYAcademy_PortfolioProject.Entities
{
    public class Experience
    {
        public int ExperienceId { get; set; }

        [Required(ErrorMessage = "Pozisyon zorunludur.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Şirket adı zorunludur.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        public string Description { get; set; }

        // Sitede rozet olarak görünür: "Tam Zamanlı", "Staj" vb.
        [Required(ErrorMessage = "Çalışma türü zorunludur.")]
        public string CategoryName { get; set; }

        // DateTime? + [Required]: Veritabanında NOT NULL olur, ama formdan boş gelirse
        // İngilizce dönüşüm hatası yerine bizim Türkçe mesajımız çıkar.
        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        // Boş (null) bırakılırsa "Devam ediyor" anlamına gelir.
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        // Veritabanında sütunu yoktur; tarihleri "Eylül 2022 – Şubat 2026" biçiminde hazırlar.
        // Hem admin listesinde hem sitede kullanıldığı için tek yerde yazıldı.
        [NotMapped]
        public string DateRangeText
        {
            get
            {
                var tr = new CultureInfo("tr-TR");
                string start = StartDate?.ToString("MMMM yyyy", tr) ?? "";
                string end = EndDate?.ToString("MMMM yyyy", tr) ?? "Devam ediyor";
                return $"{start} – {end}";
            }
        }
    }
}
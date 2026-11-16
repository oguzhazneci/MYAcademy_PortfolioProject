using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace MYAcademy_PortfolioProject.Entities
{
    public class About
    {
        public int AboutId { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100, ErrorMessage = "Ad soyad en fazla 100 karakter olabilir.")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Unvan zorunludur.")]
        [StringLength(100, ErrorMessage = "Unvan en fazla 100 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Konum zorunludur.")]
        [StringLength(100, ErrorMessage = "Konum en fazla 100 karakter olabilir.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Bölüm başlığı zorunludur.")]
        [StringLength(150, ErrorMessage = "Bölüm başlığı en fazla 150 karakter olabilir.")]
        public string Heading { get; set; }

        [Required(ErrorMessage = "Tanıtım metni zorunludur.")]
        [StringLength(2000, ErrorMessage = "Tanıtım metni en fazla 2000 karakter olabilir.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Fotoğraf adresi zorunludur.")]
        [StringLength(500, ErrorMessage = "Fotoğraf adresi en fazla 500 karakter olabilir.")]
        public string ImageUrl { get; set; }

        [StringLength(500, ErrorMessage = "CV adresi en fazla 500 karakter olabilir.")]
        public string? CvUrl { get; set; }

        // Öne çıkan bilgiler (ilk ikisi Hero'daki yüzen rozetlerde de kullanılır)
        [Required(ErrorMessage = "1. istatistik değeri zorunludur.")]
        [StringLength(20, ErrorMessage = "İstatistik değeri en fazla 20 karakter olabilir.")]
        public string StatValue1 { get; set; }

        [Required(ErrorMessage = "1. istatistik etiketi zorunludur.")]
        [StringLength(50, ErrorMessage = "İstatistik etiketi en fazla 50 karakter olabilir.")]
        public string StatLabel1 { get; set; }

        [Required(ErrorMessage = "2. istatistik değeri zorunludur.")]
        [StringLength(20, ErrorMessage = "İstatistik değeri en fazla 20 karakter olabilir.")]
        public string StatValue2 { get; set; }

        [Required(ErrorMessage = "2. istatistik etiketi zorunludur.")]
        [StringLength(50, ErrorMessage = "İstatistik etiketi en fazla 50 karakter olabilir.")]
        public string StatLabel2 { get; set; }

        [Required(ErrorMessage = "3. istatistik değeri zorunludur.")]
        [StringLength(20, ErrorMessage = "İstatistik değeri en fazla 20 karakter olabilir.")]
        public string StatValue3 { get; set; }

        [Required(ErrorMessage = "3. istatistik etiketi zorunludur.")]
        [StringLength(50, ErrorMessage = "İstatistik etiketi en fazla 50 karakter olabilir.")]
        public string StatLabel3 { get; set; }

        // Veritabanında sütunu yok; ad soyaddan hesaplanır. "Oğuz Hazneci" → "OH" (avatar dairelerinde kullanılır)
        [NotMapped]
        public string Initials
        {
            get
            {
                var words = (FullName ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (words.Length == 0)
                {
                    return "?";
                }

                var first = words[0][0].ToString();
                var last = words.Length > 1 ? words[^1][0].ToString() : string.Empty;
                return (first + last).ToUpper(new CultureInfo("tr-TR"));
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MYAcademy_PortfolioProject.Entities
{
    public class Contact
    {
        public int ContactId { get; set; }

        // Sitedeki İletişim bölümünün başlığı ("Harika Bir Fikrin mi Var? ...")
        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Konum zorunludur.")]
        public string Location { get; set; }

        // İsteğe bağlı: boş bırakılırsa sitede telefon gösterilmez.
        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
        public string? PhoneNumber { get; set; }
    }
}
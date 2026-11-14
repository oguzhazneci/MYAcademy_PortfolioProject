using System.ComponentModel.DataAnnotations;

namespace MYAcademy_PortfolioProject.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Proje adı zorunludur.")]
        public string ProjectTitle { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        public string Description { get; set; }

        // Görsel dosyanın kendisi değil, wwwroot içindeki yolu tutulur (örn: /uploads/projects/abc.png).
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        // Virgülle ayrılmış liste: "ASP.NET Core, EF Core, SQL Server". UI'da rozetlere bölünür.
        [Required(ErrorMessage = "En az bir teknoloji girin.")]
        [StringLength(300, ErrorMessage = "Teknolojiler en fazla 300 karakter olabilir.")]
        public string Technologies { get; set; }

        // İsteğe bağlı: GitHub, mağaza veya demo bağlantısı. Boşsa "Projeyi İncele" butonu gösterilmez.
        [StringLength(500, ErrorMessage = "Bağlantı en fazla 500 karakter olabilir.")]
        [Url(ErrorMessage = "Geçerli bir bağlantı girin (https:// ile başlamalı).")]
        public string? ProjectUrl { get; set; }

        public bool IsActive { get; set; }

        // Formdan alınmaz; kayıt eklenirken sunucuda atanır. "En yeni proje" sıralaması için kullanılır.
        public DateTime CreatedDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir kategori seçin.")]
        public int CategoryId { get; set; }

        // "?" → Formdan sadece CategoryId gelir; nullable olmazsa ModelState "Category zorunlu" der.
        public Category? Category { get; set; }
    }
}
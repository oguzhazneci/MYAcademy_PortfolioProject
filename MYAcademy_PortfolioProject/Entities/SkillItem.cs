using System.ComponentModel.DataAnnotations;

namespace MYAcademy_PortfolioProject.Entities
{
    public class SkillItem
    {
        public int SkillItemId { get; set; }

        [Required(ErrorMessage = "Yetenek adı zorunludur.")]
        public string SkillItemTitle { get; set; }

        [Range(0, 100, ErrorMessage = "Seviye 0 ile 100 arasında olmalıdır.")]
        public int Level { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir kategori seçin.")]
        public int SkillId { get; set; }

        // "?" → Formdan sadece SkillId gelir, Skill nesnesi gelmez; nullable olmazsa ModelState hata verir.
        public Skill? Skill { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace MYAcademy_PortfolioProject.Entities
{
    public class Skill
    {
        public int SkillId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "İkon zorunludur.")]
        public string IconUrl { get; set; }

        // "?" → Formdan gelirken bu liste boştur; nullable olmazsa ModelState onu "zorunlu" sayar.
        public List<SkillItem>? SkillItems { get; set; }
    }
}
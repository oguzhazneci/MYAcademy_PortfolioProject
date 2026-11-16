using System.Text.Json;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Entities;

namespace MYAcademy_PortfolioProject.Data
{
    // Boş bir veritabanını (örn. projeyi GitHub'dan indiren birinin) Data/seed-data.json içeriğiyle doldurur.
    // PortfolioContext'teki UseSeeding sayesinde Update-Database sırasında otomatik çalışır.
    public static class SeedData
    {
        // JSON dosyasının yapısı: her tablo için bir liste. Anahtar adları entity property adlarıyla birebir aynı.
        private class SeedFile
        {
            public List<HeroSection> HeroSections { get; set; } = new();
            public List<HeroTitle> HeroTitles { get; set; } = new();
            public List<About> Abouts { get; set; } = new();
            public List<Skill> Skills { get; set; } = new();
            public List<SkillItem> SkillItems { get; set; } = new();
            public List<Experience> Experiences { get; set; } = new();
            public List<Education> Educations { get; set; } = new();
            public List<Category> Categories { get; set; } = new();
            public List<Project> Projects { get; set; } = new();
            public List<Contact> Contacts { get; set; } = new();
            public List<SocialMedia> SocialMedias { get; set; } = new();
        }

        public static void Seed(PortfolioContext context)
        {
            // Veritabanında içerik varsa HİÇBİR ŞEY yapma: mevcut verinin üstüne yazmaz, kopya oluşturmaz.
            bool hasContent = context.Abouts.Any()
                              || context.Skills.Any()
                              || context.Projects.Any()
                              || context.Messages.Any();
            if (hasContent)
            {
                return;
            }

            // Dosya, derleme sırasında bin klasörüne kopyalanır (bkz. .csproj).
            var path = Path.Combine(AppContext.BaseDirectory, "Data", "seed-data.json");
            if (!File.Exists(path))
            {
                return;
            }

            var data = JsonSerializer.Deserialize<SeedFile>(File.ReadAllText(path));
            if (data == null)
            {
                return;
            }

            // ID'ler: JSON'daki ID'ler kaynak veritabanına ait. Yeni veritabanında SQL Server kendi ID'lerini
            // (IDENTITY) üretsin diye hepsini 0 yapıyoruz. İlişkileri ise ID yerine nesne referansıyla kuruyoruz.

            // 1) Yetenek kategorileri ↔ yetenekler
            var skillsByOldId = data.Skills.ToDictionary(x => x.SkillId);
            foreach (var item in data.SkillItems)
            {
                item.Skill = skillsByOldId[item.SkillId];   // "bu yetenek, şu kategori nesnesine ait"
                item.SkillId = 0;
                item.SkillItemId = 0;
            }
            data.Skills.ForEach(x => x.SkillId = 0);

            // 2) Proje kategorileri ↔ projeler
            var categoriesByOldId = data.Categories.ToDictionary(x => x.CategoryId);
            foreach (var project in data.Projects)
            {
                project.Category = categoriesByOldId[project.CategoryId];
                project.CategoryId = 0;
                project.ProjectId = 0;
            }
            data.Categories.ForEach(x => x.CategoryId = 0);

            // 3) İlişkisi olmayan tablolar
            data.HeroSections.ForEach(x => x.HeroSectionId = 0);
            data.HeroTitles.ForEach(x => x.HeroTitleId = 0);
            data.Abouts.ForEach(x => x.AboutId = 0);
            data.Experiences.ForEach(x => x.ExperienceId = 0);
            data.Educations.ForEach(x => x.EducationId = 0);
            data.Contacts.ForEach(x => x.ContactId = 0);
            data.SocialMedias.ForEach(x => x.SocialMediaId = 0);

            context.HeroSections.AddRange(data.HeroSections);
            context.HeroTitles.AddRange(data.HeroTitles);
            context.Abouts.AddRange(data.Abouts);
            context.Skills.AddRange(data.Skills);
            context.SkillItems.AddRange(data.SkillItems);
            context.Experiences.AddRange(data.Experiences);
            context.Educations.AddRange(data.Educations);
            context.Categories.AddRange(data.Categories);
            context.Projects.AddRange(data.Projects);
            context.Contacts.AddRange(data.Contacts);
            context.SocialMedias.AddRange(data.SocialMedias);

            // Gelen Kutusu boş görünmesin diye UYDURMA demo mesajlar (gerçek mesajlar asla repoya girmez).
            context.Messages.AddRange(
                new Message
                {
                    SenderName = "Ayşe Demir",
                    SenderEmail = "ayse.demir@example.com",
                    Subject = "Proje iş birliği",
                    MessageDetail = "Merhaba, portfolyonuzdaki AR projelerini inceledim. Eğitim alanında planladığımız bir uygulama için sizinle görüşmek isterim.",
                    SendDate = DateTime.Now.AddDays(-1),
                    IsRead = false
                },
                new Message
                {
                    SenderName = "Mehmet Kaya",
                    SenderEmail = "mehmet.kaya@example.com",
                    Subject = "Web uygulaması teklifi",
                    MessageDetail = "İyi günler, ASP.NET Core ile geliştirilecek bir yönetim paneli projemiz var. Uygunluk durumunuzu öğrenebilir miyim?",
                    SendDate = DateTime.Now.AddDays(-3),
                    IsRead = false
                },
                new Message
                {
                    SenderName = "Zeynep Arslan",
                    SenderEmail = "zeynep.arslan@example.com",
                    Subject = "Teşekkürler",
                    MessageDetail = "Pırıl Kampüs uygulamasını çocuğumla birlikte kullanıyoruz, mini oyunlar çok başarılı. Emeğinize sağlık!",
                    SendDate = DateTime.Now.AddDays(-6),
                    IsRead = true
                });

            // Hepsi tek seferde: ya tamamı eklenir ya hiçbiri (yarım kalmış bir veritabanı olmaz).
            context.SaveChanges();
        }
    }
}
namespace MYAcademy_PortfolioProject.Services
{
    // Görsel yükleme işlerini tek bir yerde toplar: doğrulama, güvenli adla kaydetme, silme.
    // Projelerde kullanıyoruz; ileride Hakkımda/Hero fotoğrafları için de aynı sınıf kullanılabilir.
    public class ImageUploadService
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSize = 2 * 1024 * 1024; // 2 MB

        private readonly IWebHostEnvironment _environment;

        public ImageUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // Dosya uygunsa null, değilse Türkçe hata mesajı döner.
        public string? Validate(IFormFile file)
        {
            if (file.Length == 0)
            {
                return "Seçilen dosya boş.";
            }

            if (file.Length > MaxFileSize)
            {
                return "Görsel en fazla 2 MB olabilir.";
            }

            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                return "Sadece JPG, PNG veya WEBP formatında görsel yükleyebilirsiniz.";
            }

            if (!file.ContentType.StartsWith("image/"))
            {
                return "Seçilen dosya bir görsel değil.";
            }

            return null;
        }

        // Dosyayı wwwroot/uploads/{folderName}/ altına benzersiz bir adla kaydeder,
        // veritabanına yazılacak yolu döner (örn: /uploads/projects/3f2a....png).
        public string Save(IFormFile file, string folderName)
        {
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            string fileName = $"{Guid.NewGuid()}{extension}";

            string folderPath = Path.Combine(_environment.WebRootPath, "uploads", folderName);
            Directory.CreateDirectory(folderPath); // klasör yoksa oluşturur, varsa bir şey yapmaz

            string fullPath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"/uploads/{folderName}/{fileName}";
        }

        // Sadece /uploads/ altındaki dosyaları siler; elle konmuş görsellere (örn. /images/) dokunmaz.
        public void Delete(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath) || !relativePath.StartsWith("/uploads/"))
            {
                return;
            }

            string fullPath = Path.Combine(_environment.WebRootPath,
                relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
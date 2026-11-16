using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.RateLimiting;
using MYAcademy_PortfolioProject.Context;
using MYAcademy_PortfolioProject.Models;
using MYAcademy_PortfolioProject.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// AutoValidateAntiforgeryToken: GET dışındaki (POST vb.) BÜTÜN isteklerde anti-forgery token'ı doğrular.
// Böylece [ValidateAntiForgeryToken] yazmayı unuttuğumuz bir action CSRF'e açık kalmaz.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddDbContext<PortfolioContext>();

// Kendi yazdığımız servisi DI sistemine tanıtıyoruz; controller constructor'ında istenince otomatik verilir.
builder.Services.AddScoped<ImageUploadService>();

// Admin kullanıcı adı/şifresi: appsettings.Development.json (demo) veya User Secrets (gerçek) içindeki
// "AdminCredentials" bölümü bu sınıfa bağlanır; controller'da IOptions<AdminCredentials> ile istenir.
builder.Services.Configure<AdminCredentials>(builder.Configuration.GetSection("AdminCredentials"));

// Çerez (cookie) tabanlı kimlik doğrulama: giriş yapınca tarayıcıya şifreli bir oturum çerezi bırakılır.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Auth/Login";     // Giriş yapmamış biri korumalı sayfaya gelirse buraya
        options.LogoutPath = "/Admin/Auth/Logout";
        options.Cookie.Name = "PortfolioAdmin";
        options.Cookie.HttpOnly = true;               // JavaScript çerezi okuyamaz (XSS'e karşı)
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;             // Kullanıldıkça süre yenilenir
    });

// Hız sınırlama (rate limiting): aynı IP adresinden iletişim formuna 10 dakikada en fazla 3 gönderim.
// Honeypot'u atlatan (tuzak alanı boş bırakan) inatçı botlara karşı ikinci savunma katmanı.
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("contact-form", httpContext =>
        // Partition: her IP adresi için ayrı bir sayaç ("kova") tutulur.
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "bilinmiyor",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,                     // Pencere başına izin verilen istek sayısı
                Window = TimeSpan.FromMinutes(10),   // Pencere süresi
                QueueLimit = 0                       // Sınır aşılınca bekletme, hemen reddet
            }));

    // Sınır aşıldığında: boş bir "429" sayfası yerine iletişim bölümüne anlaşılır bir mesajla dön.
    options.OnRejected = (context, cancellationToken) =>
    {
        var httpContext = context.HttpContext;

        var logger = httpContext.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogWarning("İletişim formu hız sınırına takıldı. IP: {Ip}", httpContext.Connection.RemoteIpAddress);

        // Controller dışında olduğumuz için TempData'ya fabrikası üzerinden erişiyoruz.
        var tempData = httpContext.RequestServices
            .GetRequiredService<ITempDataDictionaryFactory>()
            .GetTempData(httpContext);
        tempData["ContactError"] = "Kısa süre içinde çok fazla mesaj gönderdiniz. Lütfen birkaç dakika sonra tekrar deneyin.";
        tempData.Save();

        httpContext.Response.Redirect("/#contact");
        return ValueTask.CompletedTask;
    };
});

var app = builder.Build();


// Geliştirme ortamında uygulama açılırken bekleyen migration'ları uygula (veritabanı yoksa oluşturur).
// Migrate() UseSeeding'i de tetikler: projeyi yeni indiren biri F5'e bastığında dolu bir siteyle karşılaşır.
// Canlı ortamda (Production) bilerek yapılmaz: şema değişikliği kontrollü, ayrı bir adım olmalı.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<PortfolioContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// MapStaticAssets sadece derleme anında wwwroot'ta olan dosyaları sunar.
// Çalışma sırasında yüklenen görseller (uploads) için UseStaticFiles gerekir.
app.UseStaticFiles();

app.UseRouting();

// Sıra önemli: önce "kimsin?" (Authentication), sonra "buna yetkin var mı?" (Authorization)
app.UseAuthentication();
app.UseAuthorization();

// Hız sınırlama: UseRouting'den SONRA olmalı (hangi action'a gidildiğini ve [EnableRateLimiting] işaretini bilmesi için).
app.UseRateLimiter();

app.MapStaticAssets();

// Admin alanı: bu rotayla eşleşen BÜTÜN controller'lar giriş ister.
// Tek bir yerde tanımlandığı için yeni eklenen bir admin controller'ı korumasız kalamaz.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}")
    .RequireAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
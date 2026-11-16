namespace MYAcademy_PortfolioProject.Models
{
    // appsettings / User Secrets içindeki "AdminCredentials" bölümünün C# karşılığı (Options pattern).
    public class AdminCredentials
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
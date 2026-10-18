using Microsoft.Extensions.Configuration;

namespace Lojistik;

/// <summary>
/// Uygulama ayarlarını şu sırayla okur (sonraki öncekini ezer):
/// appsettings.json → appsettings.Local.json (git'e girmez) → LOJISTIK_ önekli ortam değişkenleri.
/// Örnek: LOJISTIK_ConnectionStrings__Default="Server=.;Database=Lojistik;..."
/// </summary>
internal static class AppConfig
{
    public static string ConnectionString { get; private set; } = string.Empty;

    public static void Load()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Local.json", optional: true)
            .AddEnvironmentVariables(prefix: "LOJISTIK_")
            .Build();

        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ConnectionStrings:Default ayarı bulunamadı.");
        }

        ConnectionString = connectionString;
    }
}

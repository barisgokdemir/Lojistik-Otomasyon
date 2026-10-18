using Microsoft.Data.SqlClient;

namespace Lojistik.Ui;

/// <summary>
/// Veritabanı işlemlerini çalıştırır ve hataları kullanıcıya anlaşılır mesajlarla gösterir.
/// Eski sürümde hiçbir işlem try/catch içinde değildi; bir hata uygulamayı çökertip bağlantıyı açık bırakıyordu.
/// </summary>
internal static class Islem
{
    public static bool Calistir(Action islem)
    {
        try
        {
            Cursor.Current = Cursors.WaitCursor;
            islem();
            return true;
        }
        catch (SqlException ex)
        {
            Mesaj.Hata(SqlHataMesaji(ex));
            return false;
        }
        catch (Exception ex)
        {
            Mesaj.Hata($"Beklenmeyen bir hata oluştu:\n{ex.Message}");
            return false;
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    private static string SqlHataMesaji(SqlException ex) => ex.Number switch
    {
        // Yabancı anahtar ihlali (silme veya ekleme)
        547 when ex.Message.Contains("DELETE", StringComparison.OrdinalIgnoreCase) =>
            "Bu kayıt başka kayıtlarda kullanıldığı için silinemez.",
        547 => "Girilen değerler kurallara uymuyor veya ilişkili kayıt bulunamadı.",

        // Benzersizlik ihlali
        2601 or 2627 => "Bu kayıt zaten mevcut.",

        // Veritabanı yok
        4060 => "Lojistik veritabanı bulunamadı. Önce database klasöründeki scriptleri çalıştırın.",

        // Giriş başarısız
        18456 => "Veritabanına giriş yapılamadı. Bağlantı ayarlarındaki kullanıcı bilgilerini kontrol edin.",

        // Sunucuya ulaşılamıyor
        -2 or -1 or 2 or 53 or 10061 =>
            "SQL Server'a bağlanılamadı. appsettings.json içindeki bağlantı cümlesini kontrol edin.",

        _ => $"Veritabanı hatası ({ex.Number}):\n{ex.Message}"
    };
}

using System.Data;

namespace Lojistik.Data.Repositories;

public sealed class PersonelRepository(Db db)
{
    public DataTable Listele(string? ara = null) => db.Query("""
        SELECT p.PersonelID AS [ID], p.AdSoyad AS [Ad Soyad], p.Telefon, b.BolumAdi AS [Bölüm]
        FROM dbo.Personel AS p
        INNER JOIN dbo.Bolumler AS b ON b.BolumID = p.BolumID
        WHERE (@ara IS NULL OR p.AdSoyad LIKE @ara)
        ORDER BY p.AdSoyad;
        """,
        Db.Like("@ara", ara));

    public void Ekle(string adSoyad, string? telefon, int bolumId) => db.Execute("""
        INSERT INTO dbo.Personel (AdSoyad, Telefon, BolumID) VALUES (@adSoyad, @telefon, @bolumId);
        """,
        Db.Text("@adSoyad", adSoyad, 150),
        Db.Text("@telefon", telefon, 20),
        Db.Int("@bolumId", bolumId));

    public int Sil(int id) => db.Execute("""
        DELETE FROM dbo.Personel WHERE PersonelID = @id;
        """,
        Db.Int("@id", id));
}

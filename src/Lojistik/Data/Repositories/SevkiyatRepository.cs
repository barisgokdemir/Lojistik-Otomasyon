using System.Data;

namespace Lojistik.Data.Repositories;

public sealed class SevkiyatRepository(Db db)
{
    public DataTable Listele(string? ara = null) => db.Query("""
        SELECT s.SevkiyatID AS [ID], s.SevkiyatAdi AS [Sevkiyat], s.Ulke AS [Ülke],
               u.UrunAdi AS [Ürün], t.FirmaAdi AS [Taşıyıcı], t.TasimaSekli AS [Taşıma Şekli],
               s.OlusturmaTarihi AS [Oluşturma Tarihi]
        FROM dbo.Sevkiyatlar AS s
        INNER JOIN dbo.Urunler AS u ON u.UrunID = s.UrunID
        INNER JOIN dbo.TasiyiciFirmalar AS t ON t.TasiyiciFirmaID = s.TasiyiciFirmaID
        WHERE (@ara IS NULL OR s.SevkiyatAdi LIKE @ara OR s.Ulke LIKE @ara)
        ORDER BY s.OlusturmaTarihi DESC;
        """,
        Db.Like("@ara", ara));

    public void Ekle(string sevkiyatAdi, string ulke, int urunId, int tasiyiciFirmaId) => db.Execute("""
        INSERT INTO dbo.Sevkiyatlar (SevkiyatAdi, Ulke, UrunID, TasiyiciFirmaID)
        VALUES (@sevkiyatAdi, @ulke, @urunId, @tasiyiciFirmaId);
        """,
        Db.Text("@sevkiyatAdi", sevkiyatAdi, 150),
        Db.Text("@ulke", ulke, 100),
        Db.Int("@urunId", urunId),
        Db.Int("@tasiyiciFirmaId", tasiyiciFirmaId));

    public int Sil(int id) => db.Execute("""
        DELETE FROM dbo.Sevkiyatlar WHERE SevkiyatID = @id;
        """,
        Db.Int("@id", id));
}

using System.Data;

namespace Lojistik.Data.Repositories;

public sealed class FirmaTipiRepository(Db db)
{
    public DataTable Listele(string? ara = null) => db.Query("""
        SELECT FirmaTipiID AS [ID], FirmaTipiAdi AS [Firma Tipi]
        FROM dbo.FirmaTipleri
        WHERE (@ara IS NULL OR FirmaTipiAdi LIKE @ara)
        ORDER BY FirmaTipiID;
        """,
        Db.Like("@ara", ara));

    /// <summary>ComboBox için ID / Ad listesi.</summary>
    public DataTable SecimListesi() => db.Query("""
        SELECT FirmaTipiID AS ID, FirmaTipiAdi AS Ad
        FROM dbo.FirmaTipleri
        ORDER BY FirmaTipiAdi;
        """);

    public void Ekle(string ad) => db.Execute("""
        INSERT INTO dbo.FirmaTipleri (FirmaTipiAdi) VALUES (@ad);
        """,
        Db.Text("@ad", ad, 50));

    public int Sil(int id) => db.Execute("""
        DELETE FROM dbo.FirmaTipleri WHERE FirmaTipiID = @id;
        """,
        Db.Int("@id", id));
}

using System.Data;

namespace Lojistik.Data.Repositories;

public sealed class BolumRepository(Db db)
{
    public DataTable Listele(string? ara = null) => db.Query("""
        SELECT b.BolumID AS [ID], b.BolumAdi AS [Bölüm Adı], ft.FirmaTipiAdi AS [Firma Tipi]
        FROM dbo.Bolumler AS b
        LEFT JOIN dbo.FirmaTipleri AS ft ON ft.FirmaTipiID = b.FirmaTipiID
        WHERE (@ara IS NULL OR b.BolumAdi LIKE @ara)
        ORDER BY b.BolumAdi;
        """,
        Db.Like("@ara", ara));

    public DataTable SecimListesi() => db.Query("""
        SELECT BolumID AS ID, BolumAdi AS Ad
        FROM dbo.Bolumler
        ORDER BY BolumAdi;
        """);

    public void Ekle(string bolumAdi, int? firmaTipiId) => db.Execute("""
        INSERT INTO dbo.Bolumler (BolumAdi, FirmaTipiID) VALUES (@bolumAdi, @firmaTipiId);
        """,
        Db.Text("@bolumAdi", bolumAdi, 100),
        Db.Int("@firmaTipiId", firmaTipiId));

    public int Sil(int id) => db.Execute("""
        DELETE FROM dbo.Bolumler WHERE BolumID = @id;
        """,
        Db.Int("@id", id));
}

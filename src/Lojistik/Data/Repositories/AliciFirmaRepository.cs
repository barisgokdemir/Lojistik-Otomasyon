using System.Data;

namespace Lojistik.Data.Repositories;

public sealed class AliciFirmaRepository(Db db)
{
    public DataTable Listele(string? ara = null) => db.Query("""
        SELECT AliciFirmaID AS [ID], FirmaAdi AS [Firma Adı], Adres, Telefon
        FROM dbo.AliciFirmalar
        WHERE (@ara IS NULL OR FirmaAdi LIKE @ara)
        ORDER BY FirmaAdi;
        """,
        Db.Like("@ara", ara));

    public void Ekle(string firmaAdi, string? adres, string? telefon) => db.Execute("""
        INSERT INTO dbo.AliciFirmalar (FirmaAdi, Adres, Telefon, FirmaTipiID)
        VALUES (@firmaAdi, @adres, @telefon, @firmaTipiId);
        """,
        Db.Text("@firmaAdi", firmaAdi, 150),
        Db.Text("@adres", adres, 300),
        Db.Text("@telefon", telefon, 20),
        Db.Int("@firmaTipiId", (int)FirmaTipi.Alici));

    public int Sil(int id) => db.Execute("""
        DELETE FROM dbo.AliciFirmalar WHERE AliciFirmaID = @id;
        """,
        Db.Int("@id", id));
}

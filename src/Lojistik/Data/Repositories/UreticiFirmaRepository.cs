using System.Data;

namespace Lojistik.Data.Repositories;

public sealed class UreticiFirmaRepository(Db db)
{
    public DataTable Listele(string? ara = null) => db.Query("""
        SELECT UreticiFirmaID AS [ID], FirmaAdi AS [Firma Adı], Adres, Telefon
        FROM dbo.UreticiFirmalar
        WHERE (@ara IS NULL OR FirmaAdi LIKE @ara)
        ORDER BY FirmaAdi;
        """,
        Db.Like("@ara", ara));

    public void Ekle(string firmaAdi, string? adres, string? telefon) => db.Execute("""
        INSERT INTO dbo.UreticiFirmalar (FirmaAdi, Adres, Telefon, FirmaTipiID)
        VALUES (@firmaAdi, @adres, @telefon, @firmaTipiId);
        """,
        Db.Text("@firmaAdi", firmaAdi, 150),
        Db.Text("@adres", adres, 300),
        Db.Text("@telefon", telefon, 20),
        Db.Int("@firmaTipiId", (int)FirmaTipi.Uretici));

    public int Sil(int id) => db.Execute("""
        DELETE FROM dbo.UreticiFirmalar WHERE UreticiFirmaID = @id;
        """,
        Db.Int("@id", id));
}

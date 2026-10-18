using System.Data;

namespace Lojistik.Data.Repositories;

public sealed class TasiyiciFirmaRepository(Db db)
{
    public DataTable Listele(string? ara = null) => db.Query("""
        SELECT TasiyiciFirmaID AS [ID], FirmaAdi AS [Firma Adı], TasimaSekli AS [Taşıma Şekli],
               Adres, Telefon
        FROM dbo.TasiyiciFirmalar
        WHERE (@ara IS NULL OR FirmaAdi LIKE @ara)
        ORDER BY FirmaAdi;
        """,
        Db.Like("@ara", ara));

    public DataTable SecimListesi() => db.Query("""
        SELECT TasiyiciFirmaID AS ID, FirmaAdi + N' (' + TasimaSekli + N')' AS Ad
        FROM dbo.TasiyiciFirmalar
        ORDER BY FirmaAdi;
        """);

    public void Ekle(string firmaAdi, string tasimaSekli, string? adres, string? telefon) => db.Execute("""
        INSERT INTO dbo.TasiyiciFirmalar (FirmaAdi, TasimaSekli, Adres, Telefon, FirmaTipiID)
        VALUES (@firmaAdi, @tasimaSekli, @adres, @telefon, @firmaTipiId);
        """,
        Db.Text("@firmaAdi", firmaAdi, 150),
        Db.Text("@tasimaSekli", tasimaSekli, 20),
        Db.Text("@adres", adres, 300),
        Db.Text("@telefon", telefon, 20),
        Db.Int("@firmaTipiId", (int)FirmaTipi.Tasiyici));

    public int Sil(int id) => db.Execute("""
        DELETE FROM dbo.TasiyiciFirmalar WHERE TasiyiciFirmaID = @id;
        """,
        Db.Int("@id", id));
}

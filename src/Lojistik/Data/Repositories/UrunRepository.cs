using System.Data;

namespace Lojistik.Data.Repositories;

public sealed class UrunRepository(Db db)
{
    public DataTable Listele(string? ara = null) => db.Query("""
        SELECT UrunID AS [ID], UrunAdi AS [Ürün Adı], UrunTipi AS [Ürün Tipi], StokMiktari AS [Stok]
        FROM dbo.Urunler
        WHERE (@ara IS NULL OR UrunAdi LIKE @ara)
        ORDER BY UrunAdi;
        """,
        Db.Like("@ara", ara));

    public DataTable SecimListesi() => db.Query("""
        SELECT UrunID AS ID, UrunAdi AS Ad
        FROM dbo.Urunler
        ORDER BY UrunAdi;
        """);

    public void Ekle(string urunAdi, string? urunTipi, int stokMiktari) => db.Execute("""
        INSERT INTO dbo.Urunler (UrunAdi, UrunTipi, StokMiktari)
        VALUES (@urunAdi, @urunTipi, @stok);
        """,
        Db.Text("@urunAdi", urunAdi, 150),
        Db.Text("@urunTipi", urunTipi, 100),
        Db.Int("@stok", stokMiktari));

    public int Sil(int id) => db.Execute("""
        DELETE FROM dbo.Urunler WHERE UrunID = @id;
        """,
        Db.Int("@id", id));
}

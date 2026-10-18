using Lojistik.Data;
using Lojistik.Data.Repositories;
using Lojistik.Ui;

namespace Lojistik.Forms;

public partial class UrunlerForm : Form
{
    private readonly UrunRepository _repo = new(Db.Default);

    public UrunlerForm()
    {
        InitializeComponent();
        FormYardimci.GridHazirla(kayitlarGrid, silText);
        Load += (_, _) => Listele();
    }

    private void Listele(string? ara = null) =>
        Islem.Calistir(() => kayitlarGrid.DataSource = _repo.Listele(ara));

    private void btnEkle_Click(object sender, EventArgs e)
    {
        if (!FormYardimci.Zorunlu(urunAdText, "Ürün adı")) return;
        if (!FormYardimci.SayiAl(stokText, "Stok miktarı", out var stok)) return;

        Islem.Calistir(() =>
        {
            _repo.Ekle(urunAdText.Text, urunTipiText.Text, stok);
            Mesaj.Bilgi("Ürün eklendi.");
            FormYardimci.Temizle(urunAdText, urunTipiText, stokText);
            Listele();
        });
    }

    private void btnListele_Click(object sender, EventArgs e)
    {
        aramaText.Clear();
        Listele();
    }

    private void btnAra_Click(object sender, EventArgs e) => Listele(aramaText.Text);

    private void btnSil_Click(object sender, EventArgs e)
    {
        if (!FormYardimci.IdAl(silText, "Ürün ID", out var id)) return;
        if (!Mesaj.Onay($"{id} numaralı ürün silinsin mi?")) return;

        Islem.Calistir(() =>
        {
            if (_repo.Sil(id) == 0)
            {
                Mesaj.Uyari("Bu ID ile kayıt bulunamadı.");
                return;
            }

            Mesaj.Bilgi("Kayıt silindi.");
            silText.Clear();
            Listele();
        });
    }

    private void btnKapat_Click(object sender, EventArgs e) => Close();
}

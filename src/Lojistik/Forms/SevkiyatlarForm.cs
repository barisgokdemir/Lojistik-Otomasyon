using Lojistik.Data;
using Lojistik.Data.Repositories;
using Lojistik.Ui;

namespace Lojistik.Forms;

public partial class SevkiyatlarForm : Form
{
    private readonly SevkiyatRepository _repo = new(Db.Default);
    private readonly UrunRepository _urunler = new(Db.Default);
    private readonly TasiyiciFirmaRepository _tasiyicilar = new(Db.Default);

    public SevkiyatlarForm()
    {
        InitializeComponent();
        FormYardimci.GridHazirla(kayitlarGrid, silText);
        Load += (_, _) =>
        {
            Islem.Calistir(() =>
            {
                FormYardimci.ComboDoldur(urunCombo, _urunler.SecimListesi());
                FormYardimci.ComboDoldur(tasiyiciCombo, _tasiyicilar.SecimListesi());
            });
            Listele();
        };
    }

    private void Listele(string? ara = null) =>
        Islem.Calistir(() => kayitlarGrid.DataSource = _repo.Listele(ara));

    private void btnEkle_Click(object sender, EventArgs e)
    {
        if (!FormYardimci.Zorunlu(adText, "Sevkiyat adı")) return;
        if (!FormYardimci.Zorunlu(ulkeText, "Ülke")) return;
        if (!FormYardimci.SecimZorunlu(urunCombo, "ürün", out var urunId)) return;
        if (!FormYardimci.SecimZorunlu(tasiyiciCombo, "taşıyıcı firma", out var tasiyiciId)) return;

        Islem.Calistir(() =>
        {
            _repo.Ekle(adText.Text, ulkeText.Text, urunId, tasiyiciId);
            Mesaj.Bilgi("Sevkiyat eklendi.");
            FormYardimci.Temizle(adText, ulkeText, urunCombo, tasiyiciCombo);
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
        if (!FormYardimci.IdAl(silText, "Sevkiyat ID", out var id)) return;
        if (!Mesaj.Onay($"{id} numaralı sevkiyat silinsin mi?")) return;

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

using Lojistik.Data;
using Lojistik.Data.Repositories;
using Lojistik.Ui;

namespace Lojistik.Forms;

public partial class PersonelForm : Form
{
    private readonly PersonelRepository _repo = new(Db.Default);
    private readonly BolumRepository _bolumler = new(Db.Default);

    public PersonelForm()
    {
        InitializeComponent();
        FormYardimci.GridHazirla(kayitlarGrid, silText);
        Load += (_, _) =>
        {
            Islem.Calistir(() => FormYardimci.ComboDoldur(bolumCombo, _bolumler.SecimListesi()));
            Listele();
        };
    }

    private void Listele(string? ara = null) =>
        Islem.Calistir(() => kayitlarGrid.DataSource = _repo.Listele(ara));

    private void btnEkle_Click(object sender, EventArgs e)
    {
        if (!FormYardimci.Zorunlu(adiSoyAdiText, "Ad soyad")) return;
        if (!FormYardimci.SecimZorunlu(bolumCombo, "bölüm", out var bolumId)) return;

        Islem.Calistir(() =>
        {
            _repo.Ekle(adiSoyAdiText.Text, telText.Text, bolumId);
            Mesaj.Bilgi("Personel eklendi.");
            FormYardimci.Temizle(adiSoyAdiText, telText, bolumCombo);
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
        if (!FormYardimci.IdAl(silText, "Personel ID", out var id)) return;
        if (!Mesaj.Onay($"{id} numaralı personel silinsin mi?")) return;

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

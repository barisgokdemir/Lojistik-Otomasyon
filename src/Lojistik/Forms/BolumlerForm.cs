using Lojistik.Data;
using Lojistik.Data.Repositories;
using Lojistik.Ui;

namespace Lojistik.Forms;

public partial class BolumlerForm : Form
{
    private readonly BolumRepository _repo = new(Db.Default);
    private readonly FirmaTipiRepository _firmaTipleri = new(Db.Default);

    public BolumlerForm()
    {
        InitializeComponent();
        FormYardimci.GridHazirla(kayitlarGrid, silText);
        Load += (_, _) =>
        {
            Islem.Calistir(() => FormYardimci.ComboDoldur(firmaTipiCombo, _firmaTipleri.SecimListesi()));
            Listele();
        };
    }

    private void Listele(string? ara = null) =>
        Islem.Calistir(() => kayitlarGrid.DataSource = _repo.Listele(ara));

    private void btnEkle_Click(object sender, EventArgs e)
    {
        if (!FormYardimci.Zorunlu(adText, "Bölüm adı")) return;

        // Firma tipi isteğe bağlıdır
        var firmaTipiId = FormYardimci.SeciliId(firmaTipiCombo);

        Islem.Calistir(() =>
        {
            _repo.Ekle(adText.Text, firmaTipiId);
            Mesaj.Bilgi("Bölüm eklendi.");
            FormYardimci.Temizle(adText, firmaTipiCombo);
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
        if (!FormYardimci.IdAl(silText, "Bölüm ID", out var id)) return;
        if (!Mesaj.Onay($"{id} numaralı bölüm silinsin mi?")) return;

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

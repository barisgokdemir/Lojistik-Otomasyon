using Lojistik.Data;
using Lojistik.Data.Repositories;
using Lojistik.Ui;

namespace Lojistik.Forms;

public partial class FirmaTipleriForm : Form
{
    private readonly FirmaTipiRepository _repo = new(Db.Default);

    public FirmaTipleriForm()
    {
        InitializeComponent();
        FormYardimci.GridHazirla(kayitlarGrid, silText);
        Load += (_, _) => Listele();
    }

    private void Listele(string? ara = null) =>
        Islem.Calistir(() => kayitlarGrid.DataSource = _repo.Listele(ara));

    private void btnEkle_Click(object sender, EventArgs e)
    {
        if (!FormYardimci.Zorunlu(firmaTipiText, "Firma tipi")) return;

        Islem.Calistir(() =>
        {
            _repo.Ekle(firmaTipiText.Text);
            Mesaj.Bilgi("Firma tipi eklendi.");
            FormYardimci.Temizle(firmaTipiText);
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
        if (!FormYardimci.IdAl(silText, "Firma tipi ID", out var id)) return;
        if (!Mesaj.Onay($"{id} numaralı firma tipi silinsin mi?")) return;

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

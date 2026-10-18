using Lojistik.Data;
using Lojistik.Data.Repositories;
using Lojistik.Ui;

namespace Lojistik.Forms;

public partial class TasiyiciFirmalarForm : Form
{
    private readonly TasiyiciFirmaRepository _repo = new(Db.Default);

    public TasiyiciFirmalarForm()
    {
        InitializeComponent();
        comboTasimaSekli.DropDownStyle = ComboBoxStyle.DropDownList;
        FormYardimci.GridHazirla(kayitlarGrid, silText);
        Load += (_, _) => Listele();
    }

    private void Listele(string? ara = null) =>
        Islem.Calistir(() => kayitlarGrid.DataSource = _repo.Listele(ara));

    private void btnEkle_Click(object sender, EventArgs e)
    {
        if (!FormYardimci.Zorunlu(firmaAdText, "Firma adı")) return;
        if (comboTasimaSekli.SelectedItem is not string tasimaSekli)
        {
            Mesaj.Uyari("Lütfen taşıma şeklini seçin.");
            comboTasimaSekli.Focus();
            return;
        }

        Islem.Calistir(() =>
        {
            _repo.Ekle(firmaAdText.Text, tasimaSekli, adresText.Text, firmaTelText.Text);
            Mesaj.Bilgi("Taşıyıcı firma eklendi.");
            FormYardimci.Temizle(firmaAdText, comboTasimaSekli, adresText, firmaTelText);
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
        if (!FormYardimci.IdAl(silText, "Taşıyıcı firma ID", out var id)) return;
        if (!Mesaj.Onay($"{id} numaralı taşıyıcı firma silinsin mi?")) return;

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

namespace Lojistik.Ui;

internal static class Mesaj
{
    private const string Baslik = "Lojistik Otomasyonu";

    public static void Bilgi(string metin) =>
        MessageBox.Show(metin, Baslik, MessageBoxButtons.OK, MessageBoxIcon.Information);

    public static void Uyari(string metin) =>
        MessageBox.Show(metin, Baslik, MessageBoxButtons.OK, MessageBoxIcon.Warning);

    public static void Hata(string metin) =>
        MessageBox.Show(metin, Baslik, MessageBoxButtons.OK, MessageBoxIcon.Error);

    public static bool Onay(string metin) =>
        MessageBox.Show(metin, Baslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) == DialogResult.Yes;
}

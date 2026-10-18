using System.Data;

namespace Lojistik.Ui;

/// <summary>CRUD formlarında ortak kullanılan doğrulama ve kontrol ayarları.</summary>
internal static class FormYardimci
{
    /// <summary>
    /// Grid'i salt okunur, tam satır seçimli hale getirir. Bir satıra tıklanınca
    /// ilk kolondaki ID değeri <paramref name="idHedefi"/> kutusuna yazılır.
    /// </summary>
    public static void GridHazirla(DataGridView grid, Control idHedefi)
    {
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.MultiSelect = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.RowHeadersVisible = false;

        grid.CellClick += (_, e) =>
        {
            if (e.RowIndex >= 0 && grid.Rows[e.RowIndex].Cells[0].Value is { } id)
            {
                idHedefi.Text = id.ToString();
            }
        };
    }

    public static void ComboDoldur(ComboBox combo, DataTable kaynak)
    {
        combo.DropDownStyle = ComboBoxStyle.DropDownList;
        combo.DisplayMember = "Ad";
        combo.ValueMember = "ID";
        combo.DataSource = kaynak;
        combo.SelectedIndex = -1;
    }

    public static int? SeciliId(ComboBox combo) =>
        combo.SelectedValue is int id ? id : null;

    public static bool Zorunlu(Control kontrol, string alanAdi)
    {
        if (!string.IsNullOrWhiteSpace(kontrol.Text))
        {
            return true;
        }

        Mesaj.Uyari($"{alanAdi} boş bırakılamaz.");
        kontrol.Focus();
        return false;
    }

    public static bool SecimZorunlu(ComboBox combo, string alanAdi, out int id)
    {
        if (SeciliId(combo) is { } secili)
        {
            id = secili;
            return true;
        }

        id = 0;
        Mesaj.Uyari($"Lütfen {alanAdi} seçin.");
        combo.Focus();
        return false;
    }

    public static bool SayiAl(Control kontrol, string alanAdi, out int deger, int enAz = 0)
    {
        if (int.TryParse(kontrol.Text.Trim(), out deger) && deger >= enAz)
        {
            return true;
        }

        Mesaj.Uyari($"{alanAdi} için {enAz} veya daha büyük bir tam sayı girin.");
        kontrol.Focus();
        return false;
    }

    public static bool IdAl(Control kontrol, string alanAdi, out int id) =>
        SayiAl(kontrol, alanAdi, out id, enAz: 1);

    public static void Temizle(params Control[] kontroller)
    {
        foreach (var kontrol in kontroller)
        {
            if (kontrol is ComboBox { DropDownStyle: ComboBoxStyle.DropDownList } combo)
            {
                combo.SelectedIndex = -1;
            }
            else
            {
                kontrol.Text = string.Empty;
            }
        }
    }
}

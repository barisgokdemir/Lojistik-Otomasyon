namespace Lojistik.Forms;

public partial class AnaForm : Form
{
    private Form? _aktifForm;

    public AnaForm()
    {
        InitializeComponent();
        panelSubMenuFirma.Visible = false;
    }

    private void AltMenuDegistir(Panel altMenu) => altMenu.Visible = !altMenu.Visible;

    private void FormAc(Form form, bool altMenuyuKapat = false)
    {
        _aktifForm?.Close();
        _aktifForm = form;

        form.TopLevel = false;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;
        panelChildForm.Controls.Add(form);
        panelChildForm.Tag = form;
        form.BringToFront();
        form.Show();

        Text = $"Lojistik Otomasyonu - {form.Text}";

        if (altMenuyuKapat)
        {
            panelSubMenuFirma.Visible = false;
        }
    }

    private void btnFirma_Click(object sender, EventArgs e) => AltMenuDegistir(panelSubMenuFirma);

    private void btnFirmalar_Click(object sender, EventArgs e) => FormAc(new FirmaTipleriForm(), altMenuyuKapat: true);

    private void btnAlici_Click(object sender, EventArgs e) => FormAc(new AliciFirmalarForm(), altMenuyuKapat: true);

    private void btnTasiyici_Click(object sender, EventArgs e) => FormAc(new TasiyiciFirmalarForm(), altMenuyuKapat: true);

    private void btnUretici_Click(object sender, EventArgs e) => FormAc(new UreticiFirmalarForm(), altMenuyuKapat: true);

    private void btnUrunler_Click(object sender, EventArgs e) => FormAc(new UrunlerForm());

    private void btnBolum_Click(object sender, EventArgs e) => FormAc(new BolumlerForm());

    private void btnPersonel_Click(object sender, EventArgs e) => FormAc(new PersonelForm());

    private void btnSevkiyat_Click(object sender, EventArgs e) => FormAc(new SevkiyatlarForm());

    private void btnExit_Click(object sender, EventArgs e) => Application.Exit();
}

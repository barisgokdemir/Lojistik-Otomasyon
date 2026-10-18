using Lojistik.Data;
using Lojistik.Forms;

namespace Lojistik;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        try
        {
            AppConfig.Load();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ayarlar yüklenemedi:\n{ex.Message}", "Lojistik",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Db.Initialize(AppConfig.ConnectionString);
        Application.Run(new AnaForm());
    }
}

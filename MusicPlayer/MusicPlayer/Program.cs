using System;
using System.Windows.Forms;

namespace MusicPlayer
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new FrmIndex());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico en la aplicación: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Importar función para DPI awareness
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDpiAware();
    }
}
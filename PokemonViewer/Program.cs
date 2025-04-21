using System;
using System.Windows.Forms;

namespace PokemonViewer
{
    static class Program
    {
        [STAThread] // This attribute ensures the application runs on a single thread, necessary for Windows Forms.
        static void Main()
        {
            // Start the application and show the main form
            Application.EnableVisualStyles(); // This applies the visual styles for modern UI elements.
            Application.SetCompatibleTextRenderingDefault(false); // This ensures compatibility for text rendering.
            Application.Run(new Form1()); // This launches your Form1, which is your main form.
        }
    }
}

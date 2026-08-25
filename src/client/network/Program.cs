using System;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Chạy LoginForm đầu tiên
            Application.Run(new LoginForm());
        }
    }
}
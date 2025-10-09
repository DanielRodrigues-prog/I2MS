using System;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Pega o nome de usuário logado no Windows
            string usuarioWindows = Environment.UserName;

            // Abre a tela principal, já passando o nome do usuário.
            Application.Run(new Form1(usuarioWindows));
        }
    }
}
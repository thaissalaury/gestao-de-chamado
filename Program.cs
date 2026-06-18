using System;
using System.Windows.Forms;
using GestaoChamados.Data;

namespace GestaoChamados
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicializa o Banco de Dados (Tabelas e Admin)
            ConexaoBanco.InicializarBanco();

            // Inicia pelo Login
            Application.Run(new FrmLogin());
        }
    }
}

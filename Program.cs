using System;
using System.Windows.Forms;
using GestaoChamados.Data;

namespace GestaoChamados
{
    /// <summary>
    /// Ponto de entrada principal da aplicação.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// O ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configurações básicas de renderização do Windows Forms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicializa o Banco de Dados SQLite (Cria arquivos, tabelas e usuário Admin inicial se necessário)
            ConexaoBanco.InicializarBanco();

            // Inicia a aplicação abrindo a tela de Login
            Application.Run(new FrmLogin());
        }
    }
}

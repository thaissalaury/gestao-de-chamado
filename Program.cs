using System.Windows.Forms;
using GestaoChamados;

namespace GestaoChamados
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FrmPrincipal());
        }
    }
}

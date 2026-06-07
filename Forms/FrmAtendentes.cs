using System.Windows.Forms;
using GestaoChamados.Models;
using GestaoChamados.Services;

namespace GestaoChamados
{
    public partial class FrmAtendentes : Form
    {
        private AtendenteService atendenteService = new AtendenteService();

        public FrmAtendentes()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Cadastro de Atendentes";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
        }
    }
}

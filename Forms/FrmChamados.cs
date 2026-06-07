using System.Windows.Forms;
using GestaoChamados.Models;
using GestaoChamados.Services;

namespace GestaoChamados
{
    public partial class FrmChamados : Form
    {
        private ChamadoService chamadoService = new ChamadoService();
        private ClienteService clienteService = new ClienteService();
        private AtendenteService atendenteService = new AtendenteService();

        public FrmChamados()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Gestao de Chamados";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
        }
    }
}

using System.Windows.Forms;
using GestaoChamados.Models;
using GestaoChamados.Services;

namespace GestaoChamados
{
    public partial class FrmClientes : Form
    {
        private ClienteService clienteService = new ClienteService();

        public FrmClientes()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Cadastro de Clientes";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
        }
    }
}

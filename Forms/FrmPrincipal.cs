using System.Windows.Forms;
using System.Drawing;

namespace GestaoChamados
{
    public class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Sistema de Gestao de Chamados";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Panel pnlMenu = new Panel();
            pnlMenu.Dock = DockStyle.Left;
            pnlMenu.Width = 200;
            pnlMenu.BackColor = Color.DarkBlue;
            pnlMenu.Padding = new Padding(10);

            Label lblTitulo = new Label { Text = "MENU", Font = new Font("Arial", 14, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 50, TextAlign = ContentAlignment.MiddleCenter };

            Button btnClientes = new Button { Text = "Clientes", Dock = DockStyle.Top, Height = 50, BackColor = Color.CornflowerBlue, ForeColor = Color.White, Font = new Font("Arial", 11, FontStyle.Bold) };
            btnClientes.Click += (s, e) => new FrmClientes().ShowDialog();

            Button btnAtendentes = new Button { Text = "Atendentes", Dock = DockStyle.Top, Height = 50, BackColor = Color.CornflowerBlue, ForeColor = Color.White, Font = new Font("Arial", 11, FontStyle.Bold) };
            btnAtendentes.Click += (s, e) => new FrmAtendentes().ShowDialog();

            Button btnChamados = new Button { Text = "Chamados", Dock = DockStyle.Top, Height = 50, BackColor = Color.CornflowerBlue, ForeColor = Color.White, Font = new Font("Arial", 11, FontStyle.Bold) };
            btnChamados.Click += (s, e) => new FrmChamados().ShowDialog();

            Button btnSair = new Button { Text = "Sair", Dock = DockStyle.Bottom, Height = 50, BackColor = Color.Red, ForeColor = Color.White, Font = new Font("Arial", 11, FontStyle.Bold) };
            btnSair.Click += (s, e) => this.Close();

            pnlMenu.Controls.AddRange(new Control[] { btnSair, btnChamados, btnAtendentes, btnClientes, lblTitulo });

            Panel pnlContent = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            Label lblWelcome = new Label { Text = "Bem-vindo ao Sistema de Gestao de Chamados", Font = new Font("Arial", 18, FontStyle.Bold), ForeColor = Color.DarkBlue, Dock = DockStyle.Top, Height = 100, TextAlign = ContentAlignment.MiddleCenter };
            pnlContent.Controls.Add(lblWelcome);

            Controls.AddRange(new Control[] { pnlContent, pnlMenu });
        }
    }
}

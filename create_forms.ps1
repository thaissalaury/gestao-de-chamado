# Script para gerar arquivos do Forms
$frmprincipal = @'
using System.Windows.Forms;
namespace GestaoChamados {
	public class FrmPrincipal : Form {
		public FrmPrincipal() { InitializeComponent(); }
		private void InitializeComponent() {
			Text = "Sistema de Gestao de Chamados";
			WindowState = FormWindowState.Maximized;
			StartPosition = FormStartPosition.CenterScreen;
			BackColor = Color.WhiteSmoke;
			Panel pnlMenu = new Panel { Dock = DockStyle.Left, Width = 200, BackColor = Color.DarkBlue, Padding = new Padding(10) };
			Label lbl = new Label { Text = "MENU", Font = new Font("Arial", 14, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Top, Height = 50, TextAlign = ContentAlignment.MiddleCenter };
			Button btn1 = new Button { Text = "Clientes", Dock = DockStyle.Top, Height = 50, BackColor = Color.CornflowerBlue, ForeColor = Color.White };
			btn1.Click += (s, e) => new FrmClientes().ShowDialog();
			Button btn2 = new Button { Text = "Atendentes", Dock = DockStyle.Top, Height = 50, BackColor = Color.CornflowerBlue, ForeColor = Color.White };
			btn2.Click += (s, e) => new FrmAtendentes().ShowDialog();
			Button btn3 = new Button { Text = "Chamados", Dock = DockStyle.Top, Height = 50, BackColor = Color.CornflowerBlue, ForeColor = Color.White };
			btn3.Click += (s, e) => new FrmChamados().ShowDialog();
			Button btn4 = new Button { Text = "Sair", Dock = DockStyle.Bottom, Height = 50, BackColor = Color.Red, ForeColor = Color.White };
			btn4.Click += (s, e) => Close();
			pnlMenu.Controls.AddRange(new[] { btn4, btn3, btn2, btn1, lbl });
			Panel pnlContent = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
			Label lblWelcome = new Label { Text = "Bem-vindo ao Sistema", Font = new Font("Arial", 18, FontStyle.Bold), ForeColor = Color.DarkBlue, Dock = DockStyle.Top, Height = 100, TextAlign = ContentAlignment.MiddleCenter };
			pnlContent.Controls.Add(lblWelcome);
			Controls.AddRange(new[] { pnlContent, pnlMenu });
		}
	}
}
'@
$frmprincipal | Out-File "FrmPrincipal.cs" -Encoding UTF8 -Force
Write-Host "FrmPrincipal.cs criado"

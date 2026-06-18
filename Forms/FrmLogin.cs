using System;
using System.Drawing;
using System.Windows.Forms;
using GestaoChamados.Services;

namespace GestaoChamados
{
    public class FrmLogin : Form
    {
        private TextBox txtLogin = null!;
        private TextBox txtSenha = null!;
        private Button btnEntrar = null!;
        private AutenticacaoService authService = new AutenticacaoService();

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Login - Gestão de Chamados";
            Size = new Size(400, 300);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = ColorTranslator.FromHtml("#F5F7FA");
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            Font fontLabel = new Font("Segoe UI", 10, FontStyle.Regular);
            Font fontInput = new Font("Segoe UI", 12, FontStyle.Regular);

            Panel pnlCenter = new Panel
            {
                Size = new Size(300, 200),
                Location = new Point(50, 40),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblTitulo = new Label
            {
                Text = "ACESSO AO SISTEMA",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#0F172A"),
                Location = new Point(20, 20),
                Width = 260,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 30
            };

            Label lblLogin = new Label { Text = "Usuário:", Location = new Point(20, 60), Width = 100, Font = fontLabel };
            txtLogin = new TextBox { Location = new Point(20, 80), Width = 260, Font = fontInput };

            Label lblSenha = new Label { Text = "Senha:", Location = new Point(20, 110), Width = 100, Font = fontLabel };
            txtSenha = new TextBox { Location = new Point(20, 130), Width = 260, Font = fontInput, PasswordChar = '*' };

            btnEntrar = new Button
            {
                Text = "Entrar",
                Location = new Point(100, 170),
                Width = 100,
                Height = 40,
                BackColor = ColorTranslator.FromHtml("#2563EB"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEntrar.FlatAppearance.BorderSize = 0;
            btnEntrar.Click += BtnEntrar_Click;

            pnlCenter.Controls.AddRange(new Control[] { lblTitulo, lblLogin, txtLogin, lblSenha, txtSenha, btnEntrar });
            Controls.Add(pnlCenter);
        }

        private void BtnEntrar_Click(object? sender, EventArgs e)
        {
            try
            {
                authService.Autenticar(txtLogin.Text.Trim(), txtSenha.Text.Trim());

                FrmPrincipal frmPrincipal = new FrmPrincipal();
                this.Hide();
                frmPrincipal.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro de Autenticação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using GestaoChamados.Models;
using GestaoChamados.Services;

namespace GestaoChamados
{
    public class FrmUsuarios : Form
    {
        private UsuarioService service = new UsuarioService();
        private DataGridView dgvUsuarios = null!;
        private TextBox txtNome = null!;
        private TextBox txtLogin = null!;
        private TextBox txtSenha = null!;
        private ComboBox cbPapel = null!;
        private Button btnAdicionar = null!;

        public FrmUsuarios()
        {
            InitializeComponent();
            CarregarUsuarios();
        }

        private void InitializeComponent()
        {
            Text = "Gerenciamento de Usuários";
            Size = new Size(900, 600);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = ColorTranslator.FromHtml("#F5F7FA");

            Label lblTitulo = new Label
            {
                Text = "GERENCIAMENTO DE USUÁRIOS",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#0F172A"),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };

            Panel pnlCard = new Panel
            {
                Dock = DockStyle.Top,
                Height = 160,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Panel pnlCardContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 180,
                Padding = new Padding(20, 0, 20, 20),
                BackColor = Color.Transparent
            };
            pnlCardContainer.Controls.Add(pnlCard);

            Font fontLabel = new Font("Segoe UI", 10, FontStyle.Bold);
            Font fontInput = new Font("Segoe UI", 10, FontStyle.Regular);

            Label lblNome = new Label { Text = "Nome:", Location = new Point(20, 20), Width = 80, Font = fontLabel, ForeColor = Color.Gray };
            txtNome = new TextBox { Location = new Point(20, 45), Width = 200, Font = fontInput, BorderStyle = BorderStyle.FixedSingle };

            Label lblLogin = new Label { Text = "Login:", Location = new Point(240, 20), Width = 80, Font = fontLabel, ForeColor = Color.Gray };
            txtLogin = new TextBox { Location = new Point(240, 45), Width = 200, Font = fontInput, BorderStyle = BorderStyle.FixedSingle };

            Label lblSenha = new Label { Text = "Senha:", Location = new Point(460, 20), Width = 80, Font = fontLabel, ForeColor = Color.Gray };
            txtSenha = new TextBox { Location = new Point(460, 45), Width = 200, Font = fontInput, BorderStyle = BorderStyle.FixedSingle, PasswordChar = '*' };

            Label lblPapel = new Label { Text = "Papel:", Location = new Point(680, 20), Width = 80, Font = fontLabel, ForeColor = Color.Gray };
            cbPapel = new ComboBox { Location = new Point(680, 45), Width = 150, Font = fontInput, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };

            // Papéis: 1=Admin, 2=Operador, 3=Visualizador
            var papéis = new Dictionary<string, int> { { "Administrador", 1 }, { "Operador", 2 }, { "Visualizador", 3 } };
            cbPapel.DataSource = new List<KeyValuePair<string, int>>(papéis);
            cbPapel.DisplayMember = "Key";
            cbPapel.ValueMember = "Value";

            btnAdicionar = new Button
            {
                Text = "Adicionar Usuário",
                Location = new Point(20, 100),
                Width = 160,
                Height = 40,
                BackColor = ColorTranslator.FromHtml("#10B981"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAdicionar.FlatAppearance.BorderSize = 0;
            btnAdicionar.MouseEnter += (s, e) => btnAdicionar.BackColor = ColorTranslator.FromHtml("#059669");
            btnAdicionar.MouseLeave += (s, e) => btnAdicionar.BackColor = ColorTranslator.FromHtml("#10B981");
            btnAdicionar.Click += BtnAdicionar_Click;

            Button btnExcluir = new Button
            {
                Text = "Excluir Selecionado",
                Location = new Point(200, 100),
                Width = 160,
                Height = 40,
                BackColor = ColorTranslator.FromHtml("#EF4444"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExcluir.FlatAppearance.BorderSize = 0;
            btnExcluir.MouseEnter += (s, e) => btnExcluir.BackColor = ColorTranslator.FromHtml("#B91C1C");
            btnExcluir.MouseLeave += (s, e) => btnExcluir.BackColor = ColorTranslator.FromHtml("#EF4444");
            btnExcluir.Click += BtnExcluir_Click;

            pnlCard.Controls.AddRange(new Control[] { lblNome, txtNome, lblLogin, txtLogin, lblSenha, txtSenha, lblPapel, cbPapel, btnAdicionar, btnExcluir });

            Panel pnlGridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 0, 20, 20),
                BackColor = Color.Transparent
            };

            dgvUsuarios = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F8FAFC");
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#475569");
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            dgvUsuarios.ColumnHeadersHeight = 45;

            dgvUsuarios.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgvUsuarios.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#0F172A");
            dgvUsuarios.DefaultCellStyle.BackColor = Color.White;
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#E2E8F0");
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#0F172A");
            dgvUsuarios.DefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            dgvUsuarios.RowTemplate.Height = 40;
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F8FAFC");

            dgvUsuarios.Columns.Add("Id", "ID");
            dgvUsuarios.Columns.Add("Nome", "Nome");
            dgvUsuarios.Columns.Add("Login", "Login");
            dgvUsuarios.Columns.Add("Papel", "Papel");

            var colId = dgvUsuarios.Columns["Id"];
            if (colId != null)
            {
                colId.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colId.Width = 60;
            }

            pnlGridContainer.Controls.Add(dgvUsuarios);

            Controls.AddRange(new Control[] { pnlGridContainer, pnlCardContainer, lblTitulo });
        }

        private void BtnAdicionar_Click(object? sender, EventArgs e)
        {
            try
            {
                string nome = txtNome.Text.Trim();
                string login = txtLogin.Text.Trim();
                string senha = txtSenha.Text.Trim();
                int papelId = (int)(cbPapel.SelectedValue ?? 0);

                service.Cadastrar(nome, login, senha, papelId);
                MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtNome.Clear();
                txtLogin.Clear();
                txtSenha.Clear();
                CarregarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExcluir_Click(object? sender, EventArgs e)
        {
            try
            {
                var row = dgvUsuarios.CurrentRow;
                if (row == null) return;

                int id = Convert.ToInt32(row.Cells[0].Value);

                if (MessageBox.Show("Deseja realmente excluir este usuário?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    service.Excluir(id);
                    MessageBox.Show("Usuário removido!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarUsuarios();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarUsuarios()
        {
            dgvUsuarios.Rows.Clear();
            foreach (var u in service.Listar())
            {
                string papelNome = u.PapelId switch { 1 => "Administrador", 2 => "Operador", 3 => "Visualizador", _ => "Desconhecido" };
                dgvUsuarios.Rows.Add(u.Id, u.Nome, u.Login, papelNome);
            }
        }
    }
}

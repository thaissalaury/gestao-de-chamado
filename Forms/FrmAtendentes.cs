using System;
using System.Windows.Forms;
using System.Drawing;
using GestaoChamados.Models;
using GestaoChamados.Services;

namespace GestaoChamados
{
    public class FrmAtendentes : Form
    {
        private AtendenteService service = new AtendenteService();
        private DataGridView dgvAtendentes = null!;
        private TextBox txtNome = null!;
        private TextBox txtSetor = null!;
        private Button btnAdicionar = null!;

        public FrmAtendentes()
        {
            InitializeComponent();
            CarregarAtendentes();
        }

        private void InitializeComponent()
        {
            Text = "Cadastro de Atendentes";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = ColorTranslator.FromHtml("#F5F7FA");

            Label lblTitulo = new Label
            {
                Text = "GERENCIAMENTO DE ATENDENTES",
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

            Label lblNome = new Label { Text = "Nome do Atendente:", Location = new Point(20, 20), Width = 150, Font = fontLabel, ForeColor = Color.Gray };
            txtNome = new TextBox { Location = new Point(20, 45), Width = 350, Font = fontInput, BorderStyle = BorderStyle.FixedSingle };

            Label lblSetor = new Label { Text = "Setor de Atuação:", Location = new Point(400, 20), Width = 250, Font = fontLabel, ForeColor = Color.Gray };
            txtSetor = new TextBox { Location = new Point(400, 45), Width = 300, Font = fontInput, BorderStyle = BorderStyle.FixedSingle };

            btnAdicionar = new Button
            {
                Text = "Adicionar Atendente",
                Location = new Point(20, 100),
                Width = 170,
                Height = 40,
                BackColor = ColorTranslator.FromHtml("#10B981"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAdicionar.FlatAppearance.BorderSize = 0;
            btnAdicionar.Enabled = SessaoService.PodeEscrever; // RBAC
            btnAdicionar.MouseEnter += (s, e) => btnAdicionar.BackColor = ColorTranslator.FromHtml("#059669");
            btnAdicionar.MouseLeave += (s, e) => btnAdicionar.BackColor = ColorTranslator.FromHtml("#10B981");
            btnAdicionar.Click += BtnAdicionar_Click;

            Button btnLimpar = new Button
            {
                Text = "Limpar",
                Location = new Point(210, 100),
                Width = 100,
                Height = 40,
                BackColor = ColorTranslator.FromHtml("#64748B"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLimpar.FlatAppearance.BorderSize = 0;
            btnLimpar.MouseEnter += (s, e) => btnLimpar.BackColor = ColorTranslator.FromHtml("#475569");
            btnLimpar.MouseLeave += (s, e) => btnLimpar.BackColor = ColorTranslator.FromHtml("#64748B");
            btnLimpar.Click += (s, e) => { txtNome.Clear(); txtSetor.Clear(); };

            pnlCard.Controls.AddRange(new Control[] { lblNome, txtNome, lblSetor, txtSetor, btnAdicionar, btnLimpar });

            Panel pnlGridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 0, 20, 20),
                BackColor = Color.Transparent
            };

            dgvAtendentes = new DataGridView
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

            dgvAtendentes.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F8FAFC");
            dgvAtendentes.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#475569");
            dgvAtendentes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvAtendentes.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            dgvAtendentes.ColumnHeadersHeight = 45;

            dgvAtendentes.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgvAtendentes.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#0F172A");
            dgvAtendentes.DefaultCellStyle.BackColor = Color.White;
            dgvAtendentes.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#E2E8F0");
            dgvAtendentes.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#0F172A");
            dgvAtendentes.DefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            dgvAtendentes.RowTemplate.Height = 40;
            dgvAtendentes.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F8FAFC");

            dgvAtendentes.Columns.Add("Id", "ID");
            dgvAtendentes.Columns.Add("Nome", "Nome do Atendente");
            dgvAtendentes.Columns.Add("Setor", "Setor");

            var colId = dgvAtendentes.Columns["Id"];
            if (colId != null)
            {
                colId.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colId.Width = 80;
            }

            pnlGridContainer.Controls.Add(dgvAtendentes);

            Controls.AddRange(new Control[] { pnlGridContainer, pnlCardContainer, lblTitulo });
        }

        private void BtnAdicionar_Click(object? sender, EventArgs e)
        {
            try
            {
                string nome = txtNome.Text.Trim();
                string setor = txtSetor.Text.Trim();

                if (string.IsNullOrWhiteSpace(nome))
                {
                    MessageBox.Show("Por favor, insira o nome do atendente.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNome.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(setor))
                {
                    MessageBox.Show("Por favor, insira o setor do atendente.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSetor.Focus();
                    return;
                }

                service.Cadastrar(nome, setor);
                MessageBox.Show("Atendente cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Clear();
                txtSetor.Clear();
                CarregarAtendentes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar atendente: " + ex.Message, "Erro do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarAtendentes()
        {
            dgvAtendentes.Rows.Clear();
            foreach (var atendente in service.Listar())
            {
                dgvAtendentes.Rows.Add(atendente.Id, atendente.Nome, atendente.Setor);
            }
        }
    }
}

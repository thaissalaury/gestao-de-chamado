using System;
using System.Windows.Forms;
using System.Drawing;
using GestaoChamados.Models;
using GestaoChamados.Services;

namespace GestaoChamados
{
    public class FrmClientes : Form
    {
        private ClienteService service = new ClienteService();
        private DataGridView dgvClientes = null!;
        private TextBox txtNome = null!;
        private TextBox txtContato = null!;
        private Button btnAdicionar = null!;

        public FrmClientes()
        {
            InitializeComponent();
            CarregarClientes();
        }

        private void InitializeComponent()
        {
            Text = "Cadastro de Clientes";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = ColorTranslator.FromHtml("#F5F7FA");

            Label lblTitulo = new Label
            {
                Text = "GERENCIAMENTO DE CLIENTES",
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

            Label lblNome = new Label { Text = "Nome do Cliente:", Location = new Point(20, 20), Width = 150, Font = fontLabel, ForeColor = Color.Gray };
            txtNome = new TextBox { Location = new Point(20, 45), Width = 350, Font = fontInput, BorderStyle = BorderStyle.FixedSingle };

            Label lblContato = new Label { Text = "Contato (E-mail ou Telefone):", Location = new Point(400, 20), Width = 250, Font = fontLabel, ForeColor = Color.Gray };
            txtContato = new TextBox { Location = new Point(400, 45), Width = 300, Font = fontInput, BorderStyle = BorderStyle.FixedSingle };

            btnAdicionar = new Button
            {
                Text = "Adicionar Cliente",
                Location = new Point(20, 100),
                Width = 150,
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
                Location = new Point(190, 100),
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
            btnLimpar.Click += (s, e) => { txtNome.Clear(); txtContato.Clear(); };

            pnlCard.Controls.AddRange(new Control[] { lblNome, txtNome, lblContato, txtContato, btnAdicionar, btnLimpar });

            Panel pnlGridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 0, 20, 20),
                BackColor = Color.Transparent
            };

            dgvClientes = new DataGridView
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

            dgvClientes.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F8FAFC");
            dgvClientes.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#475569");
            dgvClientes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvClientes.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            dgvClientes.ColumnHeadersHeight = 45;

            dgvClientes.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgvClientes.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#0F172A");
            dgvClientes.DefaultCellStyle.BackColor = Color.White;
            dgvClientes.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#E2E8F0");
            dgvClientes.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#0F172A");
            dgvClientes.DefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            dgvClientes.RowTemplate.Height = 40;
            dgvClientes.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F8FAFC");

            dgvClientes.Columns.Add("Id", "ID");
            dgvClientes.Columns.Add("Nome", "Nome do Cliente");
            dgvClientes.Columns.Add("Contato", "Contato");

            var colId = dgvClientes.Columns["Id"];
            if (colId != null)
            {
                colId.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colId.Width = 80;
            }

            pnlGridContainer.Controls.Add(dgvClientes);

            Controls.AddRange(new Control[] { pnlGridContainer, pnlCardContainer, lblTitulo });
        }

        private void BtnAdicionar_Click(object? sender, EventArgs e)
        {
            try
            {
                string nome = txtNome.Text.Trim();
                string contato = txtContato.Text.Trim();

                if (string.IsNullOrWhiteSpace(nome))
                {
                    MessageBox.Show("Por favor, insira o nome do cliente.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNome.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(contato))
                {
                    MessageBox.Show("Por favor, insira o contato do cliente.", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtContato.Focus();
                    return;
                }

                service.Cadastrar(nome, contato);
                MessageBox.Show("Cliente cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Clear();
                txtContato.Clear();
                CarregarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar cliente: " + ex.Message, "Erro do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarClientes()
        {
            dgvClientes.Rows.Clear();
            foreach (var cliente in service.Listar())
            {
                dgvClientes.Rows.Add(cliente.Id, cliente.Nome, cliente.Contato);
            }
        }
    }
}

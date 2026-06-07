using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using GestaoChamados.Models;
using GestaoChamados.Services;

namespace GestaoChamados
{
    public class FrmChamados : Form
    {
        private ChamadoService chamadoService = new ChamadoService();
        private ClienteService clienteService = new ClienteService();
        private AtendenteService atendenteService = new AtendenteService();
        private DataGridView dgvChamados;
        private ComboBox cbCliente;
        private ComboBox cbAtendente;
        private TextBox txtDescricao;
        private TextBox txtBusca;
        private ComboBox cbStatus;
        private Button btnAtualizarStatus;

        public FrmChamados()
        {
            InitializeComponent();
            CarregarClientes();
            CarregarAtendentes();
            CarregarChamados();
        }

        private void InitializeComponent()
        {
            Text = "Gestão de Chamados";
            Size = new Size(1100, 750);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = ColorTranslator.FromHtml("#F5F7FA"); // Fundo principal

            Font fontLabel = new Font("Segoe UI", 10, FontStyle.Bold);
            Font fontInput = new Font("Segoe UI", 10, FontStyle.Regular);

            // Título
            Label lblTitulo = new Label
            {
                Text = "CENTRAL DE CHAMADOS",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#0F172A"),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };

            // Painel Topo (Container dos Cards)
            Panel pnlTopContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 250,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 0, 20, 20)
            };

            // Card Novo Chamado
            Panel pnlNovo = new Panel
            {
                Width = 600,
                Height = 230,
                BackColor = Color.White,
                Location = new Point(20, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblTituloNovo = new Label { Text = "ABRIR NOVO CHAMADO", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#2563EB"), Location = new Point(20, 15), Width = 300 };
            
            Label lblCliente = new Label { Text = "Cliente:", Location = new Point(20, 50), Width = 100, Font = fontLabel, ForeColor = Color.Gray };
            cbCliente = new ComboBox { Location = new Point(20, 75), Width = 250, Font = fontInput, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };

            Label lblAtendente = new Label { Text = "Atendente:", Location = new Point(290, 50), Width = 100, Font = fontLabel, ForeColor = Color.Gray };
            cbAtendente = new ComboBox { Location = new Point(290, 75), Width = 250, Font = fontInput, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };

            Label lblDescricao = new Label { Text = "Descrição do Problema:", Location = new Point(20, 115), Width = 200, Font = fontLabel, ForeColor = Color.Gray };
            txtDescricao = new TextBox { Location = new Point(20, 140), Width = 400, Height = 70, Font = fontInput, Multiline = true, BorderStyle = BorderStyle.FixedSingle, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };

            Button btnAbrir = new Button 
            { 
                Text = "Abrir Chamado", 
                Location = new Point(440, 140),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Width = 140, 
                Height = 70, 
                BackColor = ColorTranslator.FromHtml("#10B981"), 
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAbrir.FlatAppearance.BorderSize = 0;
            btnAbrir.MouseEnter += (s, e) => btnAbrir.BackColor = ColorTranslator.FromHtml("#059669");
            btnAbrir.MouseLeave += (s, e) => btnAbrir.BackColor = ColorTranslator.FromHtml("#10B981");
            btnAbrir.Click += BtnAbrir_Click;

            pnlNovo.Controls.AddRange(new Control[] { lblTituloNovo, lblCliente, cbCliente, lblAtendente, cbAtendente, lblDescricao, txtDescricao, btnAbrir });

            // Card Gerenciar / Filtro
            Panel pnlGerenciar = new Panel
            {
                Width = 380,
                Height = 230,
                BackColor = Color.White,
                Location = new Point(640, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            Label lblTituloGerenciar = new Label { Text = "GERENCIAR & BUSCAR", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#F59E0B"), Location = new Point(20, 15), Width = 300 };

            Label lblBusca = new Label { Text = "Buscar Chamado:", Location = new Point(20, 50), Width = 150, Font = fontLabel, ForeColor = Color.Gray };
            txtBusca = new TextBox { Location = new Point(20, 75), Width = 340, Font = fontInput, BorderStyle = BorderStyle.FixedSingle };
            txtBusca.TextChanged += (s, e) => FiltrarChamados();

            Label lblStatus = new Label { Text = "Alterar Status:", Location = new Point(20, 115), Width = 150, Font = fontLabel, ForeColor = Color.Gray };
            cbStatus = new ComboBox { Location = new Point(20, 140), Width = 200, Font = fontInput, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            cbStatus.DataSource = Enum.GetValues(typeof(StatusChamado));

            btnAtualizarStatus = new Button 
            { 
                Text = "Atualizar", 
                Location = new Point(240, 140), 
                Width = 120, 
                Height = 28, 
                BackColor = ColorTranslator.FromHtml("#2563EB"), 
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAtualizarStatus.FlatAppearance.BorderSize = 0;
            btnAtualizarStatus.MouseEnter += (s, e) => btnAtualizarStatus.BackColor = ColorTranslator.FromHtml("#3B82F6");
            btnAtualizarStatus.MouseLeave += (s, e) => btnAtualizarStatus.BackColor = ColorTranslator.FromHtml("#2563EB");
            btnAtualizarStatus.Click += BtnAtualizarStatus_Click;

            pnlGerenciar.Controls.AddRange(new Control[] { lblTituloGerenciar, lblBusca, txtBusca, lblStatus, cbStatus, btnAtualizarStatus });

            pnlTopContainer.Controls.Add(pnlNovo);
            pnlTopContainer.Controls.Add(pnlGerenciar);

            // Container do Grid
            Panel pnlGridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 0, 20, 20),
                BackColor = Color.Transparent
            };

            dgvChamados = new DataGridView 
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

            // Estilo do Cabeçalho
            dgvChamados.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F8FAFC");
            dgvChamados.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#475569");
            dgvChamados.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvChamados.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            dgvChamados.ColumnHeadersHeight = 45;

            // Estilo das Linhas
            dgvChamados.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgvChamados.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#0F172A");
            dgvChamados.DefaultCellStyle.BackColor = Color.White;
            dgvChamados.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#E2E8F0");
            dgvChamados.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#0F172A");
            dgvChamados.DefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            dgvChamados.RowTemplate.Height = 45;
            dgvChamados.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F8FAFC");

            dgvChamados.Columns.Add("Id", "ID");
            dgvChamados.Columns.Add("Cliente", "Cliente");
            dgvChamados.Columns.Add("Atendente", "Atendente");
            dgvChamados.Columns.Add("Descricao", "Descrição");
            dgvChamados.Columns.Add("Status", "Status");
            dgvChamados.Columns.Add("Data", "Data Abertura");
            
            dgvChamados.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvChamados.Columns["Id"].Width = 60;
            dgvChamados.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvChamados.Columns["Status"].Width = 150;

            // Formatação Condicional de Status
            dgvChamados.CellFormatting += DgvChamados_CellFormatting;

            pnlGridContainer.Controls.Add(dgvChamados);

            Controls.AddRange(new Control[] { pnlGridContainer, pnlTopContainer, lblTitulo });
        }

        private void DgvChamados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvChamados.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                
                // Configura fundo para ficar como uma "Tag"
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                if (status == "Aberto")
                {
                    e.CellStyle.BackColor = ColorTranslator.FromHtml("#EF4444"); // Vermelho
                    e.CellStyle.SelectionBackColor = ColorTranslator.FromHtml("#EF4444");
                    e.Value = "ABERTO";
                }
                else if (status == "EmAndamento")
                {
                    e.CellStyle.BackColor = ColorTranslator.FromHtml("#F59E0B"); // Laranja
                    e.Value = "EM ANDAMENTO"; 
                    e.CellStyle.SelectionBackColor = ColorTranslator.FromHtml("#F59E0B");
                }
                else if (status == "Resolvido")
                {
                    e.CellStyle.BackColor = ColorTranslator.FromHtml("#10B981"); // Verde
                    e.CellStyle.SelectionBackColor = ColorTranslator.FromHtml("#10B981");
                    e.Value = "RESOLVIDO";
                }
            }
        }

        private void BtnAtualizarStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvChamados.CurrentRow == null)
                {
                    MessageBox.Show("Selecione um chamado na lista!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dgvChamados.CurrentRow.Cells[0].Value == null || !int.TryParse(dgvChamados.CurrentRow.Cells[0].Value.ToString(), out int id))
                {
                    MessageBox.Show("Falha ao ler o ID do chamado selecionado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Selecione um status válido para atualizar!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                StatusChamado novoStatus = (StatusChamado)cbStatus.SelectedItem;

                chamadoService.AlterarStatus(id, novoStatus);
                MessageBox.Show("Status atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarChamados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbCliente.SelectedIndex < 0)
                {
                    MessageBox.Show("Por favor, selecione um cliente!", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbCliente.Focus();
                    return;
                }

                if (cbAtendente.SelectedIndex < 0)
                {
                    MessageBox.Show("Por favor, selecione um atendente!", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbAtendente.Focus();
                    return;
                }

                string descricao = txtDescricao.Text.Trim();
                if (string.IsNullOrWhiteSpace(descricao))
                {
                    MessageBox.Show("Por favor, digite a descrição do chamado!", "Campo Obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDescricao.Focus();
                    return;
                }

                int clienteId = Convert.ToInt32(cbCliente.SelectedValue);
                int atendenteId = Convert.ToInt32(cbAtendente.SelectedValue);
                var cliente = clienteService.BuscarPorId(clienteId);
                var atendente = atendenteService.BuscarPorId(atendenteId);

                chamadoService.Abrir(cliente, atendente, descricao);
                MessageBox.Show("Chamado aberto com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDescricao.Clear();
                cbCliente.SelectedIndex = -1;
                cbAtendente.SelectedIndex = -1;
                CarregarChamados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir chamado: " + ex.Message, "Erro do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarClientes()
        {
            var clientes = clienteService.Listar();
            cbCliente.DataSource = new List<Cliente>(clientes);
            cbCliente.DisplayMember = "Nome";
            cbCliente.ValueMember = "Id";
            cbCliente.SelectedIndex = -1;
        }

        private void CarregarAtendentes()
        {
            var atendentes = atendenteService.Listar();
            cbAtendente.DataSource = new List<Atendente>(atendentes);
            cbAtendente.DisplayMember = "Nome";
            cbAtendente.ValueMember = "Id";
            cbAtendente.SelectedIndex = -1;
        }

        private void CarregarChamados()
        {
            dgvChamados.Rows.Clear();
            foreach (var chamado in chamadoService.Listar())
            {
                dgvChamados.Rows.Add(
                    chamado.Id,
                    chamado.Cliente?.Nome ?? "N/A",
                    chamado.Atendente?.Nome ?? "N/A",
                    chamado.Descricao,
                    chamado.Status.ToString(),
                    chamado.DataAbertura.ToString("dd/MM/yyyy HH:mm")
                );
            }
        }

        private void FiltrarChamados()
        {
            string termo = txtBusca.Text.ToLower();
            var filtrados = chamadoService.Listar()
                .Where(c => c.Descricao.ToLower().Contains(termo) || c.Cliente.Nome.ToLower().Contains(termo))
                .ToList();

            dgvChamados.Rows.Clear();
            foreach (var chamado in filtrados)
            {
                dgvChamados.Rows.Add(
                    chamado.Id,
                    chamado.Cliente?.Nome ?? "N/A",
                    chamado.Atendente?.Nome ?? "N/A",
                    chamado.Descricao,
                    chamado.Status.ToString(),
                    chamado.DataAbertura.ToString("dd/MM/yyyy HH:mm")
                );
            }
        }
    }
}

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
            Text = "Gestao de Chamados";
            Size = new Size(900, 600);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.WhiteSmoke;

            Panel pnlForm = new Panel { Dock = DockStyle.Top, Height = 220, BackColor = Color.LightGray, Padding = new Padding(15) };

            Label lblBusca = new Label { Text = "Buscar:", Location = new Point(15, 15), Width = 80, Font = new Font("Arial", 10) };
            txtBusca = new TextBox { Location = new Point(100, 15), Width = 300, Height = 25 };
            txtBusca.TextChanged += (s, e) => FiltrarChamados();

            Label lblCliente = new Label { Text = "Cliente:", Location = new Point(15, 55), Width = 80, Font = new Font("Arial", 10) };
            cbCliente = new ComboBox { Location = new Point(100, 55), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblAtendente = new Label { Text = "Atendente:", Location = new Point(15, 95), Width = 80, Font = new Font("Arial", 10) };
            cbAtendente = new ComboBox { Location = new Point(100, 95), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblDescricao = new Label { Text = "Descricao:", Location = new Point(15, 135), Width = 80, Font = new Font("Arial", 10) };
            txtDescricao = new TextBox { Location = new Point(100, 135), Width = 400, Height = 50, Multiline = true };

            Button btnAbrir = new Button { Text = "Abrir Chamado", Location = new Point(100, 195), Width = 100, Height = 30, BackColor = Color.Green, ForeColor = Color.White };
            btnAbrir.Click += BtnAbrir_Click;

            Label lblStatus = new Label { Text = "Status:", Location = new Point(450, 55), Width = 80, Font = new Font("Arial", 10) };
            cbStatus = new ComboBox { Location = new Point(530, 55), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cbStatus.DataSource = Enum.GetValues(typeof(StatusChamado));

            btnAtualizarStatus = new Button { Text = "Atualizar Status", Location = new Point(530, 95), Width = 150, Height = 30, BackColor = Color.CornflowerBlue, ForeColor = Color.White };
            btnAtualizarStatus.Click += BtnAtualizarStatus_Click;

            pnlForm.Controls.AddRange(new Control[] { lblBusca, txtBusca, lblCliente, cbCliente, lblAtendente, cbAtendente, lblDescricao, txtDescricao, btnAbrir, lblStatus, cbStatus, btnAtualizarStatus });

            dgvChamados = new DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dgvChamados.Columns.Add("Id", "ID");
            dgvChamados.Columns.Add("Cliente", "Cliente");
            dgvChamados.Columns.Add("Atendente", "Atendente");
            dgvChamados.Columns.Add("Descricao", "Descricao");
            dgvChamados.Columns.Add("Status", "Status");
            dgvChamados.Columns.Add("Data", "Data");

            Controls.AddRange(new Control[] { dgvChamados, pnlForm });
        }

        private void BtnAtualizarStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvChamados.CurrentRow == null)
                {
                    MessageBox.Show("Selecione um chamado na lista!", "Validacao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = (int)dgvChamados.CurrentRow.Cells[0].Value;
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

                int clienteId = (int)cbCliente.SelectedValue;
                int atendenteId = (int)cbAtendente.SelectedValue;
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

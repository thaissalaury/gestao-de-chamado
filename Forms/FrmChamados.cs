using System.Windows.Forms;
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

            Panel pnlForm = new Panel { Dock = DockStyle.Top, Height = 160, BackColor = Color.LightGray, Padding = new Padding(15) };

            Label lblCliente = new Label { Text = "Cliente:", Location = new Point(15, 15), Width = 80, Font = new Font("Arial", 10) };
            cbCliente = new ComboBox { Location = new Point(100, 15), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblAtendente = new Label { Text = "Atendente:", Location = new Point(15, 55), Width = 80, Font = new Font("Arial", 10) };
            cbAtendente = new ComboBox { Location = new Point(100, 55), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblDescricao = new Label { Text = "Descricao:", Location = new Point(15, 95), Width = 80, Font = new Font("Arial", 10) };
            txtDescricao = new TextBox { Location = new Point(100, 95), Width = 400, Height = 50, Multiline = true };

            Button btnAbrir = new Button { Text = "Abrir Chamado", Location = new Point(100, 155), Width = 100, Height = 30, BackColor = Color.Green, ForeColor = Color.White };
            btnAbrir.Click += BtnAbrir_Click;

            pnlForm.Controls.AddRange(new Control[] { lblCliente, cbCliente, lblAtendente, cbAtendente, lblDescricao, txtDescricao, btnAbrir });

            dgvChamados = new DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, ReadOnly = true };
            dgvChamados.Columns.Add("Id", "ID");
            dgvChamados.Columns.Add("Cliente", "Cliente");
            dgvChamados.Columns.Add("Atendente", "Atendente");
            dgvChamados.Columns.Add("Descricao", "Descricao");
            dgvChamados.Columns.Add("Status", "Status");
            dgvChamados.Columns.Add("Data", "Data");

            Controls.AddRange(new Control[] { dgvChamados, pnlForm });
        }

        private void BtnAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbCliente.SelectedIndex < 0 || cbAtendente.SelectedIndex < 0)
                {
                    MessageBox.Show("Selecione cliente e atendente!", "Validacao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDescricao.Text))
                {
                    MessageBox.Show("Digite a descricao!", "Validacao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int clienteId = (int)cbCliente.SelectedValue;
                int atendenteId = (int)cbAtendente.SelectedValue;
                var cliente = clienteService.BuscarPorId(clienteId);
                var atendente = atendenteService.BuscarPorId(atendenteId);

                chamadoService.Abrir(cliente, atendente, txtDescricao.Text);
                MessageBox.Show("Chamado aberto com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDescricao.Clear();
                cbCliente.SelectedIndex = -1;
                cbAtendente.SelectedIndex = -1;
                CarregarChamados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}

using System.Windows.Forms;
using System.Drawing;
using GestaoChamados.Models;
using GestaoChamados.Services;

namespace GestaoChamados
{
    public class FrmClientes : Form
    {
        private ClienteService service = new ClienteService();
        private DataGridView dgvClientes;
        private TextBox txtNome;
        private TextBox txtContato;

        public FrmClientes()
        {
            InitializeComponent();
            CarregarClientes();
        }

        private void InitializeComponent()
        {
            Text = "Cadastro de Clientes";
            Size = new Size(700, 500);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.WhiteSmoke;

            Panel pnlForm = new Panel { Dock = DockStyle.Top, Height = 130, BackColor = Color.LightGray, Padding = new Padding(15) };

            Label lblNome = new Label { Text = "Nome:", Location = new Point(15, 15), Width = 80, Font = new Font("Arial", 10) };
            txtNome = new TextBox { Location = new Point(100, 15), Width = 300, Height = 25 };

            Label lblContato = new Label { Text = "Contato:", Location = new Point(15, 55), Width = 80, Font = new Font("Arial", 10) };
            txtContato = new TextBox { Location = new Point(100, 55), Width = 300, Height = 25 };

            Button btnAdicionar = new Button { Text = "Adicionar", Location = new Point(100, 90), Width = 80, Height = 30, BackColor = Color.Green, ForeColor = Color.White };
            btnAdicionar.Click += BtnAdicionar_Click;

            Button btnLimpar = new Button { Text = "Limpar", Location = new Point(190, 90), Width = 80, Height = 30, BackColor = Color.Gray, ForeColor = Color.White };
            btnLimpar.Click += (s, e) => { txtNome.Clear(); txtContato.Clear(); };

            pnlForm.Controls.AddRange(new Control[] { lblNome, txtNome, lblContato, txtContato, btnAdicionar, btnLimpar });

            dgvClientes = new DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, ReadOnly = true };
            dgvClientes.Columns.Add("Id", "ID");
            dgvClientes.Columns.Add("Nome", "Nome");
            dgvClientes.Columns.Add("Contato", "Contato");

            Controls.AddRange(new Control[] { dgvClientes, pnlForm });
        }

        private void BtnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtContato.Text))
                {
                    MessageBox.Show("Preencha todos os campos!", "Validacao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                service.Cadastrar(txtNome.Text, txtContato.Text);
                MessageBox.Show("Cliente adicionado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Clear();
                txtContato.Clear();
                CarregarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

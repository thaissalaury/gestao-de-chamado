using System.Windows.Forms;
using System.Drawing;
using GestaoChamados.Models;
using GestaoChamados.Services;

namespace GestaoChamados
{
    public class FrmAtendentes : Form
    {
        private AtendenteService service = new AtendenteService();
        private DataGridView dgvAtendentes;
        private TextBox txtNome;
        private TextBox txtSetor;

        public FrmAtendentes()
        {
            InitializeComponent();
            CarregarAtendentes();
        }

        private void InitializeComponent()
        {
            Text = "Cadastro de Atendentes";
            Size = new Size(700, 500);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.WhiteSmoke;

            Panel pnlForm = new Panel { Dock = DockStyle.Top, Height = 130, BackColor = Color.LightGray, Padding = new Padding(15) };

            Label lblNome = new Label { Text = "Nome:", Location = new Point(15, 15), Width = 80, Font = new Font("Arial", 10) };
            txtNome = new TextBox { Location = new Point(100, 15), Width = 300, Height = 25 };

            Label lblSetor = new Label { Text = "Setor:", Location = new Point(15, 55), Width = 80, Font = new Font("Arial", 10) };
            txtSetor = new TextBox { Location = new Point(100, 55), Width = 300, Height = 25 };

            Button btnAdicionar = new Button { Text = "Adicionar", Location = new Point(100, 90), Width = 80, Height = 30, BackColor = Color.Green, ForeColor = Color.White };
            btnAdicionar.Click += BtnAdicionar_Click;

            Button btnLimpar = new Button { Text = "Limpar", Location = new Point(190, 90), Width = 80, Height = 30, BackColor = Color.Gray, ForeColor = Color.White };
            btnLimpar.Click += (s, e) => { txtNome.Clear(); txtSetor.Clear(); };

            pnlForm.Controls.AddRange(new Control[] { lblNome, txtNome, lblSetor, txtSetor, btnAdicionar, btnLimpar });

            dgvAtendentes = new DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, ReadOnly = true };
            dgvAtendentes.Columns.Add("Id", "ID");
            dgvAtendentes.Columns.Add("Nome", "Nome");
            dgvAtendentes.Columns.Add("Setor", "Setor");

            Controls.AddRange(new Control[] { dgvAtendentes, pnlForm });
        }

        private void BtnAdicionar_Click(object sender, EventArgs e)
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

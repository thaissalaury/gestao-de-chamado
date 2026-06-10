using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using GestaoChamados.Models;
using GestaoChamados.Services;

namespace GestaoChamados
{
    public class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Sistema de Gestão de Chamados";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = ColorTranslator.FromHtml("#F5F7FA"); // Fundo principal

            // Menu Lateral
            Panel pnlMenu = new Panel();
            pnlMenu.Dock = DockStyle.Left;
            pnlMenu.Width = 250;
            pnlMenu.BackColor = ColorTranslator.FromHtml("#1E293B"); // Menu lateral
            pnlMenu.Padding = new Padding(0);

            Label lblTitulo = new Label 
            { 
                Text = "GESTÃO DE CHAMADO", 
                Font = new Font("Segoe UI", 14, FontStyle.Bold), 
                ForeColor = Color.White, 
                Dock = DockStyle.Top, 
                Height = 80, 
                TextAlign = ContentAlignment.MiddleCenter 
            };

            // Estilo padrão para os botões do menu
            Font fontMenu = new Font("Segoe UI", 12, FontStyle.Regular);
            Color corBotao = ColorTranslator.FromHtml("#1E293B");
            Color corHover = ColorTranslator.FromHtml("#3B82F6");

            Button btnChamados = new Button 
            { 
                Text = "Chamados", 
                Dock = DockStyle.Top, 
                Height = 60, 
                BackColor = corBotao, 
                ForeColor = Color.White, 
                Font = fontMenu,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(30, 0, 0, 0)
            };
            btnChamados.FlatAppearance.BorderSize = 0;
            btnChamados.MouseEnter += (s, e) => btnChamados.BackColor = corHover;
            btnChamados.MouseLeave += (s, e) => btnChamados.BackColor = corBotao;
            btnChamados.Click += (s, e) => { using (var frm = new FrmChamados()) frm.ShowDialog(); AtualizarDashboard(); };

            Button btnAtendentes = new Button 
            { 
                Text = "Atendentes", 
                Dock = DockStyle.Top, 
                Height = 60, 
                BackColor = corBotao, 
                ForeColor = Color.White, 
                Font = fontMenu,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(30, 0, 0, 0)
            };
            btnAtendentes.FlatAppearance.BorderSize = 0;
            btnAtendentes.MouseEnter += (s, e) => btnAtendentes.BackColor = corHover;
            btnAtendentes.MouseLeave += (s, e) => btnAtendentes.BackColor = corBotao;
            btnAtendentes.Click += (s, e) => { using (var frm = new FrmAtendentes()) frm.ShowDialog(); AtualizarDashboard(); };

            Button btnClientes = new Button 
            { 
                Text = "Clientes", 
                Dock = DockStyle.Top, 
                Height = 60, 
                BackColor = corBotao, 
                ForeColor = Color.White, 
                Font = fontMenu,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(30, 0, 0, 0)
            };
            btnClientes.FlatAppearance.BorderSize = 0;
            btnClientes.MouseEnter += (s, e) => btnClientes.BackColor = corHover;
            btnClientes.MouseLeave += (s, e) => btnClientes.BackColor = corBotao;
            btnClientes.Click += (s, e) => { using (var frm = new FrmClientes()) frm.ShowDialog(); AtualizarDashboard(); };

            Button btnSair = new Button 
            { 
                Text = "Sair", 
                Dock = DockStyle.Bottom, 
                Height = 60, 
                BackColor = ColorTranslator.FromHtml("#EF4444"), // Erro/Sair
                ForeColor = Color.White, 
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSair.FlatAppearance.BorderSize = 0;
            btnSair.Click += (s, e) => this.Close();

            pnlMenu.Controls.AddRange(new Control[] { btnSair, btnChamados, btnAtendentes, btnClientes, lblTitulo });

            // Dashboard Principal
            pnlContent = new Panel 
            { 
                Dock = DockStyle.Fill, 
                BackColor = ColorTranslator.FromHtml("#F5F7FA"),
                Padding = new Padding(40)
            };

            Label lblWelcome = new Label 
            { 
                Text = "Visão Geral", 
                Font = new Font("Segoe UI", 24, FontStyle.Bold), 
                ForeColor = ColorTranslator.FromHtml("#0F172A"), 
                Dock = DockStyle.Top, 
                Height = 80, 
                TextAlign = ContentAlignment.MiddleLeft 
            };

            // Container para os Cards
            pnlCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 200,
                BackColor = Color.Transparent,
                WrapContents = false
            };

            pnlContent.Controls.Add(pnlCards);
            pnlContent.Controls.Add(lblWelcome);

            Controls.AddRange(new Control[] { pnlContent, pnlMenu });

            AtualizarDashboard();
        }

        private Panel? pnlContent;
        private FlowLayoutPanel? pnlCards;

        private void AtualizarDashboard()
        {
            if (pnlCards == null) return;
            pnlCards.Controls.Clear();

            // Lê do arquivo JSON usando o fluxo existente de DataPersistence
            var clientes = DataPersistence.Load<Cliente>("clientes.json");
            var atendentes = DataPersistence.Load<Atendente>("atendentes.json");
            var chamados = DataPersistence.Load<Chamado>("chamados.json");

            int totalClientes = clientes.Count;
            int totalAtendentes = atendentes.Count;
            int chamadosAbertos = chamados.Count(c => c.Status == StatusChamado.Aberto);
            int chamadosResolvidos = chamados.Count(c => c.Status == StatusChamado.Resolvido);

            pnlCards.Controls.Add(CriarCard("Total de Clientes", totalClientes.ToString(), ColorTranslator.FromHtml("#2563EB")));
            pnlCards.Controls.Add(CriarCard("Total de Atendentes", totalAtendentes.ToString(), ColorTranslator.FromHtml("#10B981")));
            pnlCards.Controls.Add(CriarCard("Chamados Abertos", chamadosAbertos.ToString(), ColorTranslator.FromHtml("#F59E0B")));
            pnlCards.Controls.Add(CriarCard("Chamados Resolvidos", chamadosResolvidos.ToString(), ColorTranslator.FromHtml("#10B981")));
        }

        private Panel CriarCard(string titulo, string valor, Color corDestaque)
        {
            Panel card = new Panel
            {
                Width = 260,
                Height = 140,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 20, 0)
            };

            // Linha superior de destaque do card
            Panel linhaDestaque = new Panel
            {
                Height = 5,
                Dock = DockStyle.Top,
                BackColor = corDestaque
            };

            Label lblTitulo = new Label
            {
                Text = titulo.ToUpper(),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Gray,
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(15, 0, 0, 10)
            };

            Label lblValor = new Label
            {
                Text = valor,
                Font = new Font("Segoe UI", 36, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#0F172A"),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0)
            };

            card.Controls.Add(lblValor);
            card.Controls.Add(lblTitulo);
            card.Controls.Add(linhaDestaque);

            return card;
        }
    }
}

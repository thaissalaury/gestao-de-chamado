namespace GestaoChamados.Models
{
    public class Chamado
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; } = new Cliente();
        public Atendente Atendente { get; set; } = new Atendente();
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataAbertura { get; set; }
        public StatusChamado Status { get; set; }

        public Chamado() { }

        public Chamado(int id, Cliente cliente, Atendente atendente, string descricao, DateTime dataAbertura, StatusChamado status)
        {
            Id = id;
            Cliente = cliente;
            Atendente = atendente;
            Descricao = descricao;
            DataAbertura = dataAbertura;
            Status = status;
        }
    }
}

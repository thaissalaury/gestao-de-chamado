namespace GestaoChamados.Models
{
    public class Chamado
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        public Atendente Atendente { get; set; }
        public string Descricao { get; set; }
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

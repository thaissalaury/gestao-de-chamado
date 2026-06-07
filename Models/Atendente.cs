namespace GestaoChamados.Models
{
    public class Atendente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Setor { get; set; } = string.Empty;

        public Atendente() { }

        public Atendente(int id, string nome, string setor)
        {
            Id = id;
            Nome = nome;
            Setor = setor;
        }
    }
}

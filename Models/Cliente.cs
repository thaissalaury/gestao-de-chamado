namespace GestaoChamados.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Contato { get; set; } = string.Empty;

        public Cliente() { }

        public Cliente(int id, string nome, string contato)
        {
            Id = id;
            Nome = nome;
            Contato = contato;
        }
    }
}

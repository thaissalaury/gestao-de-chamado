namespace GestaoChamados.Models
{
    public class Papel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public Papel(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }
}

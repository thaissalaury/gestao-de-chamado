namespace GestaoChamados.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public int PapelId { get; set; }
        public Papel? Papel { get; set; }

        public Usuario(int id, string nome, string login, string senhaHash, int papelId)
        {
            Id = id;
            Nome = nome;
            Login = login;
            SenhaHash = senhaHash;
            PapelId = papelId;
        }
    }
}

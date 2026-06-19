namespace GestaoChamados.Models
{
    /// <summary>
    /// Representa a entidade de um Usuário no sistema com suas credenciais e nível de acesso (Papel).
    /// </summary>
    public class Usuario
    {
        // Identificador exclusivo do Usuário
        public int Id { get; set; }
        
        // Nome completo do Usuário
        public string Nome { get; set; } = string.Empty;
        
        // Login/Username utilizado no acesso
        public string Login { get; set; } = string.Empty;
        
        // Senha criptografada (hash com BCrypt)
        public string SenhaHash { get; set; } = string.Empty;
        
        // ID do papel associado para controle RBAC (Admin, Operador, Visualizador)
        public int PapelId { get; set; }
        
        // Propriedade de navegação do papel de acesso
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

using System;
using GestaoChamados.Models;
using GestaoChamados.Repositories;
using GestaoChamados.Services;

namespace GestaoChamados.Services
{
    /// <summary>
    /// Serviço encarregado de gerenciar a lógica de autenticação e validação de usuários.
    /// </summary>
    public class AutenticacaoService
    {
        private readonly UsuarioRepository _usuarioRepository = new UsuarioRepository();

        /// <summary>
        /// Realiza a tentativa de autenticação de um usuário no sistema.
        /// </summary>
        /// <param name="login">Nome de usuário ou login de acesso.</param>
        /// <param name="senha">Senha em texto puro para verificação.</param>
        /// <returns>O objeto Usuario correspondente se as credenciais forem válidas.</returns>
        public Usuario Autenticar(string login, string senha)
        {
            // Validações básicas de preenchimento
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
            {
                throw new ArgumentException("Login e senha são obrigatórios.");
            }

            // Busca o usuário no banco de dados SQLite correspondente ao login informado
            var usuario = _usuarioRepository.BuscarPorLogin(login);

            // Verifica se o usuário existe e compara o hash da senha usando BCrypt
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash))
            {
                throw new UnauthorizedAccessException("Login ou senha inválidos.");
            }

            // Define o usuário autenticado na sessão ativa do aplicativo
            SessaoService.Login(usuario);
            return usuario;
        }
    }
}

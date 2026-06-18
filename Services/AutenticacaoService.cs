using System;
using GestaoChamados.Models;
using GestaoChamados.Repositories;
using GestaoChamados.Services;

namespace GestaoChamados.Services
{
    public class AutenticacaoService
    {
        private readonly UsuarioRepository _usuarioRepository = new UsuarioRepository();

        public Usuario Autenticar(string login, string senha)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
            {
                throw new ArgumentException("Login e senha são obrigatórios.");
            }

            var usuario = _usuarioRepository.BuscarPorLogin(login);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash))
            {
                throw new UnauthorizedAccessException("Login ou senha inválidos.");
            }

            SessaoService.Login(usuario);
            return usuario;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using GestaoChamados.Models;
using GestaoChamados.Repositories;
using GestaoChamados.Services;

namespace GestaoChamados.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository = new UsuarioRepository();

        private void GarantirPermissaoDeEscrita()
        {
            if (!SessaoService.EhAdmin)
                throw new UnauthorizedAccessException("Apenas administradores podem gerenciar usuários.");
        }

        public void Cadastrar(string nome, string login, string senha, int papelId)
        {
            GarantirPermissaoDeEscrita();

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Login é obrigatório.");

            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("Senha é obrigatória.");

            string senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
            var usuario = new Usuario(0, nome, login, senhaHash, papelId);

            _usuarioRepository.Cadastrar(usuario);
        }

        public List<Usuario> Listar()
        {
            return _usuarioRepository.Listar();
        }

        public void Excluir(int id)
        {
            GarantirPermissaoDeEscrita();
            _usuarioRepository.Excluir(id);
        }

        public Usuario? BuscarPorId(int id)
        {
            return _usuarioRepository.BuscarPorId(id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using GestaoChamados.Models;
using GestaoChamados.Repositories;
using GestaoChamados.Services;

namespace GestaoChamados.Services
{
    public class ClienteService
    {
        private readonly ClienteRepository _clienteRepository = new ClienteRepository();

        private void GarantirPermissaoDeEscrita()
        {
            if (!SessaoService.PodeEscrever)
                throw new UnauthorizedAccessException("Seu papel não permite realizar esta ação.");
        }

        public void Cadastrar(string nome, string contato)
        {
            GarantirPermissaoDeEscrita();

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do cliente não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(contato))
                throw new ArgumentException("Contato do cliente não pode ser vazio.");

            var cliente = new Cliente(0, nome, contato);
            _clienteRepository.Cadastrar(cliente);
        }

        public List<Cliente> Listar()
        {
            return _clienteRepository.Listar();
        }

        public void Excluir(int id)
        {
            GarantirPermissaoDeEscrita();
            _clienteRepository.Excluir(id);
        }

        public Cliente? BuscarPorId(int id)

        {
            return _clienteRepository.BuscarPorId(id);
        }
    }
}

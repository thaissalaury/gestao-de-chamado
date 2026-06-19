using System;
using System.Collections.Generic;
using System.Linq;
using GestaoChamados.Models;
using GestaoChamados.Repositories;
using GestaoChamados.Services;

namespace GestaoChamados.Services
{
    public class AtendenteService
    {
        private readonly AtendenteRepository _atendenteRepository = new AtendenteRepository();
        private readonly ChamadoRepository _chamadoRepository = new ChamadoRepository();

        private void GarantirPermissaoDeEscrita()
        {
            if (!SessaoService.PodeEscrever)
                throw new UnauthorizedAccessException("Seu papel não permite realizar esta ação.");
        }

        private void GarantirPermissaoAdmin()
        {
            if (!SessaoService.EhAdmin)
                throw new UnauthorizedAccessException("Apenas administradores podem realizar esta ação.");
        }

        public void Cadastrar(string nome, string setor)
        {
            GarantirPermissaoDeEscrita();

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do atendente não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(setor))
                throw new ArgumentException("Setor do atendente não pode ser vazio.");

            var atendente = new Atendente(0, nome, setor);
            _atendenteRepository.Cadastrar(atendente);
        }

        public List<Atendente> Listar()
        {
            return _atendenteRepository.Listar();
        }

        public Atendente? BuscarPorId(int id)
        {
            return _atendenteRepository.BuscarPorId(id);
        }

        public void Excluir(int id)
        {
            GarantirPermissaoAdmin();

            if (_chamadoRepository.ContarPorAtendente(id) > 0)
            {
                throw new InvalidOperationException("Não é possível excluir este atendente pois existem chamados vinculados a ele.");
            }

            _atendenteRepository.Excluir(id);
        }
    }
}

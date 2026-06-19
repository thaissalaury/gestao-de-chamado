using System;
using System.Collections.Generic;
using System.Linq;
using GestaoChamados.Models;
using GestaoChamados.Repositories;
using GestaoChamados.Services;

namespace GestaoChamados.Services
{
    public class ChamadoService
    {
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

        public void Abrir(Cliente cliente, Atendente atendente, string descricao)
        {
            // Qualquer usuário autenticado (incluindo Visualizador) pode abrir chamados
            if (cliente == null)
                throw new ArgumentException("Cliente não pode ser nulo.");

            if (atendente == null)
                throw new ArgumentException("Atendente não pode ser nulo.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição do chamado não pode ser vazia.");

            var chamado = new Chamado(
                0,
                cliente,
                atendente,
                descricao,
                DateTime.Now,
                StatusChamado.Aberto
            );

            _chamadoRepository.Cadastrar(chamado);
        }

        public List<Chamado> Listar()
        {
            return _chamadoRepository.Listar();
        }

        public List<Chamado> ListarAbertos()
        {
            return _chamadoRepository.Listar()
                .Where(c => c.Status == StatusChamado.Aberto)
                .ToList();
        }

        public Chamado? BuscarPorId(int id)
        {
            return _chamadoRepository.BuscarPorId(id);
        }

        public void AlterarStatus(int id, StatusChamado novoStatus)
        {
            GarantirPermissaoDeEscrita();

            var chamado = BuscarPorId(id);
            if (chamado == null)
                throw new ArgumentException("Chamado não encontrado.");

            _chamadoRepository.AlterarStatus(id, novoStatus);
        }

        public void Excluir(int id)
        {
            GarantirPermissaoAdmin();
            _chamadoRepository.Excluir(id);
        }
    }
}

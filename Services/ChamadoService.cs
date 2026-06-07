using GestaoChamados.Models;

namespace GestaoChamados.Services
{
    public class ChamadoService
    {
        private List<Chamado> chamados = new List<Chamado>();
        private int proximoId = 1;

        public void Abrir(Cliente cliente, Atendente atendente, string descricao)
        {
            if (cliente == null)
                throw new ArgumentException("Cliente não pode ser nulo.");

            if (atendente == null)
                throw new ArgumentException("Atendente não pode ser nulo.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição do chamado não pode ser vazia.");

            var chamado = new Chamado(
                proximoId++,
                cliente,
                atendente,
                descricao,
                DateTime.Now,
                StatusChamado.Aberto
            );

            chamados.Add(chamado);
        }

        public List<Chamado> Listar()
        {
            return chamados.ToList();
        }

        public List<Chamado> ListarAbertos()
        {
            return chamados.Where(c => c.Status == StatusChamado.Aberto).ToList();
        }

        public Chamado BuscarPorId(int id)
        {
            return chamados.FirstOrDefault(c => c.Id == id);
        }

        public void AlterarStatus(int id, StatusChamado novoStatus)
        {
            var chamado = BuscarPorId(id);
            if (chamado == null)
                throw new ArgumentException("Chamado não encontrado.");

            chamado.Status = novoStatus;
        }
    }
}

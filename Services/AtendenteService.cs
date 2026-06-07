using GestaoChamados.Models;

namespace GestaoChamados.Services
{
    public class AtendenteService
    {
        private static List<Atendente> atendentes = new List<Atendente>();
        private static int proximoId = 1;

        public void Cadastrar(string nome, string setor)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do atendente não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(setor))
                throw new ArgumentException("Setor do atendente não pode ser vazio.");

            var atendente = new Atendente(proximoId++, nome, setor);
            atendentes.Add(atendente);
        }

        public List<Atendente> Listar()
        {
            return atendentes.ToList();
        }

        public Atendente BuscarPorId(int id)
        {
            return atendentes.FirstOrDefault(a => a.Id == id);
        }
    }
}

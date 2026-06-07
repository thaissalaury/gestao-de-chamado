using GestaoChamados.Models;

namespace GestaoChamados.Services
{
    public class ClienteService
    {
        private static List<Cliente> clientes = new List<Cliente>();
        private static int proximoId = 1;

        public void Cadastrar(string nome, string contato)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do cliente não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(contato))
                throw new ArgumentException("Contato do cliente não pode ser vazio.");

            var cliente = new Cliente(proximoId++, nome, contato);
            clientes.Add(cliente);
        }

        public List<Cliente> Listar()
        {
            return clientes.ToList();
        }

        public Cliente BuscarPorId(int id)
        {
            return clientes.FirstOrDefault(c => c.Id == id);
        }
    }
}

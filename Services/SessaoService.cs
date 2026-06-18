using GestaoChamados.Models;

namespace GestaoChamados.Services
{
    public static class SessaoService
    {
        public static Usuario? UsuarioLogado { get; private set; }

        public static void Login(Usuario usuario)
        {
            UsuarioLogado = usuario;
        }

        public static void Logout()
        {
            UsuarioLogado = null;
        }

        public static bool EstaAutenticado() => UsuarioLogado != null;

        public static bool PodeEscrever
        {
            get
            {
                if (UsuarioLogado == null) return false;
                // Admin (1) e Operador (2) podem escrever. Visualizador (3) não.
                return UsuarioLogado.PapelId == 1 || UsuarioLogado.PapelId == 2;
            }
        }

        public static bool EhAdmin
        {
            get
            {
                return UsuarioLogado != null && UsuarioLogado.PapelId == 1;
            }
        }
    }
}

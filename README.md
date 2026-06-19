# Gestão de Chamados

Sistema de gestão de chamados de suporte técnico com controle de acesso por papéis (RBAC) e persistência de dados em banco de dados SQLite.

---

## Sobre o projeto

O projeto consiste em um sistema de suporte técnico (helpdesk) voltado para organizar, rastrear e gerenciar a abertura e o acompanhamento de chamados. A aplicação conecta clientes (que reportam os problemas) aos atendentes (técnicos responsáveis pela resolução), permitindo o monitoramento do status de cada ticket em tempo real através de um painel de controle (Dashboard).

**Domínio:** Gestão de Chamados  
**Dupla:** Thaissa e [Nome da Dupla]

---

## Funcionalidades

- [x] Cadastro, listagem, edição e exclusão de chamados (entidade principal)
- [x] Cadastro, listagem, edição e exclusão de clientes e atendentes
- [x] Cadastro, listagem, edição e exclusão de usuários do sistema
- [x] Login com autenticação de usuários
- [x] Controle de acesso baseado em papéis para segurança das operações
- [x] Senhas protegidas com hashing BCrypt (BCrypt.Net-Next)
- [x] Persistência de dados em banco de dados SQLite local
- [x] Dashboard dinâmico com contadores em tempo real para tomada de decisões

---

## Papéis de acesso

| Papel | O que pode fazer |
|-------|-----------------|
| **Visualizador** | Apenas visualiza a Central de Chamados (listagem) e o Dashboard principal. Não pode abrir chamados, alterar status ou gerenciar outros cadastros. |
| **Operador** | Permissão total na gestão de chamados (abrir, atualizar status e excluir), além de permissão para cadastrar, listar e editar clientes e atendentes. |
| **Admin** | Permissão irrestrita ao sistema, incluindo todas as ações de Operador e acesso exclusivo à tela de Gestão de Usuários (cadastro de operadores/visualizadores, edição de credenciais e papéis). |

---

## Tecnologias usadas

- **Linguagem:** C# (.NET 10)
- **Interface:** Windows Forms
- **Banco de dados:** SQLite (`Microsoft.Data.Sqlite`)
- **Segurança:** BCrypt.Net-Next (criptografia e hashing seguro de senhas)

---

## Como rodar o projeto

### Pré-requisitos

- Visual Studio 2022 (ou superior) com a carga de trabalho **"Desenvolvimento para Desktop com .NET"** instalada
- .NET 10 SDK (ou compatível)

### Passos

1. Clone o repositório:
   ```bash
   git clone https://github.com/thaissalaury/gestao-de-chamado.git
   ```
2. Abra o arquivo `gestao de chamado.slnx` no Visual Studio.
3. Restaure os pacotes NuGet (realizado automaticamente pelo Visual Studio ao carregar a solução).
4. Pressione **F5** para executar o projeto (ou execute `dotnet run` a partir da raiz do projeto no terminal).

> O banco de dados SQLite `gestao_chamados.db` é criado e estruturado automaticamente na primeira execução do sistema, já populando as tabelas e inserindo os papéis e o usuário administrador padrão.

---

## Acesso inicial

Na primeira execução do sistema, o banco de dados é inicializado com um usuário administrador padrão:

- **Login:** `admin`
- **Senha:** `admin123`

> [!IMPORTANT]
> Recomenda-se criar novos usuários ou alterar a senha padrão do administrador logo após o primeiro acesso para garantir a segurança da aplicação.

---

## Estrutura do projeto

```text
gestao de chamado/
├── Models/        → classes de domínio (Atendente, Cliente, Chamado, Papel, StatusChamado, Usuario)
├── Data/          → conexão e inicialização automática do banco SQLite (ConexaoBanco)
├── Repositories/  → persistência e consultas SQL (AtendenteRepository, ChamadoRepository, ClienteRepository, UsuarioRepository)
├── Services/      → lógica de negócios, controle de autenticação e sessão (AtendenteService, AutenticacaoService, ChamadoService, ClienteService, SessaoService, UsuarioService)
├── Forms/         → telas de interface do usuário (FrmPrincipal, FrmLogin, FrmChamados, FrmClientes, FrmAtendentes, FrmUsuarios)
└── Program.cs     → ponto de entrada principal da aplicação (inicialização da UI)
```

---

## Instalação (versão .exe)

Se você gerou o executável de instalação com Inno Setup ou outra ferramenta de empacotamento:

1. Baixe o instalador `[NomeDoInstalador].exe` disponibilizado na seção de Releases.
2. Execute o instalador no seu computador e siga as etapas do assistente.
3. Inicie o sistema a partir do atalho criado no menu iniciar ou Área de Trabalho.

---

## Autores

- Thaissa — [GitHub](https://github.com/thaissalaury)
- [Nome da Dupla] — [GitHub ou contato]

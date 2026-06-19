# Guia Técnico: Sistema de Gestão de Chamados

Este guia foi de autoria técnica para fornecer uma visão clara, estruturada e detalhada do projeto **Gestão de Chamados** para fins de apresentação acadêmica ou profissional. Ele resume toda a lógica do código em tópicos compreensíveis e visuais.

---

## 1. Estrutura e Arquitetura do Projeto

O sistema foi desenvolvido utilizando a arquitetura em camadas no padrão **Windows Forms (.NET 10)**. A separação de responsabilidades garante que o código seja limpo, modular e fácil de dar manutenção.

```
gestao de chamado/
├── Models/        → classes de domínio (Atendente, Cliente, Chamado, Papel, StatusChamado, Usuario)
├── Data/          → conexão e inicialização automática do banco SQLite (ConexaoBanco)
├── Repositories/  → persistência e consultas SQL (AtendenteRepository, ChamadoRepository, ClienteRepository, UsuarioRepository)
├── Services/      → lógica de negócios, controle de autenticação e sessão (AtendenteService, AutenticacaoService, ChamadoService, ClienteService, SessaoService, UsuarioService)
├── Forms/         → telas de interface do usuário (FrmPrincipal, FrmLogin, FrmChamados, FrmClientes, FrmAtendentes, FrmUsuarios)
└── Program.cs     → ponto de entrada principal da aplicação (inicialização da UI)
```

### Detalhes das Camadas:

1. **`Models/` (Classes de Domínio)**:
   Representam as entidades de dados do sistema (POCO):
   * `Cliente`: Nome e contato.
   * `Atendente`: Nome e setor.
   * `Chamado`: Guarda o vínculo entre cliente, atendente, descrição, data de abertura e status (`Aberto`, `EmAndamento`, `Resolvido`).
   * `Usuario` e `Papel`: Controlam credenciais e nível de acesso.

2. **`Data/` (Conexão)**:
   * A classe `ConexaoBanco.cs` cria automaticamente o arquivo de banco de dados SQLite (`gestao_chamados.db`) e todas as tabelas caso o banco ainda não exista na máquina quando o programa rodar. Ela também insere o usuário padrão `admin` com a senha hash.

3. **`Repositories/` (Camada de Dados)**:
   * Contém classes dedicadas à comunicação direta com o banco SQLite. Elas executam instruções SQL tradicionais (`INSERT`, `SELECT`, `DELETE`, `UPDATE`) usando o `Microsoft.Data.Sqlite`. Isso separa totalmente o código de interface das consultas do banco.

4. **`Services/` (Lógica de Negócios e Regras)**:
   * Contêm a inteligência do sistema. Fazem validações (como impedir campos vazios) e gerenciam as sessões. A classe `AutenticacaoService` faz a verificação segura comparando a senha informada com o hash salvo no banco.

5. **`Forms/` (Interface Gráfica)**:
   * Telas interativas projetadas via código com um design visual contemporâneo (uso de FlatStyle, cores hexadecimais como Slate Blue, cinzas suaves e efeitos visuais como formatação semafórica de cores para os status dos chamados).

---

## 2. Banco de Dados SQLite

Substituindo a persistência em arquivos simples de texto (JSON), o sistema implementa um banco de dados relacional local **SQLite**.

### Tabelas Criadas:
* `papeis`: Armazena os níveis de permissão (`Admin`, `Operador`, `Visualizador`).
* `usuarios`: Armazena o cadastro de acessos do sistema, apontando para um papel.
* `clientes`: Cadastro de clientes.
* `atendentes`: Cadastro de atendentes técnicos.
* `chamados`: Tabela principal que gerencia os chamados conectando as chaves estrangeiras de `clientes` e `atendentes`.

---

## 3. Segurança e Controle de Acesso (RBAC)

O sistema implementa segurança ativa em dois pilares:

### Hashing de Senhas com BCrypt:
Para garantir que as senhas estejam protegidas mesmo se o banco de dados for exposto, foi integrada a biblioteca **BCrypt.Net-Next**. As senhas passam por um algoritmo de hash de via única com geração dinâmica de sal (*salt*), tornando-as impossíveis de serem decodificadas de forma reversa.

### Controle de Acesso Baseado em Papéis (RBAC):
Modifica as permissões da interface do Windows Forms dependendo do usuário que fez o login:
* **Visualizador (Papel ID 3)**: Apenas monitora e lista chamados e vê o Dashboard. Botões de escrita e menus de configuração de clientes, atendentes e usuários ficam desabilitados.
* **Operador (Papel ID 2)**: Pode abrir chamados, atualizar o status, gerenciar clientes e técnicos, mas não gerencia outros usuários.
* **Admin (Papel ID 1)**: Possui privilégio completo, incluindo acesso exclusivo à tela de Gestão de Usuários (cadastros e trocas de papéis).

---

## 4. Sugestão de Roteiro para Apresentação (5-10 minutos)

1. **Introdução (1 min)**:
   * Fale sobre a finalidade do software: centralizar chamados de suporte, substituindo planilhas ou cadernos físicos por um sistema com banco de dados local.

2. **Arquitetura (2 min)**:
   * Explique brevemente o padrão em camadas e como a separação entre UI (Forms) e persistência (Repositories) torna o projeto escalável.

3. **Demonstração do Login e Restrições (3 min)**:
   * Execute o programa e entre como um usuário **Visualizador** (crie um na tela de admin antes se desejar). Mostre que o menu de clientes/atendentes fica bloqueado para ele.
   * Faça logout e entre como **Admin** (`admin`/`admin123`). Mostre o dashboard atualizando os totais dinamicamente e acesse a tela de controle de usuários.

4. **Operação e Interface (2 min)**:
   * Faça a abertura de um chamado, selecionando o cliente e o técnico.
   * Altere o status do chamado e ressalte a mudança visual das cores semafóricas (Vermelho para Aberto, Laranja para Em Andamento e Verde para Resolvido) no grid principal.

5. **Segurança (1 min)**:
   * Destaque o uso do **BCrypt** como melhor prática moderna de segurança para criptografia de senhas.

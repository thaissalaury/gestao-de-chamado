# RELATÓRIO DE ENGENHARIA REVERSA: Sistema de Gestão de Chamados

Este documento apresenta a análise técnica detalhada do sistema de Gestão de Chamados, abrangendo desde a sua arquitetura até a implementação detalhada de cada componente.

---

## 1. Visão Geral do Projeto

### Objetivo Principal
O sistema tem como objetivo automatizar e organizar a abertura e o acompanhamento de chamados técnicos (tickets), conectando clientes que possuem problemas a atendentes responsáveis por resolvê-los.

### Problema que Resolve
Substitui o controle manual ou informal de solicitações de suporte, permitindo:
- Registro centralizado de clientes e atendentes.
- Rastreamento do status de cada chamado (Aberto, Em Andamento, Resolvido).
- Visão geral rápida da carga de trabalho através de um Dashboard.

### Público-alvo
Departamentos de TI, equipes de suporte técnico ou pequenas empresas de assistência técnica.

### Principais Funcionalidades
- **Gestão de Clientes:** Cadastro e listagem de clientes.
- **Gestão de Atendentes:** Cadastro e listagem de profissionais de suporte.
- **Controle de Chamados:** Abertura de chamados vinculando cliente e atendente, alteração de status e filtragem por descrição/cliente.
- **Dashboard:** Painel com contadores em tempo real de clientes, atendentes e status de chamados.

---

## 2. Arquitetura da Solução

### Estrutura de Pastas e Arquivos
O projeto segue uma organização baseada em responsabilidades (separação de preocupações):

```text
/ (Raiz)
├── Program.cs                # Ponto de entrada da aplicação
├── gestao de chamado.csproj   # Configurações do projeto .NET
├── Models/                   # Definições de Entidades (POCOs)
│   ├── Atendente.cs
│   ├── Cliente.cs
│   ├── Chamado.cs
│   └── StatusChamado.cs
├── Services/                  # Camada de Lógica de Negócio e Persistência
│   ├── AtendenteService.cs
│   ├── ClienteService.cs
│   ├── ChamadoService.cs
│   └── DataPersistence.cs
└── Forms/                    # Camada de Interface do Usuário (UI)
    ├── FrmPrincipal.cs       # Menu e Dashboard
    ├── FrmAtendentes.cs      # Tela de Atendentes
    ├── FrmClientes.cs        # Tela de Clientes
    └── FrmChamados.cs        # Tela de Chamados
```

### Relação entre Componentes
A aplicação utiliza um fluxo unidirecional de dependências:
**UI (Forms)** $\rightarrow$ **Services** $\rightarrow$ **Models** $\rightarrow$ **Persistence (JSON)**

### Fluxo Geral de Execução
1. O `Program.cs` inicia a aplicação e abre o `FrmPrincipal`.
2. O `FrmPrincipal` carrega as estatísticas do Dashboard via `DataPersistence`.
3. O usuário navega para as telas específicas (`FrmClientes`, `FrmAtendentes`, `FrmChamados`).
4. As telas interagem com as classes de `Service`, que processam a lógica e salvam os dados via `DataPersistence`.

---

## 3. Tecnologias Utilizadas

| Tecnologia | Versão/Tipo | Finalidade |
| :--- | :--- | :--- |
| **C#** | 13.0 / .NET 10 | Linguagem principal de desenvolvimento. |
| **WinForms** | .NET 10 Windows | Framework para criação da interface gráfica. |
| **System.Text.Json** | Nativo .NET | Serialização e desserialização de objetos para arquivos JSON. |
| **JSON** | Formato de Arquivo | Armazenamento persistente dos dados (simulando um banco de dados). |
| **PowerShell** | Shell | Utilizado para automação de build e execução via CLI. |

---

## 4. Análise Detalhada do Código

### A. Camada de Modelos (Models)

As classes de modelo são simples (POCO - Plain Old CLR Objects) e servem para transportar dados.

#### **Classe `Cliente`**
- **Responsabilidade:** Representar os dados de um cliente.
- **Atributos:** `Id` (int), `Nome` (string), `Contato` (string).

#### **Classe `Atendente`**
- **Responsabilidade:** Representar os dados de um funcionário de suporte.
- **Atributos:** `Id` (int), `Nome` (string), `Setor` (string).

#### **Classe `Chamado`**
- **Responsabilidade:** Registrar a solicitação de suporte.
- **Atributos:** `Id` (int), `Cliente` (Objeto Cliente), `Atendente` (Objeto Atendente), `Descricao` (string), `DataAbertura` (DateTime), `Status` (Enum StatusChamado).
- **Relação:** Possui dependência direta de `Cliente` e `Atendente`.

#### **Enum `StatusChamado`**
- Define os estados possíveis de um ticket: `Aberto` (1), `EmAndamento` (2), `Resolvido` (3).

---

### B. Camada de Serviços (Services)

#### **Classe `DataPersistence` (Estática)**
É o "motor" de banco de dados do sistema.
- **`Save<T>(fileName, data)`**: Converte a lista de objetos para JSON e escreve no arquivo.
- **`Load<T>(fileName)`**: Lê o arquivo JSON e converte de volta para uma lista de objetos.

#### **Classe `AtendenteService` / `ClienteService`**
Ambas seguem o mesmo padrão de CRUD básico:
- **`Cadastrar(...)`**: Valida se os campos não estão vazios, gera um ID incremental e salva no JSON.
- **`Listar()`**: Retorna a lista completa de registros.
- **`BuscarPorId(id)`**: Localiza um registro específico via LINQ (`FirstOrDefault`).

#### **Classe `ChamadoService`**
A classe mais complexa da camada de negócio.
- **`Abrir(cliente, atendente, descricao)`**: Valida a existência dos objetos e cria um novo chamado com status `Aberto`.
- **`AlterarStatus(id, novoStatus)`**: Localiza o chamado pelo ID e atualiza seu estado.
- **`ListarAbertos()`**: Filtra apenas chamados que não foram resolvidos.

---

### C. Camada de Interface (Forms)

As telas utilizam componentes dinâmicos criados via código (não via Designer), com foco em um design moderno (Cores Hex, Padding, FlatStyle).

- **`FrmPrincipal`**: Gerencia a navegação e o Dashboard. Possui a função `AtualizarDashboard()` que reconta os itens nos arquivos JSON a cada abertura de tela.
- **`FrmAtendentes` / `FrmClientes`**: Telas de cadastro com `DataGridView` para exibição e campos de texto para entrada.
- **`FrmChamados`**: A tela mais rica, com:
    - **Abertura de Chamados:** Comboboxes vinculados aos dados de Clientes e Atendentes.
    - **Gerenciamento:** Busca em tempo real via `TextChanged` e atualização de status via Enum.
    - **Formatação Condicional:** O `DgvChamados_CellFormatting` altera a cor da célula de status (Vermelho para Aberto, Laranja para Andamento, Verde para Resolvido).

---

## 5. Fluxo de Funcionamento

### Diagrama de Execução
```text
Program.cs (Main)
    ↓
FrmPrincipal (Dashboard)
    ↓
[ Escolha do Menu ]
    ↓
FrmChamados / FrmClientes / FrmAtendentes
    ↓
ChamadoService / ClienteService / AtendenteService
    ↓
DataPersistence (Save/Load)
    ↓
Arquivos JSON (clientes.json, atendentes.json, chamados.json)
```

### Detalhamento do Passo a Passo
1. **Inicialização:** O sistema inicia no `Program.cs`, que instancia o `FrmPrincipal`.
2. **Carga de Dados:** O Dashboard do `FrmPrincipal` chama o `DataPersistence` para ler os arquivos JSON e contar quantos registros existem de cada tipo.
3. **Ação do Usuário:** O usuário clica em "Clientes" $\rightarrow$ Abre `FrmClientes`.
4. **Processamento:** O usuário digita o nome e clica em "Adicionar". O `FrmClientes` chama `ClienteService.Cadastrar()`.
5. **Persistência:** O `ClienteService` chama `DataPersistence.Save()`, que sobrescreve o arquivo `clientes.json` com a nova lista.
6. **Atualização:** O `FrmClientes` chama `CarregarClientes()`, que recarrega a grade (`DataGridView`) a partir do arquivo atualizado.

---

## 6. Regras de Negócio Identificadas

| Regra | Implementação | Funcionamento | Exemplo |
| :--- | :--- | :--- | :--- |
| **Validação de Entrada** | `Services/*.cs` | Impede cadastros com campos nulos ou vazios via `ArgumentException`. | Tentar cadastrar cliente sem nome gera erro. |
| **ID Incremental** | `Services/*.cs` | Calcula o próximo ID baseado no maior ID existente no JSON (`Max(id) + 1`). | Se o último cliente é ID 5, o próximo será 6. |
| **Vínculo Obrigatório** | `ChamadoService.cs` | Um chamado não pode ser aberto sem um Cliente e um Atendente válidos. | Erro se `cliente == null` ao abrir chamado. |
| **Estado do Chamado** | `ChamadoService.cs` | Um chamado inicia obrigatoriamente como `Aberto`. | Novo chamado $\rightarrow$ Status = `Aberto`. |

---

## 7. Análise dos Scripts PowerShell
Não foram encontrados arquivos `.ps1` no repositório. No entanto, o projeto é compatível com a execução via PowerShell utilizando o comando `dotnet run` ou `dotnet build`, que gerencia as dependências do .NET SDK.

---

## 8. Banco de Dados (Persistência)
O sistema utiliza **Persistência em Arquivos Flat (JSON)** em vez de um banco de dados SQL.

**Entidades Armazenadas:**
- `clientes.json`: Lista de objetos `Cliente`.
- `atendentes.json`: Lista de objetos `Atendente`.
- `chamados.json`: Lista de objetos `Chamado`.

**Relacionamento:** 
O relacionamento é implementado via **Composição de Objetos** no JSON. Dentro do arquivo `chamados.json`, cada chamado contém os objetos completos de `Cliente` e `Atendente` aninhados.

---

## 9. APIs e Integrações
O sistema é uma aplicação **Standalone (Desktop)**. Não consome APIs externas nem expõe endpoints. Toda a comunicação de dados ocorre localmente entre a memória da aplicação e o sistema de arquivos do Windows.

---

## 10. Segurança
- **Autenticação:** Inexistente. Qualquer pessoa com acesso ao executável pode gerenciar os dados.
- **Autorização:** Inexistente. Não há níveis de acesso (Admin vs Usuário).
- **Criptografia:** Inexistente. Os dados são salvos em texto simples (JSON), podendo ser lidos ou editados manualmente por qualquer usuário no diretório `bin/Debug`.
- **Vulnerabilidades:** A principal vulnerabilidade é a exposição dos dados em arquivos JSON abertos e a falta de validação de permissões.

---

## 11. Resumo para Apresentação

**O que é?**
Um sistema de gestão de tickets de suporte desenvolvido em C# com Windows Forms.

**Como funciona?**
A aplicação permite cadastrar clientes e técnicos, e gerenciar a vida útil de um chamado técnico, desde a abertura até a resolução, salvando todas as informações em arquivos JSON para garantir que os dados não sejam perdidos ao fechar o programa.

**Tecnologias:**
.NET 10, C#, WinForms e System.Text.Json.

**Problemas que resolve:**
Elimina a desorganização de pedidos de suporte, permitindo que a empresa saiba exatamente quantos chamados estão abertos e quem é o técnico responsável por cada um.

**Diferenciais:**
Interface moderna com cores customizadas, dashboard de estatísticas em tempo real e sistema de formatação visual de status (cores semafóricas).

---

## 12. Explicação Didática: Como Explicar Este Projeto

Para ensinar este projeto a um iniciante, use a **Analogia do Caderno de Recados**:

> *"Imagine que você tem um caderno onde anota quem são seus clientes e quem são seus funcionários. Quando um cliente liga com um problema, você abre uma página nova (o Chamado), escreve o nome do cliente, escolhe qual funcionário vai resolver e descreve o problema. No início, você coloca um post-it vermelho escrito 'ABERTO'. Quando o funcionário começa a trabalhar, você troca por um amarelo 'EM ANDAMENTO' e, ao final, um verde 'RESOLVIDO'. Este software faz exatamente isso, mas em vez de papel, ele usa arquivos de texto (JSON) no computador."*

**Pontos chave para explicar o código:**
1. **Models:** São as "folhas do caderno" (definem o que deve ser anotado).
2. **Services:** São as "regras de preenchimento" (não pode deixar nome em branco, o ID deve ser sequencial).
3. **Forms:** São a "capa e as páginas" que o usuário vê e interage.
4. **DataPersistence:** É o "armário" onde o caderno é guardado e retirado.

---

## 13. Análise de Profundidade e Melhorias

### Padrões de Projeto Utilizados
- **Layered Architecture (Arquitetura em Camadas):** Separação clara entre UI, Negócio e Dados.
- **Singleton-like (Static Services):** O uso de classes estáticas para persistência e serviços simplifica o acesso aos dados em aplicações pequenas.
- **Data Transfer Object (DTO):** As classes em `Models` atuam como DTOs para a serialização JSON.

### Sugestões de Refatoração e Melhorias
1. **Migração para Banco de Dados:** Substituir arquivos JSON por **SQLite** ou **SQL Server** via Entity Framework Core para suportar mais dados e garantir a integridade referencial.
2. **Implementação de Autenticação:** Adicionar uma tela de Login para proteger os dados.
3. **Injeção de Dependência:** Substituir as instâncias manuais de serviços (`new ClienteService()`) por um container de DI para facilitar testes unitários.
4. **Validações Avançadas:** Implementar validação de e-mail e telefone nos modelos de Cliente.
5. **Paginação:** Se a lista de chamados crescer muito, o `DataGridView` poderá ficar lento. Implementar paginação ou carregamento sob demanda.

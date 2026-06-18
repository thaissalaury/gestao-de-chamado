using System;
using Microsoft.Data.Sqlite;
using System.IO;

namespace GestaoChamados.Data
{
    public static class ConexaoBanco
    {
        private const string DbName = "gestao_chamados.db";
        public static string ConnectionString => $"Data Source={DbName}";

        public static void InicializarBanco()
        {
            if (File.Exists(DbName)) return;

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                // Tabela de Papéis
                command.CommandText = @"
                    CREATE TABLE papeis (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        nome TEXT NOT NULL UNIQUE
                    );";
                command.ExecuteNonQuery();

                // Tabela de Usuários
                command.CommandText = @"
                    CREATE TABLE usuarios (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        nome TEXT NOT NULL,
                        login TEXT NOT NULL UNIQUE,
                        senha_hash TEXT NOT NULL,
                        papel_id INTEGER NOT NULL,
                        FOREIGN KEY (papel_id) REFERENCES papeis (id)
                    );";
                command.ExecuteNonQuery();

                // Tabela de Clientes
                command.CommandText = @"
                    CREATE TABLE clientes (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        nome TEXT NOT NULL,
                        contato TEXT
                    );";
                command.ExecuteNonQuery();

                // Tabela de Atendentes
                command.CommandText = @"
                    CREATE TABLE atendentes (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        nome TEXT NOT NULL,
                        setor TEXT
                    );";
                command.ExecuteNonQuery();

                // Tabela de Chamados
                command.CommandText = @"
                    CREATE TABLE chamados (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        cliente_id INTEGER NOT NULL,
                        atendente_id INTEGER NOT NULL,
                        descricao TEXT NOT NULL,
                        data_abertura TEXT NOT NULL,
                        status TEXT NOT NULL,
                        FOREIGN KEY (cliente_id) REFERENCES clientes (id),
                        FOREIGN KEY (atendente_id) REFERENCES atendentes (id)
                    );";
                command.ExecuteNonQuery();

                // Inserir Papéis básicos
                command.CommandText = "INSERT INTO papeis (nome) VALUES ('Admin'), ('Operador'), ('Visualizador');";
                command.ExecuteNonQuery();

                // Criar usuário admin inicial (senha: admin123)
                string senhaHash = BCrypt.Net.BCrypt.HashPassword("admin123");
                command.CommandText = "INSERT INTO usuarios (nome, login, senha_hash, papel_id) VALUES ('Administrador', 'admin', '" + senhaHash + "', 1);";
                command.ExecuteNonQuery();
            }
        }
    }
}

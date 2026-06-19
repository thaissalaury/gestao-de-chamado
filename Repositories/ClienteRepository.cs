using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using GestaoChamados.Models;
using GestaoChamados.Data;

namespace GestaoChamados.Repositories
{
    public class ClienteRepository
    {
        public List<Cliente> Listar()
        {
            var clientes = new List<Cliente>();
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, nome, contato FROM clientes";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clientes.Add(new Cliente(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                        ));
                    }
                }
            }
            return clientes;
        }

        public void Excluir(int id)
        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM clientes WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public Cliente? BuscarPorId(int id)

        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, nome, contato FROM clientes WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Cliente(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                        );
                    }
                }
            }
            return null;
        }

        public void Cadastrar(Cliente cliente)
        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO clientes (nome, contato) VALUES (@nome, @contato)";
                command.Parameters.AddWithValue("@nome", cliente.Nome);
                command.Parameters.AddWithValue("@contato", cliente.Contato);
                command.ExecuteNonQuery();
            }
        }
    }
}

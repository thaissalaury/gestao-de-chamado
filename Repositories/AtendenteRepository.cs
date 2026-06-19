using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using GestaoChamados.Models;
using GestaoChamados.Data;

namespace GestaoChamados.Repositories
{
    public class AtendenteRepository
    {
        public List<Atendente> Listar()
        {
            var atendentes = new List<Atendente>();
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, nome, setor FROM atendentes";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        atendentes.Add(new Atendente(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                        ));
                    }
                }
            }
            return atendentes;
        }

        public void Excluir(int id)
        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM atendentes WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public Atendente? BuscarPorId(int id)

        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, nome, setor FROM atendentes WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Atendente(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                        );
                    }
                }
            }
            return null;
        }

        public void Cadastrar(Atendente atendente)
        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO atendentes (nome, setor) VALUES (@nome, @setor)";
                command.Parameters.AddWithValue("@nome", atendente.Nome);
                command.Parameters.AddWithValue("@setor", atendente.Setor);
                command.ExecuteNonQuery();
            }
        }
    }
}

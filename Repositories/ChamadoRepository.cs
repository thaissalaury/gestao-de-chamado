using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using GestaoChamados.Models;
using GestaoChamados.Data;

namespace GestaoChamados.Repositories
{
    public class ChamadoRepository
    {
        public List<Chamado> Listar()
        {
            var chamados = new List<Chamado>();
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT c.id, c.descricao, c.data_abertura, c.status,
                           cl.id, cl.nome, cl.contato,
                           at.id, at.nome, at.setor
                    FROM chamados c
                    JOIN clientes cl ON c.cliente_id = cl.id
                    JOIN atendentes at ON c.atendente_id = at.id";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cliente = new Cliente(reader.GetInt32(4), reader.GetString(5), reader.IsDBNull(6) ? string.Empty : reader.GetString(6));
                        var atendente = new Atendente(reader.GetInt32(7), reader.GetString(8), reader.IsDBNull(9) ? string.Empty : reader.GetString(9));

                        chamados.Add(new Chamado(
                            reader.GetInt32(0),
                            cliente,
                            atendente,
                            reader.GetString(1),
                            DateTime.Parse(reader.GetString(2)),
                            Enum.Parse<StatusChamado>(reader.GetString(3))
                        ));
                    }
                }
            }
            return chamados;
        }

        public Chamado? BuscarPorId(int id)
        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT c.id, c.descricao, c.data_abertura, c.status,
                           cl.id, cl.nome, cl.contato,
                           at.id, at.nome, at.setor
                    FROM chamados c
                    JOIN clientes cl ON c.cliente_id = cl.id
                    JOIN atendentes at ON c.atendente_id = at.id
                    WHERE c.id = @id";
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var cliente = new Cliente(reader.GetInt32(4), reader.GetString(5), reader.IsDBNull(6) ? string.Empty : reader.GetString(6));
                        var atendente = new Atendente(reader.GetInt32(7), reader.GetString(8), reader.IsDBNull(9) ? string.Empty : reader.GetString(9));

                        return new Chamado(
                            reader.GetInt32(0),
                            cliente,
                            atendente,
                            reader.GetString(1),
                            DateTime.Parse(reader.GetString(2)),
                            Enum.Parse<StatusChamado>(reader.GetString(3))
                        );
                    }
                }
            }
            return null;
        }

        public void Cadastrar(Chamado chamado)
        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO chamados (cliente_id, atendente_id, descricao, data_abertura, status) VALUES (@clienteId, @atendenteId, @descricao, @data, @status)";
                command.Parameters.AddWithValue("@clienteId", chamado.Cliente.Id);
                command.Parameters.AddWithValue("@atendenteId", chamado.Atendente.Id);
                command.Parameters.AddWithValue("@descricao", chamado.Descricao);
                command.Parameters.AddWithValue("@data", chamado.DataAbertura.ToString("o")); // ISO 8601
                command.Parameters.AddWithValue("@status", chamado.Status.ToString());
                command.ExecuteNonQuery();
            }
        }

        public void AlterarStatus(int id, StatusChamado novoStatus)
        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE chamados SET status = @status WHERE id = @id";
                command.Parameters.AddWithValue("@status", novoStatus.ToString());
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public int ContarPorCliente(int clienteId)
        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM chamados WHERE cliente_id = @id";
                command.Parameters.AddWithValue("@id", clienteId);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public int ContarPorAtendente(int atendenteId)
        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM chamados WHERE atendente_id = @id";
                command.Parameters.AddWithValue("@id", atendenteId);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using GestaoChamados.Models;
using GestaoChamados.Data;

namespace GestaoChamados.Repositories
{
    public class UsuarioRepository
    {
        public List<Usuario> Listar()
        {
            var usuarios = new List<Usuario>();
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, nome, login, senha_hash, papel_id FROM usuarios";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetInt32(4)
                        ));
                    }
                }
            }
            return usuarios;
        }

        public void Excluir(int id)
        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM usuarios WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public Usuario? BuscarPorLogin(string login)

        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, nome, login, senha_hash, papel_id FROM usuarios WHERE login = @login";
                command.Parameters.AddWithValue("@login", login);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuario(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetInt32(4)
                        );
                    }
                }
            }
            return null;
        }

        public void Cadastrar(Usuario usuario)
        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO usuarios (nome, login, senha_hash, papel_id) VALUES (@nome, @login, @senha, @papelId)";
                command.Parameters.AddWithValue("@nome", usuario.Nome);
                command.Parameters.AddWithValue("@login", usuario.Login);
                command.Parameters.AddWithValue("@senha", usuario.SenhaHash);
                command.Parameters.AddWithValue("@papelId", usuario.PapelId);
                command.ExecuteNonQuery();
            }
        }

        public Usuario? BuscarPorId(int id)
        {
            using (var connection = new SqliteConnection(ConexaoBanco.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, nome, login, senha_hash, papel_id FROM usuarios WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuario(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetInt32(4)
                        );
                    }
                }
            }
            return null;
        }
    }
}

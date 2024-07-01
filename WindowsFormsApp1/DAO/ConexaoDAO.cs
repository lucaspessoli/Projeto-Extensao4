using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1.DAO
{
    internal class ConexaoDAO
    {
        private NpgsqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        public ConexaoDAO()
        {
            Initialize();
        }

        private void Initialize()
        {
            server = "localhost";
            database = "extensao4";
            uid = "postgres";
            password = "unipar";
            string connectionString;
            connectionString = $"Server={server};Port=5432;Database={database};User Id={uid};Password={password};";
            connection = new NpgsqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool InserirProduto(Produto produto)
        {
            string query = "INSERT INTO produtos (nome_produto, quantidade_produto, valor_produto) " +
                           "VALUES (@NomeProduto, @QuantidadeProduto, @ValorProduto)";

            if (OpenConnection())
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@NomeProduto", produto.nomeProduto);
                    cmd.Parameters.AddWithValue("@QuantidadeProduto", produto.quantidadeProduto);
                    cmd.Parameters.AddWithValue("@ValorProduto", produto.valorProduto);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                    finally
                    {
                        CloseConnection();
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public bool DeletarProdutoPorId(int idProduto)
        {
            string query = "DELETE FROM produtos WHERE id = @IdProduto";

            if (OpenConnection())
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@IdProduto", idProduto);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                    finally
                    {
                        CloseConnection();
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public List<Produto> BuscarTodosProdutos()
        {
            List<Produto> produtos = new List<Produto>();
            string query = "SELECT id, nome_produto, quantidade_produto, valor_produto FROM produtos";

            if (OpenConnection())
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    try
                    {
                        using (NpgsqlDataReader dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                Produto produto = new Produto();
                                produto.id = Convert.ToInt32(dataReader["id"]);
                                produto.nomeProduto = dataReader["nome_produto"].ToString();
                                produto.quantidadeProduto = Convert.ToInt32(dataReader["quantidade_produto"]);
                                produto.valorProduto = Convert.ToDouble(dataReader["valor_produto"]);
                                produtos.Add(produto);
                            }
                            dataReader.Close();
                        }
                        return produtos;
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return null;
                    }
                    finally
                    {
                        CloseConnection();
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public bool LoginUsuario(string nomeUsuario, string senhaUsuario)
        {
            string query = "SELECT id FROM usuarios WHERE nome_usuario = @NomeUsuario AND senha_usuario = @SenhaUsuario";

            if (OpenConnection())
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@NomeUsuario", nomeUsuario);
                    cmd.Parameters.AddWithValue("@SenhaUsuario", senhaUsuario);

                    try
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            return true; // Login válido
                        }
                        else
                        {
                            return false; // Login inválido
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                    finally
                    {
                        CloseConnection();
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public bool CadastrarUsuario(Usuario usuario)
        {
            string query = "INSERT INTO usuarios (nome_usuario, senha_usuario) VALUES (@NomeUsuario, @SenhaUsuario)";

            if (OpenConnection())
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@NomeUsuario", usuario.nomeUsuario);
                    cmd.Parameters.AddWithValue("@SenhaUsuario", usuario.senhaUsuario);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                    finally
                    {
                        CloseConnection();
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public bool RemoverQuantidadeProduto(int idProduto, int quantidadeRemover)
        {
            string querySelect = "SELECT quantidade_produto FROM produtos WHERE id = @IdProduto";
            string queryUpdate = "UPDATE produtos SET quantidade_produto = @NovaQuantidade WHERE id = @IdProduto";

            if (OpenConnection())
            {
                NpgsqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Buscar a quantidade atual do produto
                    int quantidadeAtual = 0;
                    using (NpgsqlCommand cmdSelect = new NpgsqlCommand(querySelect, connection))
                    {
                        cmdSelect.Parameters.AddWithValue("@IdProduto", idProduto);
                        quantidadeAtual = Convert.ToInt32(cmdSelect.ExecuteScalar());
                    }

                    // Verificar se há estoque suficiente
                    if (quantidadeAtual < quantidadeRemover)
                    {
                        // Caso não haja estoque suficiente, rolar a transação de volta e retornar false
                        transaction.Rollback();
                        return false;
                    }

                    // Calcular a nova quantidade após remoção
                    int novaQuantidade = quantidadeAtual - quantidadeRemover;

                    // Atualizar o estoque do produto
                    using (NpgsqlCommand cmdUpdate = new NpgsqlCommand(queryUpdate, connection))
                    {
                        cmdUpdate.Parameters.AddWithValue("@NovaQuantidade", novaQuantidade);
                        cmdUpdate.Parameters.AddWithValue("@IdProduto", idProduto);
                        cmdUpdate.ExecuteNonQuery();
                    }

                    // Confirmar a transação
                    transaction.Commit();
                    return true;
                }
                catch (NpgsqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback(); // Em caso de exceção, rolar a transação de volta
                    return false;
                }
                finally
                {
                    CloseConnection();
                }
            }
            else
            {
                return false;
            }
        }

    }
}

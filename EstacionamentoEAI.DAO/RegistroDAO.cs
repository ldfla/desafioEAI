﻿using EstacionamentoEAI.DAO.Interfaces;
using EstacionamentoEAI.Definition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstacionamentoEAI.DAO
{
    public class RegistroDAO : IDAO<Registro>, IDisposable
    {
        private IConnection _conn;

        public RegistroDAO(IConnection connection)
        {
            this._conn = connection;
        }

        public bool Atualizar(Registro model)
        {
            try
            {
                using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
                {
                    //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = "UPDATE Registros SET Estacionamento = @estacionamento, Veiculo= @veiculo, DataDeEntrada = @datadeentrada," +
                                             " DataDeSaida = @datadesaida, UsuarioEntrada = @usuarioentrada, UsuarioSaida = @usuariosaida, Valor = @valor" +
                                             " WHERE Id = @id";

                    //Adiciona os parametros de Atualização. 
                    sqlCommand.Parameters.Add("@estacionamento", SqlDbType.Int).Value = model.Estacionamento.Id;
                    sqlCommand.Parameters.Add("@veiculo", SqlDbType.Int).Value = model.Veiculo.Id;
                    sqlCommand.Parameters.Add("@datadeentrada", SqlDbType.DateTime).Value = model.DataDeEntrada;
                    sqlCommand.Parameters.Add("@usuarioentrada", SqlDbType.Int).Value = model.UsuarioEntrada.Id;
                    sqlCommand.Parameters.Add("@datadesaida", SqlDbType.DateTime).Value = model.DataDeSaida;
                    sqlCommand.Parameters.Add("@usuariosaida", SqlDbType.Int).Value = model.UsuarioSaida.Id;
                    sqlCommand.Parameters.Add("@valor", SqlDbType.Decimal).Value = model.Valor;
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = model.Id;

                    //Executa a Query de update
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                //Se houver erro na atualização, retorna false
                return false;
            }

            return true;
        }

        public Registro BuscarItem(params object[] objeto)
        {

            Registro registro = null;

            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto 
                sqlCommand.CommandType = System.Data.CommandType.Text;

                string selectQuery = string.Empty;
                string placa = string.Empty;
                string action = objeto[0].ToString();

                switch (action)
                {
                    case "placa":
                    case "placaSaida":
                        placa = objeto[1].ToString();
                        selectQuery = "SELECT Registros.Id, Registros.Estacionamento, Registros.Veiculo, Registros.DataDeEntrada, Registros.DataDeSaida," +
                                        " Registros.UsuarioEntrada, Registros.UsuarioSaida, Registros.Valor, Veiculo.Id, Veiculo.Placa, Estacionamentos.CustoHora" +
                                        " FROM Registros" +
                                        " INNER JOIN Veiculo ON Registros.Veiculo = Veiculo.Id" +
                                        " INNER JOIN Estacionamentos ON Registros.Estacionamento = Estacionamentos.Id" +
                                        " WHERE (Veiculo.Placa = @placa and DataDeSaida IS NULL)";
                        //Adiciona os parametros de Busca da Placa. 
                        sqlCommand.Parameters.Add("@placa", SqlDbType.NVarChar).Value = placa;
                        break;
                    default:
                        return registro;
                }
                sqlCommand.CommandText = selectQuery;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    if (sqlDataReader.Read())
                    {
                        switch (action)
                        {
                            case "placa":
                                if (!sqlDataReader.IsDBNull(0))
                                {
                                    registro = new Registro
                                    {
                                        Id = sqlDataReader.GetInt32(0),
                                        Estacionamento = new Estacionamento { Id = sqlDataReader.GetInt32(1) },
                                        Veiculo = new Veiculo { Id = sqlDataReader.GetInt32(2), Placa = placa },
                                        DataDeEntrada = sqlDataReader.GetDateTime(3),
                                        UsuarioEntrada = new Usuario { Id = sqlDataReader.GetInt32(5) },
                                        CustoHora = sqlDataReader.GetDecimal(10)
                                    };
                                }
                                break;
                            case "placaSaida":
                                try
                                {
                                    registro = new Registro
                                    {
                                        Id = sqlDataReader.GetInt32(0),
                                        Estacionamento = new Estacionamento { Id = sqlDataReader.GetInt32(1) },
                                        Veiculo = new Veiculo { Id = sqlDataReader.GetInt32(2), Placa = placa },
                                        DataDeEntrada = sqlDataReader.GetDateTime(3),
                                        UsuarioEntrada = new Usuario { Id = sqlDataReader.GetInt32(5) },
                                        CustoHora = sqlDataReader.GetDecimal(10),
                                        DataDeSaida = sqlDataReader.GetDateTime(4),
                                        UsuarioSaida = new Usuario { Id = sqlDataReader.GetInt32(6) },
                                        Valor = sqlDataReader.GetDecimal(7),
                                    };
                                }
                                catch (SqlException)
                                {
                                    sqlDataReader.Close();
                                    return registro;
                                }
                                break;
                            default:
                                return registro;
                        }
                    }
                }
                sqlDataReader.Close();
            }
            return registro;
        }

        public Registro Inserir(Registro model)
        {
            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "INSERT into Registros (Estacionamento, Veiculo, DataDeEntrada, UsuarioEntrada)" +
                                        " VALUES (@estacionamento, @veiculo, @datadeentrada, @usuarioentrada) SELECT SCOPE_IDENTITY()";

                //Adiciona os parametros de Inserção. 
                sqlCommand.Parameters.Add("@estacionamento", SqlDbType.Int).Value = model.Estacionamento.Id;
                sqlCommand.Parameters.Add("@veiculo", SqlDbType.Int).Value = model.Veiculo.Id;
                sqlCommand.Parameters.Add("@datadeentrada", SqlDbType.DateTime).Value = model.DataDeEntrada;
                sqlCommand.Parameters.Add("@usuarioentrada", SqlDbType.Int).Value = model.UsuarioEntrada.Id;

                //Executa a Query e retorna o ID do registro adicionado
                model.Id = int.Parse(sqlCommand.ExecuteScalar().ToString());
            }
            return model;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<Registro> ListarVagasOcupadas(int estacionamentoId)
        {
            List<Registro> registros = new List<Registro>();

            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "SELECT Registros.Id, Modelos.Nome, Modelos.Id, Veiculo.Placa, Veiculo.ID, Registros.Estacionamento," +
                                         " Registros.DataDeEntrada, Registros.UsuarioEntrada, Estacionamentos.CustoHora" +
                                         " FROM Veiculo" + 
                                         " INNER JOIN Registros ON Veiculo.Id = Registros.Veiculo" + 
                                         " INNER JOIN Modelos ON Veiculo.Modelo = Modelos.Id" +
                                         " INNER JOIN Estacionamentos ON Registros.Estacionamento = Estacionamentos.Id" +
                                         " WHERE(Registros.Estacionamento = @estacionamentoId and Registros.DataDeSaida IS NULL)";

                //Adiciona os parametros de Busca. 
                sqlCommand.Parameters.Add("@estacionamentoId", SqlDbType.Int).Value = estacionamentoId;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        registros.Add(new Registro
                        {
                            Id = sqlDataReader.GetInt32(0),
                            DataDeEntrada = sqlDataReader.GetDateTime(6),
                            UsuarioEntrada = new Usuario
                            {
                                Id = sqlDataReader.GetInt32(7)
                            },
                            Veiculo = new Veiculo
                            {
                                Id = sqlDataReader.GetInt32(4),
                                Placa = sqlDataReader.GetString(3),
                                Modelo = new Modelo
                                {
                                    Id = sqlDataReader.GetInt32(2),
                                    Nome = sqlDataReader.GetString(1)
                                }
                            },
                            Estacionamento = new Estacionamento
                            {
                                Id = sqlDataReader.GetInt32(5),
                                CustoHora = sqlDataReader.GetDecimal(8)
                            }
                    });
                    }                                        
                }
                sqlDataReader.Close();
            }
            return registros;
        }

        public int ContaVagasOcupadas(int estacionamentoId)
        {
            int vagasOcupadas = 0;

            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "SELECT COUNT(Id) FROM Registros WHERE DataDeSaida IS NULL and Estacionamento = @estacionamentoId";

                //Adiciona os parametros de Busca. 
                sqlCommand.Parameters.Add("@estacionamentoId", SqlDbType.Int).Value = estacionamentoId;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    vagasOcupadas = sqlDataReader.GetInt32(0);
                }
                sqlDataReader.Close();
            }
            return vagasOcupadas;
        }

        public List<Registro> ListarItens()
        {
            throw new NotImplementedException();
        }
    }
}

using EstacionamentoEAI.DAO.Interfaces;
using EstacionamentoEAI.Definition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
            catch (SqlException)
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
                        sqlCommand.Parameters.Add("@placa", SqlDbType.NVarChar).Value = placa.Replace("-", "").ToUpper();
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
                                        Veiculo = new Veiculo { Id = sqlDataReader.GetInt32(2), Placa = placa.Replace("-", "").ToUpper() },
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
                                        Veiculo = new Veiculo { Id = sqlDataReader.GetInt32(2), Placa = placa.Replace("-", "").ToUpper() },
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

        public List<Registro> ListarItens(string placa)
        {
            List<Registro> registros = new List<Registro>();

            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "SELECT Registros.Id, Modelos.Nome AS ModeloNome, Modelos.Id AS ModeloId, Veiculo.Placa, Veiculo.Id AS VeiculoId," +
                                         " Veiculo.Observacao, Registros.Estacionamento, Registros.DataDeEntrada, Registros.UsuarioEntrada," +
                                         " Estacionamentos.CustoHora, Marca.Nome AS MarcaNome, Marca.Id AS MarcaId, Estacionamentos.Nome AS EstacionamentoNome," +
                                         " Estacionamentos.Id AS EstacionamentoId, Registros.DataDeSaida, Registros.UsuarioSaida, Registros.Valor" +
                                         " FROM Veiculo" +
                                         " INNER JOIN Registros ON Veiculo.Id = Registros.Veiculo" +
                                         " INNER JOIN Modelos ON Veiculo.Modelo = Modelos.Id" +
                                         " INNER JOIN Estacionamentos ON Registros.Estacionamento = Estacionamentos.Id" +
                                         " INNER JOIN Marca ON Modelos.Marca = Marca.id" +
                                         " WHERE (Veiculo.Placa= @placa)";

                sqlCommand.Parameters.Add("@placa", SqlDbType.NVarChar).Value = placa.Replace("-", "").ToUpper();
                try
                {
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            DateTime dataSaida = DateTime.MinValue;
                            Usuario usuarioSaida = null;
                            Decimal valor = 0;

                            //Valida dados para o relatorio
                            //Data de Saida
                            if (!sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("DataDeSaida")))
                            {
                                dataSaida = sqlDataReader.GetDateTime(14);
                            }

                            //Funcionario que registra Saida
                            if (!sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("UsuarioSaida")))
                            {
                                usuarioSaida = new Usuario { Id = sqlDataReader.GetInt32(15) };
                            }
                            else
                            {
                                usuarioSaida = new Usuario { Id = 0 };
                            }

                            //Valor
                            if (!sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("Valor")))
                            {
                                valor = sqlDataReader.GetDecimal(16);
                            }

                            registros.Add(new Registro
                            {
                                Id = sqlDataReader.GetInt32(0),
                                Veiculo = new Veiculo
                                {
                                    Id = sqlDataReader.GetInt32(4),
                                    Placa = sqlDataReader.GetString(3),
                                    Observacao = sqlDataReader.GetString(5),
                                    Modelo = new Modelo
                                    {
                                        Id = sqlDataReader.GetInt32(2),
                                        Nome = sqlDataReader.GetString(1),
                                        Marca = new Marca
                                        {
                                            Id = sqlDataReader.GetInt32(11),
                                            Nome = sqlDataReader.GetString(10)
                                        }
                                    }
                                },
                                DataDeEntrada = sqlDataReader.GetDateTime(7),
                                UsuarioEntrada = new Usuario { Id = sqlDataReader.GetInt32(8) },
                                CustoHora = sqlDataReader.GetDecimal(9),
                                Estacionamento = new Estacionamento { Id = sqlDataReader.GetInt32(13), Endereco = sqlDataReader.GetString(12) },
                                DataDeSaida = dataSaida,
                                UsuarioSaida = usuarioSaida,
                                Valor = valor
                            });
                        }
                    }
                }
                catch (SqlException)
                {
                    return registros;                    
                }
            }
            return registros;
        }

        public List<Registro> ListarItens(Estacionamento estacionamento)
        {
            List<Registro> registros = new List<Registro>();

            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "SELECT Registros.Id, Modelos.Nome AS ModeloNome, Modelos.Id AS ModeloId, Veiculo.Placa, Veiculo.Id AS VeiculoId," +
                                         " Veiculo.Observacao, Registros.Estacionamento, Registros.DataDeEntrada, Registros.UsuarioEntrada," +
                                         " Estacionamentos.CustoHora, Marca.Nome AS MarcaNome, Marca.Id AS MarcaId, Estacionamentos.Nome AS EstacionamentoNome," +
                                         " Estacionamentos.Id AS EstacionamentoId, Registros.DataDeSaida, Registros.UsuarioSaida, Registros.Valor" +
                                         " FROM Veiculo" +
                                         " INNER JOIN Registros ON Veiculo.Id = Registros.Veiculo" +
                                         " INNER JOIN Modelos ON Veiculo.Modelo = Modelos.Id" +
                                         " INNER JOIN Estacionamentos ON Registros.Estacionamento = Estacionamentos.Id" +
                                         " INNER JOIN Marca ON Modelos.Marca = Marca.id" +
                                         " WHERE (Estacionamento.ID = @estacionamentoId) ORDER BY Registros.DataDeEntrada ASC";

                sqlCommand.Parameters.Add("@estacionamentoId", SqlDbType.Int).Value = estacionamento.Id;
                try
                {
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            DateTime dataSaida = DateTime.MinValue;
                            Usuario usuarioSaida = null;
                            Decimal valor = 0;

                            //Valida dados para o relatorio
                            //Data de Saida
                            if (!sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("DataDeSaida")))
                            {
                                dataSaida = sqlDataReader.GetDateTime(14);
                            }

                            //Funcionario que registra Saida
                            if (!sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("UsuarioSaida")))
                            {
                                usuarioSaida = new Usuario { Id = sqlDataReader.GetInt32(15) };
                            }
                            else
                            {
                                usuarioSaida = new Usuario { Id = 0 };
                            }

                            //Valor
                            if (!sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("Valor")))
                            {
                                valor = sqlDataReader.GetDecimal(16);
                            }

                            registros.Add(new Registro
                            {
                                Id = sqlDataReader.GetInt32(0),
                                Veiculo = new Veiculo
                                {
                                    Id = sqlDataReader.GetInt32(4),
                                    Placa = sqlDataReader.GetString(3),
                                    Observacao = sqlDataReader.GetString(5),
                                    Modelo = new Modelo
                                    {
                                        Id = sqlDataReader.GetInt32(2),
                                        Nome = sqlDataReader.GetString(1),
                                        Marca = new Marca
                                        {
                                            Id = sqlDataReader.GetInt32(11),
                                            Nome = sqlDataReader.GetString(10)
                                        }
                                    }
                                },
                                DataDeEntrada = sqlDataReader.GetDateTime(7),
                                UsuarioEntrada = new Usuario { Id = sqlDataReader.GetInt32(8) },
                                CustoHora = sqlDataReader.GetDecimal(9),
                                Estacionamento = new Estacionamento { Id = sqlDataReader.GetInt32(13), Endereco = sqlDataReader.GetString(12) },
                                DataDeSaida = dataSaida,
                                UsuarioSaida = usuarioSaida,
                                Valor = valor
                            });
                        }
                    }
                }
                catch (SqlException)
                {
                    return registros;
                }
            }
            return registros;
        }

        public List<Registro> ListarItens()
        {
            throw new NotImplementedException();
        }

        public List<Registro> GeraRelatorio(Relatorio relatorio)
        {
            List<Registro> registros = new List<Registro>();

            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                string retiraCarrosEstacionados = string.Empty;
                if (!relatorio.IncluirCarrosEstacionados)
                {
                    retiraCarrosEstacionados = " AND Registros.DataDeSaida IS NOT NULL";
                }

                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "SELECT Registros.Id, Modelos.Nome AS ModeloNome, Modelos.Id AS ModeloId, Veiculo.Placa, Veiculo.Id AS VeiculoId," +
                                         " Veiculo.Observacao, Registros.Estacionamento, Registros.DataDeEntrada, Registros.UsuarioEntrada," +
                                         " Estacionamentos.CustoHora, Marca.Nome AS MarcaNome, Marca.Id AS MarcaId, Estacionamentos.Nome AS EstacionamentoNome," +
                                         " Estacionamentos.Id AS EstacionamentoId, Registros.DataDeSaida, Registros.UsuarioSaida, Registros.Valor" +
                                         " FROM Veiculo" +
                                         " INNER JOIN Registros ON Veiculo.Id = Registros.Veiculo" +
                                         " INNER JOIN Modelos ON Veiculo.Modelo = Modelos.Id" +
                                         " INNER JOIN Estacionamentos ON Registros.Estacionamento = Estacionamentos.Id" +
                                         " INNER JOIN Marca ON Modelos.Marca = Marca.id" +
                                         " WHERE (Estacionamentos.ID = @estacionamentoId AND Registros.DataDeEntrada > @dataInicio AND " +
                                         " Registros.DataDeEntrada < @dataFinal " + retiraCarrosEstacionados + ")";

                sqlCommand.Parameters.Add("@estacionamentoId", SqlDbType.Int).Value = relatorio.Estacionamento.Id;
                sqlCommand.Parameters.Add("@dataInicio", SqlDbType.DateTime).Value = relatorio.Semana.DataInicial;
                sqlCommand.Parameters.Add("@dataFinal", SqlDbType.DateTime).Value = relatorio.Semana.DataFinal;
                
                try
                {
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            DateTime dataSaida = DateTime.MinValue;
                            Usuario usuarioSaida = null;
                            Decimal valor = 0;

                            //Valida dados para o relatorio
                            //Data de Saida
                            if (!sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("DataDeSaida")))
                            {
                                dataSaida = sqlDataReader.GetDateTime(14);
                            }

                            //Funcionario que registra Saida
                            if (!sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("UsuarioSaida")))
                            {
                                usuarioSaida = new Usuario { Id = sqlDataReader.GetInt32(15) };
                            }
                            else
                            {
                                usuarioSaida = new Usuario { Id = 0 };
                            }

                            //Valor
                            if (!sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("Valor")))
                            {
                                valor = sqlDataReader.GetDecimal(16);
                            }

                            registros.Add(new Registro
                            {
                                Id = sqlDataReader.GetInt32(0),
                                Veiculo = new Veiculo
                                {
                                    Id = sqlDataReader.GetInt32(4),
                                    Placa = sqlDataReader.GetString(3),
                                    Observacao = sqlDataReader.GetString(5),
                                    Modelo = new Modelo
                                    {
                                        Id = sqlDataReader.GetInt32(2),
                                        Nome = sqlDataReader.GetString(1),
                                        Marca = new Marca
                                        {
                                            Id = sqlDataReader.GetInt32(11),
                                            Nome = sqlDataReader.GetString(10)
                                        }
                                    }
                                },
                                DataDeEntrada = sqlDataReader.GetDateTime(7),
                                UsuarioEntrada = new Usuario { Id = sqlDataReader.GetInt32(8) },
                                CustoHora = sqlDataReader.GetDecimal(9),
                                Estacionamento = new Estacionamento { Id = sqlDataReader.GetInt32(13), Endereco = sqlDataReader.GetString(12) },
                                DataDeSaida = dataSaida,
                                UsuarioSaida = usuarioSaida,
                                Valor = valor
                            });
                        }
                    }
                }
                catch (SqlException ex)
                {
                    //Log ex.ToString();
                    return registros;
                }
            }
            return registros;
        }
       
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_conn != null)
                {
                    _conn.Dispose();
                    _conn = null;
                }
            }
            //Ref: https://docs.microsoft.com/pt-br/visualstudio/code-quality/ca1816-call-gc-suppressfinalize-correctly?view=vs-2015
        }
    }
}

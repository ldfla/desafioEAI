using EstacionamentoEAI.DAO.Interfaces;
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
    public class ModeloDAO : IDAO<Modelo>, IDisposable
    {
        private IConnection _conn;

        public ModeloDAO(IConnection connection)
        {
            this._conn = connection;
        }

        public bool Atualizar(Modelo model)
        {
            throw new NotImplementedException();
        }

        public Modelo BuscarItem(params object[] objeto)
        {
            Modelo modelo = null;
            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                sqlCommand.CommandType = System.Data.CommandType.Text;

                string action = objeto[0].ToString();
                string selectQuery = string.Empty;
                string nomeModelo = string.Empty;
                int idMarca = 0;
                switch (action)
                {
                    case "modelo":
                        nomeModelo = objeto[1].ToString();
                        idMarca = Convert.ToInt32(objeto[2].ToString());
                        selectQuery = "SELECT Id, Nome, Marca FROM Modelos WHERE (Nome = @nomeModelo AND Marca = @idMarca)";
                        sqlCommand.Parameters.Add("@nomeModelo", SqlDbType.NVarChar).Value = nomeModelo.ToUpper();
                        sqlCommand.Parameters.Add("@idMarca", SqlDbType.Int).Value = idMarca;

                        break;
                    default:
                        return modelo;
                }
                sqlCommand.CommandText = selectQuery;
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    modelo = new Modelo
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Nome = sqlDataReader.GetString(1),
                        Marca = new Marca { Id = sqlDataReader.GetInt32(2)}
                    };
                }
                sqlDataReader.Close();
            }
            return modelo;
        }

        public Modelo Inserir(Modelo model)
        {
            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "INSERT into Modelos (Nome, Marca)" +
                                        " VALUES (@nome, @marca) SELECT SCOPE_IDENTITY()";

                //Adiciona os parametros de Inserção. 
                sqlCommand.Parameters.Add("@nome", SqlDbType.NVarChar).Value = model.Nome;
                sqlCommand.Parameters.Add("@marca", SqlDbType.Int).Value = model.Marca.Id;
                
                //Executa a Query e retorna o ID do registro adicionado
                model.Id = Convert.ToInt32(sqlCommand.ExecuteScalar().ToString());
            }
            return model;
        }

        public List<Modelo> ListarItens(int marca)
        {
            List<Modelo> listaDeModelos = new List<Modelo>();
            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "SELECT Id, Nome FROM Modelos WHERE Marca = @marca";

                sqlCommand.Parameters.Add("@marca", System.Data.SqlDbType.Int).Value = marca;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        Modelo modelo  = new Modelo
                        {
                            Id = sqlDataReader.GetInt32(0),
                            Nome = sqlDataReader.GetString(1)
                        };

                        listaDeModelos.Add(modelo);

                    }
                }
                sqlDataReader.Close();
            }
            return listaDeModelos;
        }

        public List<Modelo> ListarItens()
        {
            throw new NotImplementedException();
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

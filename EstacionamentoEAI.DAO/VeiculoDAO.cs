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
    public class VeiculoDAO : IDAO<Veiculo>, IDisposable
    {
        private IConnection _conn;

        public VeiculoDAO(IConnection connection)
        {
            this._conn = connection;
        }

        public bool Atualizar(Veiculo model)
        {
            throw new NotImplementedException();
        }

        public Veiculo BuscarItem(params object[] objeto)
        {
            Veiculo veiculo = null;
            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                sqlCommand.CommandType = System.Data.CommandType.Text;

                string selectQuery = string.Empty;
                string placa = string.Empty;
                switch (objeto[0].ToString())
                {
                    case "placa":
                        placa = objeto[1].ToString();
                        selectQuery = "SELECT ID, Placa, Modelo, Observacao, Cliente FROM Veiculo WHERE Placa = @placa";
                        sqlCommand.Parameters.Add("@placa", SqlDbType.NVarChar).Value = placa;
                        break;
                    default:
                        return veiculo;
                }
                sqlCommand.CommandText = selectQuery;
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    veiculo = new Veiculo
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Placa = sqlDataReader.GetString(1),
                        Modelo = new Modelo { Id = sqlDataReader.GetInt32(2) },
                        Observacao = sqlDataReader.GetString(3),
                        Cliente = new Cliente { Id = sqlDataReader.GetInt32(4) }
                    };
                }
                sqlDataReader.Close();
            }
            return veiculo;
        }

        public Veiculo Inserir(Veiculo model)
        {
            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "INSERT into Veiculo (Placa, Modelo, Observacao, Cliente) VALUES (@placa, @modelo, @observacao, @cliente) SELECT SCOPE_IDENTITY()";

                //Adiciona os parametros de Inserção. 
                sqlCommand.Parameters.Add("@placa", SqlDbType.NVarChar).Value = model.Placa;
                sqlCommand.Parameters.Add("@modelo", SqlDbType.Int).Value = model.Modelo.Id;
                sqlCommand.Parameters.Add("@observacao", SqlDbType.NVarChar).Value = model.Observacao;
                sqlCommand.Parameters.Add("@cliente", SqlDbType.Int).Value = model.Cliente.Id;

                //Executa a Query e retorna o ID do veiculo adicionado
                model.Id = int.Parse(sqlCommand.ExecuteScalar().ToString());
            }
            return model;
        }

        public bool Remover(Veiculo model)
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

        public List<Veiculo> ListarItens()
        {
            throw new NotImplementedException();
        }
    }
}

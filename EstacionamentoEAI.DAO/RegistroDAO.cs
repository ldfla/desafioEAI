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
    public class RegistroDAO : IDAO<Registro>, IDisposable
    {
        private IConnection _conn;

        public RegistroDAO(IConnection connection)
        {
            this._conn = connection;
        }

        public bool Atualizar(Registro model)
        {
            throw new NotImplementedException();
        }

        public Registro BuscarItem(params object[] objeto)
        {
            throw new NotImplementedException();
        }

        public Registro Inserir(Registro model)
        {
            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "INSERT into Registros (Estacionamento, Veiculo, DataDeEntrada, UsuarioEntrada) VALUES (@estacionamento, @veiculo, @datadeentrada, @usuarioentrada) SELECT SCOPE_IDENTITY()";

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

        public Collection<Registro> ListarItens(int estacionamentoId)
        {
            throw new NotImplementedException();    
        }

        public bool Remover(Registro model)
        {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
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

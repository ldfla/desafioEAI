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
            throw new NotImplementedException();
        }

        public Veiculo Inserir(Veiculo model)
        {
                using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
                {
                    //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = "INSERT into Veiculos (Placa, Modelo, Observacao, Cliente) VALUES (@placa, @modelo, @observacao, @cliente) SELECT SCOPE_IDENTITY()";
                
                    //Adiciona os parametros de Inserção. 
                    sqlCommand.Parameters.Add("@placa", SqlDbType.NVarChar).Value = model.Placa;
                    sqlCommand.Parameters.Add("@modelo", SqlDbType.Int).Value = model.Modelo;
                    sqlCommand.Parameters.Add("@observacao", SqlDbType.NVarChar).Value = model.Observacao;
                    sqlCommand.Parameters.Add("@cliente", SqlDbType.Int).Value = model.Cliente;

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
            throw new NotImplementedException();
        }

        public List<Veiculo> ListarItens()
        {
            throw new NotImplementedException();
        }
    }
}

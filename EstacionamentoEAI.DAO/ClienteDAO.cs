using EstacionamentoEAI.DAO.Interfaces;
using EstacionamentoEAI.Definition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstacionamentoEAI.DAO
{
    public class ClienteDAO : IDAO<Cliente>, IDisposable
    {
        private IConnection _conn;

        public ClienteDAO(IConnection connection)
        {
            this._conn = connection;
        }

        public bool Atualizar(Cliente model)
        {
            throw new NotImplementedException();
        }

        public Cliente BuscarItem(params object[] objeto)
        {
            throw new NotImplementedException();
        }

        public Cliente Inserir(Cliente model)
        {
            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "INSERT into Clientes (Nome, Endereco, Documento, Telefone)" +
                                        " VALUES (@nome, @endereco, @documento, @telefone) SELECT SCOPE_IDENTITY()";

                //Adiciona os parametros de Inserção. 
                sqlCommand.Parameters.Add("@nome", SqlDbType.NVarChar).Value = model.Nome;
                sqlCommand.Parameters.Add("@endereco", SqlDbType.NVarChar).Value = model.Endereco;
                sqlCommand.Parameters.Add("@documento", SqlDbType.NVarChar).Value = model.Documento ;
                sqlCommand.Parameters.Add("@telefone", SqlDbType.NVarChar).Value = model.Telefone;


                //Executa a Query e retorna o ID do registro adicionado
                model.Id = Convert.ToInt32(sqlCommand.ExecuteScalar().ToString());
            }
            return model;
        }

        public List<Cliente> ListarItens()
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

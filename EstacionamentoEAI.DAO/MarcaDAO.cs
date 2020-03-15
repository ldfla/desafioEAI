using EstacionamentoEAI.DAO.Interfaces;
using EstacionamentoEAI.Definition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstacionamentoEAI.DAO
{
    public class MarcaDAO : IDAO<Marca>, IDisposable
    {
        private IConnection _conn;

        public MarcaDAO(IConnection connection)
        {
            this._conn = connection;
        }

        public bool Atualizar(Marca model)
        {
            throw new NotImplementedException();
        }

        public Marca BuscarItem(params object[] objeto)
        {
            throw new NotImplementedException();
        }


        public Marca Inserir(Marca model)
        {
            throw new NotImplementedException();
        }


        public bool Remover(Marca model)
        {
            throw new NotImplementedException();
        }

        public List<Marca> ListarItens()
        {
            List<Marca> listaDeMarcas = new List<Marca>();

            using (SqlCommand sqlCommand = _conn.AbrirConexao().CreateCommand())
            {
                //Define o comando SQL como tipo Texto, utilizando Query diretamente no SQL. Sem uso de SP
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = "SELECT Id, Nome FROM Marca";
                                
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        Marca marca = new Marca
                        {
                            Id = sqlDataReader.GetInt32(0),
                            Nome = sqlDataReader.GetString(1)
                        };

                        listaDeMarcas.Add(marca);
                        
                    }
                }
                sqlDataReader.Close();
            }
            return listaDeMarcas;
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

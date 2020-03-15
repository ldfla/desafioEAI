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
    public class UsuarioDAO : IDAO<Usuario>, IDisposable
    {
        private IConnection _conn;

        public UsuarioDAO(IConnection connection)
        {
            this._conn = connection;
        }
        
        public bool Atualizar(Usuario model)
        {
            throw new NotImplementedException();
        }

        public Usuario BuscarItem(params object[] objeto)
        {
            Usuario usuario = null;

            var connection = new Connection();
            SqlConnection conn = connection.AbrirConexao();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;

            switch (objeto[0].ToString())
            {
                case "funcionario":
                    sqlCommand.CommandText = "SELECT Id, Email, Nome, Login FROM Usuarios WHERE Login = 'funcionarioeai' and Password = 'abc1234'";
                    break;
                case "gerente":
                    sqlCommand.CommandText = "SELECT Id, Email, Nome, Login FROM Usuarios WHERE Login = 'gerenteeai' and Password = 'abc1234'";
                    break;
                default:
                    sqlCommand.CommandText = "";
                    break;
            }


            if (sqlCommand.CommandText.Length != 0)
            {
                sqlCommand.Connection = conn;
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    usuario = new Usuario
                    {
                        Id = sqlDataReader.GetInt32(0),
                        Email = sqlDataReader.GetString(1),
                        Nome = sqlDataReader.GetString(2),
                        Login = sqlDataReader.GetString(3)
                    };
                }
                connection.FecharConexao(); 
            }
            return usuario;
        }


        public Usuario Inserir(Usuario model)
        {
            throw new NotImplementedException();
        }

        public bool Remover(Usuario model)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> ListarItens()
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

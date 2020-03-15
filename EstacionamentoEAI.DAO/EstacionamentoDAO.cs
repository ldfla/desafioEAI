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
    public class EstacionamentoDAO : IDAO<Estacionamento>, IDisposable
    {
        private IConnection _conn;

        public EstacionamentoDAO(IConnection connection)
        {
            this._conn = connection;
        }

        public bool Atualizar(Estacionamento model)
        {
            throw new NotImplementedException();
        }

        public Estacionamento BuscarItem(params object[] objeto)
        {
            Estacionamento estacionamento = null;
            string action = objeto[0].ToString();

            var connection = new EstacionamentoEAI.DAO.Connection();
            SqlConnection conn = connection.AbrirConexao();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;

            switch (action)
            {
                case "vagas":
                    sqlCommand.CommandText = "SELECT Id, Nome, Endereco, Responsavel, Telefone, NumeroVagas, CustoHora, Manager FROM Estacionamentos WHERE Id = 1";

                    sqlCommand.Connection = conn;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        sqlDataReader.Read();
                        estacionamento = new Estacionamento
                        {
                            Id = sqlDataReader.GetInt32(0),
                            Nome = sqlDataReader.GetString(1),
                            Endereco = sqlDataReader.GetString(2),
                            Responsavel = sqlDataReader.GetString(3),
                            Telefone = sqlDataReader.GetString(4),
                            NumeroDeVagas = sqlDataReader.GetInt32(5),
                            CustoHora = sqlDataReader.GetDecimal(6),
                            Manager = new Usuario { Id = sqlDataReader.GetInt32(7) }
                        };
                    }
                    break;                
                default:
                    break;
            }

            connection.FecharConexao();
            return estacionamento;
        }

        public Estacionamento Inserir(Estacionamento model)
        {
            throw new NotImplementedException();
        }

        public bool Remover(Estacionamento model)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }

        public List<Estacionamento> ListarItens()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstacionamentoEAI.DAO.Interfaces;

namespace EstacionamentoEAI.DAO
{
    public class Connection : IConnection, IDisposable
    {
        private SqlConnection _conn;

        public Connection()
        {
            _conn = new SqlConnection("Data Source=sfc-master; Database=EstacionamentoEAI; User Id=eaibrasil; Password=Password1.;");
        }

        public SqlConnection AbrirConexao()
        {
            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }
            return _conn;            
        }

        public void Dispose()
        {
            this.FecharConexao();
            GC.SuppressFinalize(this);
        }

        public void FecharConexao()
        {
            if (_conn.State == ConnectionState.Open)
            {
                _conn.Close();
            }            
        }
    }
}

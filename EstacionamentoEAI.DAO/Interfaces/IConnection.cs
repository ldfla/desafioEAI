using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace EstacionamentoEAI.DAO.Interfaces
{
    public interface IConnection:IDisposable
    {
        SqlConnection AbrirConexao();

        void FecharConexao();
        
    }
}

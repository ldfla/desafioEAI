using System;
using System.Data.SqlClient;

namespace EstacionamentoEAI.DAO.Interfaces
{
    public interface IConnection:IDisposable
    {
        SqlConnection AbrirConexao();

        void FecharConexao();
        
    }
}

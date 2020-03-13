using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstacionamentoEAI;
using EstacionamentoEAI.DAO;
using System.Data.SqlClient;
using System.Data;

namespace EsacionamentoEAI.UnitTest
{
    [TestClass]
    public class EstacionamentoEAIUnitTest
    {
        [TestMethod]
        public void VerificaBancoDeDados()
        {
            var connection = new EstacionamentoEAI.DAO.Connection();
            SqlConnection conn = connection.AbrirConexao();
            Assert.AreEqual(conn.State, ConnectionState.Open);
        }

        [TestMethod] 
        public void AutenticarUsuario()
        {
        }
        [TestMethod]

        public void RegistrarEntradaVeiculo()
        {
        }

        [TestMethod]
        public void RegistrarSaidaVeiculo()
        {
        }

        [TestMethod]
        public void CadastrarProprietario()
        {
        }

        [TestMethod]
        public void CadastrarVeiculo()
        {
        }

        [TestMethod]
        public void AdicionarMarca()
        {
        }
        [TestMethod]
        public void AdicionarModeloVeiculo()
        {
        }
        [TestMethod]
        public void AdicionarCorVeiculo()
        {
        }


        [TestMethod]
        public void GerarRelatorio()
        {
        }
    }
}

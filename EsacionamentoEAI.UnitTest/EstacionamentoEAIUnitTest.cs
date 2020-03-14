using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstacionamentoEAI;
using EstacionamentoEAI.Controllers;
using EstacionamentoEAI.DAO;
using System.Data.SqlClient;
using System.Data;
using EstacionamentoEAI.Definition;
using System.Web.Mvc;




namespace EstacionamentoEAI.UnitTest
{
    [TestClass]
    public class EstacionamentoEAIUnitTest
    {
        [TestMethod]
        public void VerificaAbrirBancoDeDados()
        {
            var connection = new EstacionamentoEAI.DAO.Connection();
            SqlConnection conn = connection.AbrirConexao();
            Assert.AreEqual(conn.State, ConnectionState.Open);
            connection.FecharConexao();
        }

        [TestMethod]
        public void VerificaFecharBancoDeDados()
        {
            var connection = new EstacionamentoEAI.DAO.Connection();
            SqlConnection conn = connection.AbrirConexao();
            connection.FecharConexao();
            Assert.AreEqual(conn.State, ConnectionState.Closed);
        }

        //Para futura implementação de mecanismo de autenticação. 
        [TestMethod]
        public void AutenticarUsuario()
        {
            var controller = new EstacionamentoController();
            var usuario = controller.AutenticaFuncionarioFake();
            Assert.AreEqual("Funcionario", usuario.Nome);
        }

        //Para futura implementação de mecanismo de autenticação. 
        [TestMethod]
        public void AutenticarGerente()
        {
            var controller = new GerenciaController();
            var usuario = controller.AutenticaGerenteFake();
            Assert.AreEqual("Gerente", usuario.Nome);
        }
        
        [TestMethod]
        public void VerificaVagasOcupadas()
        {
            var connection = new Connection();
            RegistroDAO registroDAO = new RegistroDAO(connection);
            registroDAO.ContaVagasOcupadas(1);
            Assert.IsNotNull(registroDAO.ContaVagasOcupadas(1));
        }

        [TestMethod]
        public void RegistrarEntradaVeiculo()
        {
            Veiculo veiculo = new Veiculo
            {
                Cliente = null,
                Modelo = new Modelo { Id = 1 },
                Observacao = "",
                Placa = "AAA-0000"
            };
            var controller = new EstacionamentoController();

            int Id = controller.RegistraEntrada(veiculo);
            Assert.AreNotEqual(0, Id);
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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstacionamentoEAI;
using EstacionamentoEAI.Controllers;
using EstacionamentoEAI.DAO;
using System.Data.SqlClient;
using System.Data;
using EstacionamentoEAI.Definition;
using System.Web.Mvc;
using EstacionamentoEAI.DAO.Interfaces;
using System.Collections.Generic;

namespace EstacionamentoEAI.UnitTest
{
    [TestClass]
    public class EstacionamentoEAIUnitTest
    {
        IConnection conn = new Connection();

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
                Cliente = new Cliente(),
                Modelo = new Modelo { Id = 1 },
                Observacao = "",
                Placa = "AAA-0000"
            };
            var controller = new EstacionamentoController();
            FormCollection formCollection = new FormCollection {
                { "placaVeiculo","AAA-0000" },
                { "modeloVeiculo","1" },
                { "observacaoVeiculo","" },
                { "nomeCliente","" }
            };
            
            int Id = controller.RegistraEntrada(formCollection);
            Assert.AreNotEqual(0, Id);
        }

        [TestMethod]
        public void RegistrarSaidaVeiculo()
        {

            var controller = new EstacionamentoController();

            bool atualizado = controller.RegistraSaida("AAA-0000");
            Assert.IsTrue(atualizado);
        }

        [TestMethod]
        public void CadastrarProprietario()
        {
        }

        [TestMethod]
        public void CadastrarVeiculo()
        {
            Veiculo veiculo = new Veiculo { Placa = "AAA-0000",
                Modelo = new Modelo {
                    Id = 1,
                    Nome = "TLX",
                    Marca = new Marca { Id = 1 }
                },
                Observacao = "",
                Cliente = new Cliente()
            };

            VeiculoDAO veiculoDAO = new VeiculoDAO(conn);
            veiculo = veiculoDAO.Inserir(veiculo);
            Assert.AreNotEqual(0, veiculo.Id);
        }
        
        [TestMethod]
        public void ListarModelos()
        {
            ModeloDAO modeloDAO = new ModeloDAO(conn);
            List<Modelo> listModelos = modeloDAO.ListarItens(1);
            Assert.AreNotEqual(0, listModelos.Count);
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
        public void GerarRelatorio()
        {
        }
    }
}

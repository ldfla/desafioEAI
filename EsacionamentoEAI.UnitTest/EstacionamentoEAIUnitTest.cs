using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void CadastrarVeiculo()
        {
            Veiculo veiculo = new Veiculo { Placa = "CCC-2222",
                Modelo = new Modelo {
                    Id = 507,
                    Nome = "306",
                    Marca = new Marca { Id = 220 }
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
        public void TestaSemana()
        {
            Semana semana = new Semana(new DateTime(2020, 3, 11), DayOfWeek.Sunday);
            DateTime dataInicialTest = new DateTime(2020, 3, 8);
            DateTime dataFinalTest = new DateTime(2020, 3, 15);
            //Assert.AreEqual(semana.DataInicial, dataInicialTest);
            Assert.AreEqual(semana.DataFinal, dataFinalTest);
        }

        [TestMethod]
        public void GerarRelatorioSemanal()
        {
            RegistroDAO registroDAO = new RegistroDAO(conn);
            Semana semana = new Semana(new DateTime(2020, 3, 11), DayOfWeek.Sunday);
            Estacionamento estacionamento = new Estacionamento { Id = 1 };
            Relatorio relatorio = new Relatorio(semana, estacionamento);
            relatorio.IncluirCarrosEstacionados = false;

            List<Registro> registros = registroDAO.GeraRelatorio(relatorio);

            Assert.AreNotEqual(0, registros.Count);
        }

        [TestMethod]
        public void DadosDoRelatorio()
        {
            var controller = new GerenciaController();
            Relatorio relatorio = new Relatorio(new Semana(new DateTime(2020,03,10), DayOfWeek.Sunday), new Estacionamento { Id = 1});
            RegistroDAO registroDAO = new RegistroDAO(conn);
            relatorio.Registros = registroDAO.GeraRelatorio(relatorio);

            relatorio.View = controller.GerarDadosRelatorio(relatorio);
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
        public void GerarHistorico()
        {
            RegistroDAO registroDAO = new RegistroDAO(conn);
            List<Registro> registros = registroDAO.ListarItens("FQJ4444");

            Assert.AreNotEqual(0, registros.Count);
        }

        [TestMethod]
        public void CadastrarProprietario()
        {
        }

    }
}

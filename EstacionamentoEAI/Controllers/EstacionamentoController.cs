using EstacionamentoEAI.Definition;
using EstacionamentoEAI.DAO;
using EstacionamentoEAI.DAO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EstacionamentoEAI.Controllers
{
   
    public class EstacionamentoController : Controller
    {
        IConnection conn = new Connection();
        Estacionamento estacionamento = new Estacionamento();

        // GET: Estacionamento
        [HttpGet]
        public ActionResult Index()
        {
            
            EstacionamentoDAO estacionamentoDAO = new EstacionamentoDAO(conn);
            estacionamento = estacionamentoDAO.BuscarItem("vagas");
            RegistroDAO registroDAO = new RegistroDAO(conn);
            int vagasOcupadas = registroDAO.ContaVagasOcupadas(estacionamento.Id);
            MarcaDAO marcaDAO = new MarcaDAO(conn);
            List<Marca> lstMarca = marcaDAO.ListarItens();

            ViewBag.VagasTotal = estacionamento.NumeroDeVagas;
            ViewBag.VagasOcupadas = vagasOcupadas;
            ViewData["Marca"] = lstMarca;

            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            string action = formCollection["controle_btn"];
            if (action == "entrada")
            {

                RegistraEntrada(new Veiculo { 
                    Placa = formCollection["placaVeiculo"],
                    Modelo = new Modelo { Id = int.Parse(formCollection["modeloVeiculo"]) },
                    Observacao = formCollection["observacaoVeiculo"]
                });
            }
        
            return RedirectToAction("Index");
        }

              

        #region private Methods

        public int RegistraEntrada(Veiculo veiculo)
        {
            int registroId = 0;
            using (IConnection conn = new Connection())
            {
                conn.AbrirConexao();

                Registro registro = new Registro
                {
                    DataDeEntrada = DateTime.Now,
                    Estacionamento = estacionamento,
                    UsuarioEntrada = AutenticaFuncionarioFake(),
                    Veiculo = veiculo,
                };
                
                IDAO<Registro> registroDAO = new RegistroDAO(conn);
                Registro novoRegistro = registroDAO.Inserir(registro);
                registroId = novoRegistro.Id;
            }
            return registroId;
        }

        public void RegistraSaida(Veiculo veiculo)
        {
            using (IConnection conn = new Connection())
            {
                Registro registro = new Registro
                {
                    DataDeSaida = DateTime.Now,
                    Estacionamento = estacionamento,
                    UsuarioEntrada = AutenticaFuncionarioFake(),
                    Veiculo = veiculo,
                };
            }
        }

        public Usuario AutenticaFuncionarioFake()
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO(conn);
            return usuarioDAO.BuscarItem("funcionario");
        }

        #endregion
    }
}

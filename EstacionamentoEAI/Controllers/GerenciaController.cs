using EstacionamentoEAI.Definition;
using EstacionamentoEAI.DAO;
using EstacionamentoEAI.DAO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace EstacionamentoEAI.Controllers
{
    
    public class GerenciaController : Controller
    {

        IConnection conn = new Connection();
        Estacionamento estacionamento = new Estacionamento();


        /// <summary>
        /// Index do Estacionamento. Versão Gerencial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            EstacionamentoDAO estacionamentoDAO = new EstacionamentoDAO(conn);
            estacionamento = estacionamentoDAO.BuscarItem("vagas");
            RegistroDAO registroDAO = new RegistroDAO(conn);
            int vagasOcupadas = registroDAO.ContaVagasOcupadas(estacionamento.Id);

            ViewBag.VagasTotal = estacionamento.NumeroDeVagas;
            ViewBag.VagasOcupadas = vagasOcupadas;
            ViewBag.VagasDisponiveis = estacionamento.NumeroDeVagas - vagasOcupadas;
            ViewBag.Estacionamento = estacionamento.Endereco;
            return View();
        }

        public ActionResult CarrosEstacionados()
        {
            RegistroDAO registroDAO = new RegistroDAO(conn);
            List<Registro> registros = registroDAO.ListarVagasOcupadas(1);

            ViewData.Model = registros;

            return View();
        }

        public ActionResult Historico(string placa)
        {
            //Regex para as placas AAA-0000, AAA0000 e AAA0A00
            string padraoBrasilMercosul = @"([A-Z]{3}\-?[0-9]([0-9]|[A-Z])[0-9]{2})";
            
            if (Regex.IsMatch(placa, padraoBrasilMercosul, RegexOptions.IgnoreCase))
            {
                ViewBag.Placa = placa.ToUpper();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Gerencia");
            }
        }

        #region private Methods

        public Usuario AutenticaGerenteFake()
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO(conn);
            return usuarioDAO.BuscarItem("gerente");
        }

        #endregion
    }
}

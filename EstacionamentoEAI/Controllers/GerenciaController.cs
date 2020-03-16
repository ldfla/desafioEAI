using EstacionamentoEAI.Definition;
using EstacionamentoEAI.DAO;
using EstacionamentoEAI.DAO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

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

        /// <summary>
        /// Mostra os carros que estão no momento no pátio do estacionamento
        /// </summary>
        /// <returns></returns>
        public ActionResult CarrosEstacionados()
        {
            RegistroDAO registroDAO = new RegistroDAO(conn);
            List<Registro> registros = registroDAO.ListarVagasOcupadas(1);

            ViewData.Model = registros;

            return View();
        }

        /// <summary>
        /// Mostra o histórico de um veículo no estacionamento de acordo com a placa
        /// </summary>
        /// <param name="placa"></param>
        /// <returns></returns>
        public ActionResult Historico(string placa)
        {
            placa = placa.Replace("-", "").ToUpper();

            if (string.IsNullOrEmpty(placa))
            {
                return RedirectToAction("Index", "Gerencia");
            }

            if (VerificaPlaca(placa))
            {
                RegistroDAO registroDAO = new RegistroDAO(conn);
                List<Registro> registros = registroDAO.ListarItens(placa);

                ViewBag.Placa = placa.ToUpper();
                ViewData.Model = registros;

                return View();
            }

            return RedirectToAction("Index", "Gerencia");

        }

        [HttpGet]
        public ActionResult Relatorios()
        {
            EstacionamentoDAO estacionamentoDAO = new EstacionamentoDAO(conn);
            estacionamento = estacionamentoDAO.BuscarItem("vagas");
            RegistroDAO registroDAO = new RegistroDAO(conn);
            int vagasOcupadas = registroDAO.ContaVagasOcupadas(estacionamento.Id);

            ViewBag.VagasTotal = estacionamento.NumeroDeVagas;
            ViewBag.VagasOcupadas = vagasOcupadas;
            ViewBag.VagasDisponiveis = estacionamento.NumeroDeVagas - vagasOcupadas;
            ViewBag.Estacionamento = estacionamento.Endereco;
            ViewBag.DiaDeHoje = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
            return View();
        }

        [HttpPost]
        public ActionResult Semanal(FormCollection formCollection)
        {
            DateTime data = DateTime.Parse(formCollection["dataRelatorio"]);
            bool incluiEstacionados = MakeBoolean(formCollection["incluiEstacionado"]);
            int estacionamentoID = 1;

            EstacionamentoDAO estacionamentoDAO = new EstacionamentoDAO(conn);
            Estacionamento est = estacionamentoDAO.BuscarItem("vagas", estacionamentoID);

            if (est != null)
            {
                Semana semana = new Semana(data, DayOfWeek.Sunday);
                Relatorio relatorio = new Relatorio(semana, est);

                RegistroDAO registroDAO = new RegistroDAO(conn);
                relatorio.Registros = registroDAO.GeraRelatorio(relatorio);

                relatorio.View = GerarDadosRelatorio(relatorio);
                
                return View();
            }

            return RedirectToAction("Index", "Estacionamento");
        }

        #region private Methods

        private List<RelatorioItem> GerarDadosRelatorio(Relatorio relatorio)
        {
            List<RelatorioItem> relatorioItems = new List<RelatorioItem>();
            DateTime dataInicial = relatorio.Semana.DataInicial;

            for (int i = 0; i < 6; i++)
            {

            }

            return relatorioItems;
        }

        private bool MakeBoolean(string value)
        {
            value = value.ToUpper().Trim();
            if (value == "ON" || value=="1" || value == "TRUE" || value == "SIM")
                return true;

            return false;
        }

        public Usuario AutenticaGerenteFake()
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO(conn);
            return usuarioDAO.BuscarItem("gerente");
        }

        private bool VerificaPlaca(string placa)
        {
            //Regex para as placas AAA-0000, AAA0000 e AAA0A00
            string padraoBrasilMercosul = @"([A-Z]{3}\-?[0-9]([0-9]|[A-Z])[0-9]{2})";

            if (Regex.IsMatch(placa, padraoBrasilMercosul, RegexOptions.IgnoreCase))
                return true;

            return false;
        }

        #endregion
        
    }


}

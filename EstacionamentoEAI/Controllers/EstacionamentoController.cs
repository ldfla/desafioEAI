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
            string placa = formCollection["placaVeiculo"].ToUpper();

            switch (action)
            {
                case "entrada":
                    
                    if (RegistraEntrada(formCollection) > 0)
                    {
                        RedirectToAction("Index");
                    }
                    else {
                        return RedirectToAction("Estacionado", new { placa = placa });
                    }
                    break;
                case "saida":
                    break;
                default:
                    return View();                    
            }
                    
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult Estacionado(string placa)
        {
            ViewBag.Placa = placa.ToUpper();
            return View();
        }


        #region private Methods

        public int RegistraEntrada(FormCollection formCollection)
        {
            string placa = formCollection["placaVeiculo"].ToUpper();
            string cliente = formCollection["nomeCliente"].ToUpper();
            int modelo = int.Parse(formCollection["modeloVeiculo"]);
            string observacao = formCollection["observacaoVeiculo"];

            Veiculo veiculo = new Veiculo {
                Placa = placa,
                Modelo = new Modelo { Id = modelo },
                Observacao = observacao, 
                Cliente = new Cliente()
            };

            int registroId = 0;
            using (IConnection conn = new Connection())
            {
                EstacionamentoDAO estacionamentoDAO = new EstacionamentoDAO(conn);
                estacionamento = estacionamentoDAO.BuscarItem("vagas");
                RegistroDAO registroDAO = new RegistroDAO(conn);
                int vagasOcupadas = registroDAO.ContaVagasOcupadas(estacionamento.Id);

                //Verifica se existem vagas disponiveis
                if (estacionamento.NumeroDeVagas > vagasOcupadas)
                {
                    VeiculoDAO veiculoDAO = new VeiculoDAO(conn);
                    //Verifica se o Veiculo já existe no DB
                    Veiculo verificaVeiculo = veiculoDAO.BuscarItem("placa", veiculo.Placa);

                    if (verificaVeiculo == null)
                    {
                        veiculo = veiculoDAO.Inserir(veiculo);
                    }
                    else
                    {
                        veiculo = verificaVeiculo;
                    }

                    //Verifica se o Veiculo já está estacionado 
                    Registro registro = registroDAO.BuscarItem("placa", veiculo.Placa);

                    if (registro == null)
                    {
                        registro = new Registro
                        {
                            DataDeEntrada = DateTime.Now,
                            Estacionamento = estacionamento,
                            UsuarioEntrada = AutenticaFuncionarioFake(),
                            Veiculo = veiculo,
                        };

                        Registro novoRegistro = registroDAO.Inserir(registro);
                        registroId = novoRegistro.Id;
                    }
                    conn.FecharConexao();
                }
            }
            return registroId;
        }

        public bool RegistraSaida(string placa)
        {
            bool atualizado = false;

            using (IConnection conn = new Connection())
            {
                IDAO<Registro> registroDAO = new RegistroDAO(conn);
                Registro registro = registroDAO.BuscarItem(placa);

                if (registro.DataDeSaida == null)
                {
                    registro.DataDeSaida = DateTime.Now;
                    registro.UsuarioSaida = AutenticaFuncionarioFake();
                    TimeSpan timeSpan = registro.DataDeSaida - registro.DataDeEntrada;
                    int horas = Convert.ToInt32(Math.Ceiling(timeSpan.TotalHours));
                    registro.Valor = horas * registro.CustoHora;
                } else
                {
                    conn.FecharConexao(); 
                    return atualizado;
                }

                atualizado = registroDAO.Atualizar(registro);
            }

            return atualizado;
        }

        public Usuario AutenticaFuncionarioFake()
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO(conn);
            return usuarioDAO.BuscarItem("funcionario");
        }

        #endregion
    }
}

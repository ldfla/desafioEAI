using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EstacionamentoEAI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Sobre o Desafio :";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Meus Contatos: ";

            return View();
        }
    }
}
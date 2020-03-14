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
    
    public class GerenciaController : Controller
    {

        IConnection conn = new Connection();


        // GET: Gerencia
        public ActionResult Index()
        {
            return View();
        }

        // GET: Gerencia/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Gerencia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gerencia/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Gerencia/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Gerencia/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Gerencia/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Gerencia/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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

using ItauProjeto.CRUD;
using ItauProjeto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ItauProjeto.Controllers
{
    public class CompartilhadoController : Controller
    {

        [ChildActionOnly]
        public ActionResult GetSessionLogin()
        {
            ViewModelCompartilhado modelSession = new ViewModelCompartilhado();
            HttpCookie cookie = Request.Cookies["loggedCliente"];
            if (cookie != null)
            {
                 Session["loggedCliente"] = cookie.Value;
            }
            if (Session["loggedCliente"] != null)
            {
                CRUDCliente crud = new CRUDCliente();
                modelSession.login = crud.ListarPorID(Session["loggedCliente"].ToString())?.Nome;
                modelSession.id = Session["loggedCliente"].ToString();
            }

            if (cookie != null && string.IsNullOrEmpty(modelSession.login))
            {
                Response.Cookies["loggedCliente"].Expires = DateTime.Now.AddDays(-1);
                Session["loggedCliente"] = null;
            }
            return PartialView("~/Views/Shared/_GetSessionLogin.cshtml", modelSession);
        }

        [ChildActionOnly]
        public ActionResult LayoutMessage()
        {
            return PartialView("~/Views/Shared/_LayoutMessage.cshtml");
        }

        [ChildActionOnly]
        public ActionResult LayoutFormExclusao(string id)
        {
            CRUDCliente crud = new CRUDCliente();
            ModelCliente model = crud.ListarPorID(id);
            return PartialView("~/Views/Shared/_LayoutFormExclusao.cshtml", model);
        }

        public ActionResult Fechar()
        {
            Session["loggedCliente"] = null;
            Response.Cookies["loggedCliente"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "Home");
        }

    }
}
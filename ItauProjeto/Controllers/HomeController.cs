using ItauProjeto.CRUD;
using ItauProjeto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ItauProjeto.Controllers
{
    public class HomeController : Controller
    {
        private CRUDCliente crud;
        private ViewModelHome model;


        public HomeController()
        {
            crud = new CRUDCliente();
            model = new ViewModelHome();
        }

        // GET: Home
        public ActionResult Index()
        {
            if (online())
            {
                return RedirectToAction("Index", "Portal", new { id = Session["loggedCliente"] });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ViewModelHome model)
        {
            ModelCliente tempModel = crud.validarLoginSenha(model.modelCliente.Email, model.modelCliente.Senha);
            if(tempModel == null)
            {
                model.respostaLogin = "Usuário não encontrado.";
            }
            else
            {
                string idAux = tempModel.id.ToString();
                if (model.permanecerLogado)
                {
                    adicionarCookie(idAux);
                    Session["loggedCliente"] = idAux;
                }
                else
                {
                    Session["loggedCliente"] = idAux;
                    Response.Cookies["loggedCliente"].Expires = DateTime.Now.AddDays(-1);
                }
                return RedirectToAction("Index", "Portal", new { id = idAux });
            }
            return View(model);
        }

        /*cookie*/
        public void adicionarCookie(string id)
        {
            HttpCookie cookie = Request.Cookies["loggedCliente"];
            if (cookie == null)
            {
                cookie = new HttpCookie("loggedCliente");
            }

            cookie.Value = id;

            cookie.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Add(cookie);
        }
        /*cookie*/
        
        public bool online()
        {
            HttpCookie cookie = Request.Cookies["loggedCliente"];
            if (cookie != null)
            {
                Session["loggedCliente"] = cookie.Value;
            }

            if (Session["loggedCliente"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
//var outroController = DependencyResolver.Current.GetService<HomeController>();//usar metodos de outros controller
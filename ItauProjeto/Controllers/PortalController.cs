using ItauProjeto.CRUD;
using ItauProjeto.Funcoes;
using ItauProjeto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ItauProjeto.Controllers
{
    public class PortalController : Controller
    {

        private CRUDCliente crud;
        private ViewModelPortal model;

        public PortalController()
        {
            crud = new CRUDCliente();
            model = new ViewModelPortal();
        }

        // GET: Portal
        public ActionResult Index(string id)
        {
            
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index", "Home");
            if (!online(id))
            {
                Session["loggedCliente"] = null;
                Response.Cookies["loggedCliente"].Expires = DateTime.Now.AddDays(-1);
                return RedirectToAction("Index", "Home");
            }


            model.modelCliente = crud.ListarPorID(id);
            return View(model);
        }

        public bool online(string id)
        {
            HttpCookie cookie = Request.Cookies["loggedCliente"];
            if (cookie != null)
            {
                Session["loggedCliente"] = cookie.Value;
            }

            if (Session["loggedCliente"] != null && Session["loggedCliente"].ToString() == id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ActionResult Editar(string id)
        {
            if(!online(id))
            {
                return RedirectToAction("Index", "Error");
            }

            ModelCliente model = crud.ListarPorID(id);
            if (model != null)
                return View(model);
            else
                return RedirectToAction("Index", "Error");
        }

        [HttpPost]
        public ActionResult Editar(ModelCliente model)
        {
            if (online(model.id.ToString()) && ModelState.IsValid)
            {
                if (model.ComprovanteEndereco != null)
                {
                    ConfigUpload configUpload = new ConfigUpload(model.id.ToString(), "arquivo", 0, 0, "", Request.PhysicalApplicationPath);
                    model.NomeDoArquivo = configUpload.SalvarArquivo(model.ComprovanteEndereco);
                }
                crud.Salvar(model);

                return RedirectToAction("Index", "Portal", new { id = model.id.ToString() });
            }
            return View(model);
        }

        public ActionResult AlterarSenha(string id)
        {
            if (!online(id))
            {
                return RedirectToAction("Index", "Error");
            }

            ModelCliente model = crud.ListarPorID(id);
            if (model != null)
                return View(model);
            else
                return RedirectToAction("Index", "Error");
        }

        [HttpPost]
        public ActionResult AlterarSenha(ModelCliente model)
        {
            if (online(model.id.ToString()))
            {
                crud.AlterarSenha(model);

                return RedirectToAction("Index", "Portal", new { id = model.id.ToString() });
            }
            return View(model);
        }

        [HttpPost]
        public PartialViewResult AjaxExcluirCliente(ModelCliente model)
        {
            
            ModelCliente tempModel = new ModelCliente();
            if (online(model.id.ToString()) && !string.IsNullOrEmpty(model.SenhaAtual))
            {
                tempModel = crud.ListarPorID(model.id.ToString());

                if (crud.validarSenhaAtual(model.SenhaAtual, model.id.ToString()) && crud.Excluir(tempModel))
                {
                    Session["loggedCliente"] = null;
                    Response.Cookies["loggedCliente"].Expires = DateTime.Now.AddDays(-1);
                    return PartialView("LayoutAjaxExcluirCliente", model);
                }
                else
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            else
            {
                Response.StatusCode = 404;
                return null;
            }            
        }

        /*[HttpPost]
        public JsonResult JsonAjaxExcluir(string id)
        {
            var data = "{\"dados\":[";
            data = data + "{";
            if (online(id))
            {
                ModelCliente tempModel = new ModelCliente();
                tempModel.id = int.Parse(id);
                
                if (crud.Excluir(tempModel))
                {
                    data = data + "\"sucesso\" : \"OK\"";
                    Session["loggedCliente"] = null;
                    Response.Cookies["loggedCliente"].Expires = DateTime.Now.AddDays(-1);
                }
                else
                {
                    data = data + "\"sucesso\" : \"NOTOK\"";
                }
            }
            else
            {
                data = data + "\"sucesso\" : \"NOTOK\"";
            }
            data = data + "}";
            data = data + "]}";            

            return Json(data, JsonRequestBehavior.AllowGet);
        }*/
    }
}
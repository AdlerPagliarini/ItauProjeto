using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ItauProjeto.Models;
using ItauProjeto.CRUD;

namespace ItauProjeto.Controllers
{
    public class ValidacoesController : Controller
    {
        public ActionResult validarDisponibilidadeEmail(ModelCliente v)
        {
            string email = v.Email;
            CRUDCliente crud = new CRUD.CRUDCliente();
            if (string.IsNullOrEmpty(email))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if(v.id > 0){
                    return Json(crud.validarDisponibilidadeEmail(email, v.id.ToString()), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(crud.validarDisponibilidadeEmail(email), JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult validarSenhaAtual(ModelCliente v)
        {
            string id = v.id.ToString();
            string senha = v.SenhaAtual;
            CRUDCliente crud = new CRUDCliente();
            if (string.IsNullOrEmpty(senha))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(crud.validarSenhaAtual(senha, id), JsonRequestBehavior.AllowGet);
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ItauProjeto.CRUD;
using ItauProjeto.Funcoes;
using ItauProjeto.Models;

namespace ItauProjeto.Controllers
{
    public class CadastrarController : Controller
    {
        private CRUDCliente crud;
        private ModelCliente model;
        private ConfigUpload configUpload;

        public CadastrarController()
        {
            crud = new CRUDCliente();
            model = new ModelCliente();
        }

        public ActionResult Index(string id)
        {
            return View(model);            
        }

        [HttpPost]
        public ActionResult Index(ModelCliente model)
        {
            if (ModelState.IsValid)
            {
                //primeiro eu insiro o cliente na base, depois salvo o ComprovanteEndereco na pasta do ID gerado para este cliente
                model.NomeDoArquivo = model.ComprovanteEndereco.FileName;
                string idGerado = crud.Salvar(model).ToString();

                if (idGerado != "0")
                {
                    //salvar comprovante
                    if (Diretorio.criarPasta("//upload//", idGerado))
                    {
                        //como estou usando um classe que desenvolvi para fazer recortes de imagem e criar miniaturas preciso criar essas pastas
                        if (Diretorio.criarPasta("//upload//" + idGerado + "//", "crop") && Diretorio.criarPasta("//upload//" + idGerado + "//", "min"))
                        {
                            configUpload = new ConfigUpload(idGerado, "arquivo", 0, 0, "", Request.PhysicalApplicationPath);
                            configUpload.SalvarArquivo(model.ComprovanteEndereco);
                        }
                    }

                    return RedirectToAction("Index", new { id = idGerado });
                }
            }
            return View(model);
        }
    }
}
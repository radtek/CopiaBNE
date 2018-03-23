using BNE.BLL;
using BNE.EL;
using BNE.Web.Parceiros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BNE.Web.Parceiros.Controllers
{
    public class PerfilController : BaseController
    {
      
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Salvar(Cadastro model)
        {
            ItemSession item = (ItemSession)Session["ItemSession"];
            if (item != null)
            {
                try
                {
                    UsuarioParceiro usuarioParceiro = new UsuarioParceiro();

                    usuarioParceiro.TipoUsuarioParceiro = TipoUsuarioParceiro.loadObject(model.Tipo);
                    usuarioParceiro.IdUsuarioFilialPerfil = item.idUsuarioFilialPerfil;
                    usuarioParceiro.Funcao = Funcao.CarregarPorDescricao(model.Profissao);

                    if (usuarioParceiro.Funcao == null)
                    {
                        ViewBag.Out = new ResponseView() { Title = "Atenção", Message = "A profissão informada é inválida!", Status = "warning", Model = model };
                        return View("Index");
                    }

                    usuarioParceiro.Cidade = Cidade.LoadObject(item.IdCidade);
                    usuarioParceiro.DataCadastro = DateTime.Now;
                    usuarioParceiro.NumDDDCelular = model.Celular.Split(' ')[0].Replace("(", "").Replace(")", "");
                    usuarioParceiro.NumCelular = model.Celular.Split(' ')[1].Replace("-", "");
                    usuarioParceiro.Email = model.Email;
                    usuarioParceiro.NmeUsuario = model.NmePessoa;

                    if (usuarioParceiro.Save())
                    {
                        usuarioParceiro.DispararSMS();
                        ViewBag.Out = new ResponseView() { Title = "Pronto", Message = "Dados cadastrados com sucesso!", Status = "success", Model = null};
                    }
                    else 
                    {
                        ViewBag.Out = new ResponseView() { Title = "Opss", Message = "Não foi possível salvar os dados! Tente novamente mais tarde", Status = "error", Model = model };
                    }    
                }
                catch (Exception ex) 
                {
                    GerenciadorException.GravarExcecao(ex);
                    ViewBag.Out = new ResponseView() { Title = "Opss", Message = "Não foi possível recuperar informações.", Status = "error", Model = model };
                }
            }
            else 
            {
                ViewBag.Out = new ResponseView() { Title = "Opss", Message = "A sessão foi expirada.", Status = "error", Model = model };
            }
            return View("Index");
        }

        public ActionResult Sair()
        {
            Session["ItemSession"] = null;
            return RedirectToAction("Index", "Login");
        }

    }
}

using System;
using System.Web.Mvc;
using BNE.Web.LanHouse.Code.Enumeradores;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.Controllers
{
    public class FilialController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RecarregarOrigem()
        {
            return View();
        }

        public ActionResult CidadeInexistente()
        {
            return View();
        }

        public ActionResult Entrar(string nomeLan)
        {
            string diretorio = nomeLan;
            object model;
            try
            {
                TAB_Origem_Filial objOrigemFilial;
                if (BLL.OrigemFilial.CarregarPorDiretorio(diretorio, out objOrigemFilial))
                {
                    Session[Chave.IdFilialLAN.ToString()] = objOrigemFilial.Idf_Filial;
                    Session[Chave.NomeFantasiaLAN.ToString()] = objOrigemFilial.TAB_Filial.Nme_Fantasia;
                    Session[Chave.CnpjFilialLAN.ToString()] = objOrigemFilial.TAB_Filial.Num_CNPJ;
                    Session[Chave.DiretorioSTCLan.ToString()] = objOrigemFilial.Des_Diretorio;
                    Session[Chave.IdOrigemLAN.ToString()] = objOrigemFilial.Idf_Origem;

                    // pegando cidade da LAN House
                    int idEndereco = objOrigemFilial.TAB_Filial.TAB_Endereco.Idf_Endereco;
                    TAB_Cidade objCidade;
                    if (BLL.Endereco.CarregarCidadePorId(idEndereco, out objCidade))
                        Session[Chave.IdCidadeLAN.ToString()] = objCidade.Idf_Cidade;
                    else
                    {
                        model = new Models.ModelFilialNaoExisteCidade(diretorio);
                        return RedirectToAction("CidadeInexistente", model);
                    }

                    // emitir ticket de origem (cookie)
                    Code.EmitirTicketOrigem.Emitir(this);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
            model = new Models.ModelFilialNaoExisteOrigem(diretorio);
            return View("NaoExisteOrigem", model);
        }
    }
}

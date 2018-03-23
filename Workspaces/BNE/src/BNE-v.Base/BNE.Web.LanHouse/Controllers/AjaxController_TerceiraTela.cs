using System.Web.Mvc;
using BNE.BLL.Custom.IntegrationObjects;
using BNE.BLL.Integracoes.Facebook;
using BNE.Web.LanHouse.BLL.Entity;
using BNE.Web.LanHouse.Code.Enumeradores;
using BNE.Web.LanHouse.BLL;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.Controllers
{
    public partial class AjaxController
    {

        #region Métodos públicos
        public ActionResult TerceiraTela(Models.ModelAjaxTerceiraTela model)
        {
            if (!ModelState.IsValid)
                return MensagemErro();

            Session[Chave.TerceiraTela.ToString()] = new TerceiraTela(model);

            var tela2 = Session[Chave.SegundaTela.ToString()] as SegundaTela;
            var tela3 = Session[Chave.TerceiraTela.ToString()] as TerceiraTela;

            if (tela2 == null || tela3 == null)
                return Json(false);

            TAB_Pessoa_Fisica objPessoaFisica;
            if (BLL.PessoaFisica.CarregarPorCpf(tela3.Cpf, out objPessoaFisica))
            {
                if (objPessoaFisica.Dta_Nascimento.Equals(tela2.DataNasc))
                    FormalizarAutenticacao(objPessoaFisica.Idf_Pessoa_Fisica);
                else
                    return Json(false);
            }

            int idPessoaFisica = IdPessoaFisica();
            int idCidade = IdCidadeLAN();
            int idOrigemLan = IdOrigemLAN();

            int? idOrigemFilial = null;
            //Melhorar este carregamento
            TAB_Origem_Filial objOrigemFilial;
            if (OrigemFilial.CarregarPorFilial(Filial.SelectByID(IdFilialOportunidade()), out objOrigemFilial))
                idOrigemFilial = objOrigemFilial.Idf_Origem;

            var dadosFacebook = Session[Chave.DadosFacebook.ToString()] as ProfileFacebook.DadosFacebook;

            string cartaBoasVindas;
            Companhia.RecuperarCartaBoasVindas(CnpjOportunidade(), out cartaBoasVindas);

            string mensagemErro;
            if (MiniCurriculo.Salvar(ref idPessoaFisica, Code.Helper.RecuperarIP(this), idCidade, idOrigemFilial, idOrigemLan, cartaBoasVindas, tela2, tela3, dadosFacebook, out mensagemErro))
            {
                if (IdPessoaFisica().Equals(0))
                    FormalizarAutenticacao(idPessoaFisica);

                return View("NovoToken");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(mensagemErro))
                    return Json(new { error = mensagemErro });
            }

            return Json(false);
        }
        #endregion Métodos públicos

    }
}
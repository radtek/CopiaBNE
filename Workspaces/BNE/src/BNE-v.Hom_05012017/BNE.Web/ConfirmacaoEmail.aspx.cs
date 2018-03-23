using System;
using System.Web;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web
{
    public partial class ConfirmacaoEmail : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["codigo"]))
                    {
                        var confirmacaoEmail = HttpUtility.ParseQueryString(Helper.FromBase64(Request.QueryString["codigo"]));
                        var userid = Convert.ToInt32(confirmacaoEmail["userid"]);
                        var codigo = confirmacaoEmail["codigo"];

                        var objPessoaFisica = PessoaFisica.LoadObject(userid);
                        if (objPessoaFisica.ValidacaoEmailUtilizarCodigo(codigo))
                        {
                            LogarAutomaticoPessoaFisica(objPessoaFisica.IdPessoaFisica);

                            if(!string.IsNullOrEmpty(Request.QueryString["editar"]))
                            {
                                //redirecionar para o miniCV
                                if (IdPessoaFisicaLogada.HasValue)
                                    Session.Add(Chave.Temporaria.Variavel1.ToString(), IdPessoaFisicaLogada.Value);

                                Redirect(Page.GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
                            }
                            else
                            {
                                ModalConfirmacaoEmail.Inicializar();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                    base.ExibirMensagem(ex.Message, TipoMensagem.Aviso);
                }

            }
            ModalConfirmacaoEmail.VisualizarVagasNoPerfil += ModalConfirmacaoEmail_VisualizarVagasNoPerfil;
        }

        void ModalConfirmacaoEmail_VisualizarVagasNoPerfil(object sender, EventArgs e)
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                base.RedirecionarCandidatoPesquisaVaga(PessoaFisica.LoadObject(IdPessoaFisicaLogada.Value));
            }
            else
            {
                Redirect(Page.GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }

    }
}
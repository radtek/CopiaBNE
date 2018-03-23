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
                            ModalConfirmacaoEmail.Inicializar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                    base.ExibirMensagem("Ocorreu um erro ao validar o seu e-mail", TipoMensagem.Aviso);
                }

            }
            ModalConfirmacaoEmail.VisualizarVagasNoPerfil += ModalConfirmacaoEmail_VisualizarVagasNoPerfil;
        }

        void ModalConfirmacaoEmail_VisualizarVagasNoPerfil(object sender, EventArgs e)
        {
            base.RedirecionarCandidatoPesquisaVaga(PessoaFisica.LoadObject(IdPessoaFisicaLogada.Value));
        }

    }
}
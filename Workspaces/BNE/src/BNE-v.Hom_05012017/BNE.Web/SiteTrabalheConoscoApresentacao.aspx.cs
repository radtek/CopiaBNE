using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.BLL;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;

namespace BNE.Web
{
    public partial class SiteTrabalheConoscoApresentacao : BasePage
    {

        #region Propriedades

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected List<int> Permissoes
        {
            get
            {
                return (List<int>)ViewState[Chave.Temporaria.Permissoes.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Permissoes.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
        }
        #endregion

        #region btnCriarAgora_Click
        protected void btnCriarAgora_Click(object sender, EventArgs e)
        {
            Response.Redirect("SiteTrabalheConoscoCriacao.aspx");
        }
        #endregion

        #region Inicializar
        private void Inicializar()
        {
            AjustarPermissoes();

            base.InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "SiteTrabalheConosco");
            lblSiteTrabalheConosco.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaApresentacaoSTC);
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, BLL.Enumeradores.CategoriaPermissao.SalaSelecionadora);

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.SalaSelecionadora.AcessarTelaSalaSelecionadora))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemErro._300034);
                    Response.Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Response.Redirect(Configuracao.UrlAcessoNegadoEmpresa);
        }
        #endregion

    }
}
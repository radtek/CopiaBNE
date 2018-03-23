using System;
using System.Collections.Generic;
using System.Configuration;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SiteTrabalheConoscoConfirmacao : BasePage
    {

        #region Propriedades

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        private List<int> Permissoes
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

        #region Eventos

        #region Page_Load
        /// <summary>
        /// Método executado quando a página é carregada
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            AjustarPermissoes();

            OrigemFilial objOrigemFilial;
            if (base.IdFilial.HasValue && OrigemFilial.CarregarPorFilial(base.IdFilial.Value, out objOrigemFilial))
            {
                var url = ConfigurationManager.AppSettings["urlSite"];

                hlkEnderecoWww.NavigateUrl = String.Format("{1}/{0}", objOrigemFilial.DescricaoDiretorio, url);
                hlkEnderecoWww.Text = String.Format("{1}/{0}", objOrigemFilial.DescricaoDiretorio, url.Replace("http://", String.Empty));
            }

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "SiteTrabalheConosco");
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
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, BLL.Enumeradores.CategoriaPermissao.SalaSelecionadora);

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.SalaSelecionadora.AcessarTelaSalaSelecionadora))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), null));
        }
        #endregion

        #region btnComeceUtilizarJa_Click
        protected void btnComeceUtilizarJa_Click(object sender, EventArgs e)
        {
            try
            {
                OrigemFilial objOrigemFilial;
                if (base.IdFilial.HasValue && OrigemFilial.CarregarPorFilial(base.IdFilial.Value, out objOrigemFilial))
                {
                    objOrigemFilial.Template.CompleteObject();
                    base.Tema.Value = objOrigemFilial.Template.NomeTemplate;
                    base.IdOrigem.Value = objOrigemFilial.Origem.IdOrigem;
                    base.IdCurriculo.Clear();
                    base.STC.Value = true;
                    Redirect("~/SiteTrabalheConoscoMenu.aspx");
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion

    }
}
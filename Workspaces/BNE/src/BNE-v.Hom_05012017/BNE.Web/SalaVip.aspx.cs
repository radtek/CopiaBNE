using System;
using System.Collections.Generic;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaVip : BasePage
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

        #region Eventos

        #region Page_Load
        /// <summary>
        /// Método executado quando a página é carregada
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "SalaVip");

            Ajax.Utility.RegisterTypeForAjax(typeof(SalaVip));
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        /// <summary>
        /// Método utilizado para para preenchimento de componentes, funções de foco e navegação
        /// </summary>
        private void Inicializar()
        {
            AjustarPermissoes();

            ucTabs.Inicializar();
            ucDados.Inicializar();

            //É preciso ajustar o topo caso o usuario tenha o perfil empresa e candidato
            PropriedadeAjustarTopoUsuarioCandidato(true);

            ExibirMenuSecaoCandidato();
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o usuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoCandidato.Value, BLL.Enumeradores.CategoriaPermissao.SalaVIP);

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.SalaVIP.AcessarTelaSalaVIP))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null));
        }
        #endregion

        protected void btnVip_Click(object sender, EventArgs e)
        {
            if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                Redirect(Page.GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.EscolhaPlano.ToString(), null));
            else
                ExibirLogin();
        }

        #endregion

    }
}

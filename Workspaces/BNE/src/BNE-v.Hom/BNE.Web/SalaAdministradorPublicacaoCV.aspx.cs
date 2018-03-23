using System;
using System.Collections.Generic;
using System.Web.UI;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.BLL;
using BNE.Web.Code.Session;
using BNE.Web.Resources;
using Resources;

namespace BNE.Web
{
    public partial class SalaAdministradorPublicacaoCV : BasePage
    {

        #region Propriedades

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera as Permissoes
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
            ucPublicacaoCV.EventVisualizacaoCurriculo += ucPublicacaoCV_EventVisualizacaoCurriculo;
            ucPublicacaoCV.EventEditarCurriculo += ucPublicacaoCV_EventEditarCurriculo;
        }
        #endregion

        #region ucPublicacaoCV_EventEditarCurriculo
        void ucPublicacaoCV_EventEditarCurriculo(int idCurriculo, List<int> listProximoCurriculo)
        {
            Session.Add(Chave.Temporaria.Variavel1.ToString(), idCurriculo);
            Session.Add(Chave.Temporaria.Variavel2.ToString(), listProximoCurriculo);

            Redirect("CadastroCurriculoCompleto.aspx");
        }
        #endregion

        #region ucPublicacaoCV_EventVisualizacaoCurriculo
        void ucPublicacaoCV_EventVisualizacaoCurriculo(int idCurriculo)
        {
            string serverName = Request.ServerVariables["HTTP_HOST"];
            base.IdCurriculoVisualizacao.Value = idCurriculo;
            Session.Add(Chave.Temporaria.Variavel4.ToString(), true);
            ScriptManager.RegisterStartupScript(this, GetType(), "AbrirPopup", string.Format("AbrirPopup('http://{0}/VisualizacaoCurriculo.aspx', 600, 800);", serverName), true);
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarPermissoes();
            ucPublicacaoCV.Inicializar(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);
            AjustarTituloTela("Publicação de Currículos");
            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "PublicacaoCV");
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, BLL.Enumeradores.CategoriaPermissao.TelaSalaAdministrador);

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.TelaSalaAdministrador.AcessarTelaSalaAdministradorPublicacaoCV))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(Configuracao.UrlAvisoAcessoNegado);
        }
        #endregion

        #endregion
    }
}
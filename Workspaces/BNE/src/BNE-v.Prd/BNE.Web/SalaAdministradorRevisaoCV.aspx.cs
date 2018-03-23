using System;
using System.Collections.Generic;
using System.Web.UI;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.BLL;
using BNE.Web.Code.Session;
using Resources;
using BNE.Web.Resources;

namespace BNE.Web
{
    public partial class SalaAdministradorRevisaoCV : BasePage
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

            ucRevisaoCV.EventVisualizacaoCurriculo += ucRevisaoCV_EventVisualizacaoCurriculo;
            ucRevisaoCV.EventEditarCurriculo += ucRevisaoCV_EventEditarCurriculo;

            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "RevisaoCV");
        }
        #endregion

        #region ucRevisaoCV_EventVisualizacaoCurriculo
        void ucRevisaoCV_EventVisualizacaoCurriculo(int idCurriculo)
        {
            string serverName = Request.ServerVariables["HTTP_HOST"];
            base.IdCurriculoVisualizacao.Value = idCurriculo;
            Session.Add(Chave.Temporaria.Variavel4.ToString(), true);
            ScriptManager.RegisterStartupScript(this, GetType(), "AbrirPopup", string.Format("AbrirPopup('http://{0}/VisualizacaoCurriculo.aspx', 600, 800);", serverName), true);
        }
        #endregion

        #region ucRevisaoCV_EventEditarCurriculo
        void ucRevisaoCV_EventEditarCurriculo(int idCurriculo, List<int> listProximoCurriculo)
        {
            Session.Add(Chave.Temporaria.Variavel1.ToString(), idCurriculo);
            Session.Add(Chave.Temporaria.Variavel2.ToString(), listProximoCurriculo);

            Redirect("CadastroCurriculoCompleto.aspx");
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarPermissoes();
            ucRevisaoCV.Inicializar();
            AjustarTituloTela("Revisão de Currículos");
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

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.TelaSalaAdministrador.AcessarTelaSalaAdministradorRevisaoCV))
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
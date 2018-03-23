using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.BLL;
using BNE.Web.Code.Session;
using Resources;
using BNE.Web.Resources;

namespace BNE.Web
{
    public partial class SalaAdministradorAuditoriaCV : BasePage
    {

        #region Eventos

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
            ucAuditoriaCV.EventVisualizacaoCurriculo += ucAuditoriaCV_EventVisualizacaoCurriculo;
            ucAuditoriaCV.EventEditarCurriculo += ucAuditoriaCV_EventEditarCurriculo;
        }

        #endregion

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

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarPermissoes();
            AjustarTituloTela("Auditoria de Currículos");
            base.InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "EdicaoCV");

            ucAuditoriaCV.Inicializar();
        }
        #endregion

        #region ucAuditoriaCV_EventVisualizacaoCurriculo
        void ucAuditoriaCV_EventVisualizacaoCurriculo(int idCurriculo)
        {
            string serverName = Request.ServerVariables["HTTP_HOST"];
            base.IdCurriculoVisualizacao.Value = idCurriculo;
            Session.Add(Chave.Temporaria.Variavel4.ToString(), true);
            ScriptManager.RegisterStartupScript(this, GetType(), "AbrirPopup", string.Format("AbrirPopup('http://{0}/VisualizacaoCurriculo.aspx', 600, 800);", serverName), true);
        }
        #endregion

        #region ucAuditoriaCV_EventEditarCurriculo
        void ucAuditoriaCV_EventEditarCurriculo(int idCurriculo, List<int> listProximoCurriculo)
        {
            Session.Add(Chave.Temporaria.Variavel1.ToString(), idCurriculo);
            Session.Add(Chave.Temporaria.Variavel2.ToString(), listProximoCurriculo);

            Redirect("CadastroCurriculoCompleto.aspx");
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

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.TelaSalaAdministrador.AcessarTelaSalaAdministradorAuditoriaCV))
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
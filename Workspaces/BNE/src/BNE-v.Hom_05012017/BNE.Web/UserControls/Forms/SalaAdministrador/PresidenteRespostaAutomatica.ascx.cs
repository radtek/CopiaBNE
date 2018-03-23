using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System.Data;
using Telerik.Web.UI;
using BNE.BLL;
using Resources;
using BNE.EL;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class PresidenteRespostaAutomatica : BaseUserControl
    {
        #region Propriedades

        #region PageIndex - PageIndex
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int PageIndex
        {
            get
            {
                if (ViewState[Chave.Temporaria.PageIndex.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.PageIndex.ToString()].ToString());
                else
                    return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
            }
        }
        #endregion

        #region IdfRespostaAutomatica
        /// <summary>
        /// Propriedade que armazena e recupera o ID do Agradecimento
        /// </summary>
        protected int IdfRespostaAutomatica
        {
            get
            {
                if (Session["IdfRespostaAutomatica"] != null)
                    return Int32.Parse(Session["IdfRespostaAutomatica"].ToString());
                else
                    return 0;
            }
            set
            {
                Session.Add("IdfRespostaAutomatica", value);
            }
        }
        #endregion

        #region Filtro
        /// <summary>
        /// Propriedade que armazena e recupera o ID do Agradecimento
        /// </summary>
        protected String Filtro
        {
            get
            {
                if (Session["FiltroRa"] != null)
                    return Session["FiltroRa"].ToString();
                else
                    return null;
            }
            set
            {
                Session.Add("FiltroRa", value);
            }
        }
        #endregion

        #endregion

        #region Eventos
        
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Panel)Parent.FindControl("pnlTipoMensagem")).Visible = true;
            ((Panel)Parent.FindControl("pnlContainerMensagens")).CssClass = "container_carta";
            if (!Page.IsPostBack)
            {
                PageIndex = 1;
                gvRespostasAutomaticas.PageSize = 6;
                CarregarGrid();
            }
            
        }
        #endregion

        #region gvRespostasAutomaticas_PageIndexChanged
        protected void gvRespostasAutomaticas_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion

        #region btnExcluirRespostaAutomatica_Click
        protected void btnExcluirRespostaAutomatica_Click(object sender, ImageClickEventArgs e)
        {
            IdfRespostaAutomatica = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
            try
            {
                RespostaAutomatica.Delete(IdfRespostaAutomatica);

                ucModalConfirmacao.PreencherCampos(MensagemAviso._23012, MensagemAviso._24023, false, "OK");
                ucModalConfirmacao.MostrarModal();

                CarregarGrid();

            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnEditarRespostaAutomatica_Click
        
        protected void btnEditarRespostaAutomatica_Click(object sender, ImageClickEventArgs e)
        {
            RespostaAutomatica objRespostaAutomatica = RespostaAutomatica.LoadObject(Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString()));
            upRespostaAutomatica.Visible = false;
            upEditarRespostaAutomatica.Visible = true;
            upNovaRespostaAutomatica.Visible = false;
            upTodasRespostaAutomatica.Visible = false;
            ucPresidenteEditarRespostaAutomatica.PreencherCampos(objRespostaAutomatica);

            upPrincipal.Update();
        }
        #endregion

        #region btnFiltrar_Click

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            Filtro = tbxFiltroBusca.Text;
            CarregarGrid();
        }

        #endregion

        #region btnNovaResposta_Click

        protected void btnNovaResposta_Click(object sender, EventArgs e)
        {
            upRespostaAutomatica.Visible = false;
            upEditarRespostaAutomatica.Visible = false;
            upNovaRespostaAutomatica.Visible = true;
            upPrincipal.Update();
        }
        #endregion

        #region btnVerTodas_Click

        protected void btnVerTodas_Click(object sender, EventArgs e)
        {
            upRespostaAutomatica.Visible = false;
            upEditarRespostaAutomatica.Visible = false;
            upNovaRespostaAutomatica.Visible = false;
            upTodasRespostaAutomatica.Visible = true;
            ucPresidenteRespAutoVisualizarTodas.CarregarGrid();
            ((Panel)Parent.FindControl("pnlTipoMensagem")).Visible = false;
            ((Panel)Parent.FindControl("pnlContainerMensagens")).CssClass = "";
            ((UpdatePanel)Parent.FindControl("upContainerMensagens")).Update();
            upPrincipal.Update();
        }
         #endregion

        #endregion

        #region Metodos

        #region CarregarGrid
        public void CarregarGrid()
        {
            int totalRegistros;
            if (String.IsNullOrEmpty(Filtro))
                UIHelper.CarregarRadGrid(gvRespostasAutomaticas, RespostaAutomatica.ListarRespostasAutomaticasDT(PageIndex, gvRespostasAutomaticas.PageSize, out totalRegistros), totalRegistros);
            else
            {
                UIHelper.CarregarRadGrid(gvRespostasAutomaticas, RespostaAutomatica.ListarRespostasAutomaticasDT(Filtro, PageIndex, gvRespostasAutomaticas.PageSize, out totalRegistros), totalRegistros);
            }
            upRespostaAutomatica.Update();
        }

        #endregion

      
        #endregion

       
    }
}
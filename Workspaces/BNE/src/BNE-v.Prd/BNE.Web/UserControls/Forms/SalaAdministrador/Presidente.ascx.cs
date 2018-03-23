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
using BNE.Web;
using Resources;
using BNE.EL;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class Presidente : BaseUserControl
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

        #region IdfAgradecimento
        /// <summary>
        /// Propriedade que armazena e recupera o ID do Agradecimento
        /// </summary>
        protected int IdfAgradecimento
        {
            get
            {
                if (Session["IdfAgradecimento"] != null)
                    return Int32.Parse(Session["IdfAgradecimento"].ToString());
                else
                    return 0;
            }
            set
            {
                Session.Add("IdfAgradecimento", value);
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
                if (Session["FiltroA"] != null)
                    return Session["FiltroA"].ToString();
                else
                    return null;
            }
            set
            {
                Session.Add("FiltroA", value);
            }
        }
        #endregion

        #endregion

        #region Eventos
        
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PageIndex = 1;
                gvAgradecimentos.PageSize = 6;
                CarregarGrid();
            }
           
        }

        #endregion

        #region gvAgradecimentos_PageIndexChanged
        protected void gvAgradecimentos_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion


        #region btnEditarAgradecimento_Click
        
        protected void btnEditarAgradecimento_Click(object sender, ImageClickEventArgs e)
        {

            Agradecimento objAgradecimento = Agradecimento.LoadObject(Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString()));
            upAgradecimento.Visible = false;
            upEditarAgradecimento.Visible = true;
            UpNovoAgradecimento.Visible = false;
            ucPresidenteEditarAgradecimento.PreencherCampos(objAgradecimento);

            upPrincipal.Update();

        }

        #endregion

        #region btnExcluirAgradecimento_Click
        protected void btnExcluirAgradecimento_Click(object sender, ImageClickEventArgs e)
        {
            IdfAgradecimento = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
            try
            {
                Agradecimento.Delete(IdfAgradecimento);

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

        #region btnNovoAgradecimento_Click

        protected void btnNovoAgradecimento_Click(object sender, EventArgs e)
        {
            upAgradecimento.Visible = false;
            upEditarAgradecimento.Visible = false;
            UpNovoAgradecimento.Visible = true;
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

        #endregion

        #region Metodos

        #region CarregarGrid
        public void CarregarGrid()
        {
            int totalRegistros;
            if (String.IsNullOrEmpty(Filtro))
                UIHelper.CarregarRadGrid(gvAgradecimentos, Agradecimento.ListarAgradecimentosDT(PageIndex, gvAgradecimentos.PageSize, out totalRegistros), totalRegistros);
            else
            {
                UIHelper.CarregarRadGrid(gvAgradecimentos, Agradecimento.ListarAgradecimentosDT(Filtro, PageIndex, gvAgradecimentos.PageSize, out totalRegistros), totalRegistros);
            }
            upAgradecimento.Update();
        }

        #endregion

      

        #endregion


    }
}
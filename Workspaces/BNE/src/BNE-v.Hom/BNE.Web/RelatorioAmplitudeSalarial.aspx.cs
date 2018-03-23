using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using BNE.Web.Code;
using BNE.EL;
using Resources;
using BNE.Web.Code.Enumeradores;
using BNE.BLL;

namespace BNE.Web
{
    public partial class RelatorioAmplitudeSalarial : BasePage
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

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                Inicializar();
            base.InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "RelatorioAmplitudeSalarial");
        }
        #endregion

        public void Inicializar()
        {
            UIHelper.CarregarRadComboBox(rcbCategoria, FuncaoCategoria.Listar(), "Idf_Funcao_Categoria", "Des_Funcao_Categoria", new RadComboBoxItem("Qualquer", "0"));
            CarregarGrid();
        }

        #region btnPesquisar_Click
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            CarregarGrid();
        }
        #endregion

        #region gvAmplitudeSalarial_SortCommand
        protected void gvAmplitudeSalarial_SortCommand(object source, GridSortCommandEventArgs e)
        {
            CarregarGrid();
        }
        #endregion

        #region gvAmplitudeSalarial_ItemCommand
        protected void gvAmplitudeSalarial_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EditarAmplitude"))
                {
                    e.Item.Edit = true;
                    e.Item.OwnerTableView.EditMode = GridEditMode.InPlace;
                    
                    CarregarGrid();
                }
                if (e.CommandName.Equals("Cancel"))
                {
                    e.Item.Edit = false;
                    CarregarGrid();
                }
                if (e.CommandName.Equals("SalvarAmplitude"))
                {
                    Funcao objFuncao = new Funcao();
                    
                    int? idfAmplitude = null;
                    GridDataItem item = (GridDataItem)e.Item; 
                    if(!string.IsNullOrEmpty(item.GetDataKeyValue("Idf_Amplitude_Salarial").ToString()))
                        idfAmplitude = Convert.ToInt32(item.GetDataKeyValue("Idf_Amplitude_Salarial").ToString());

                    decimal? limiteInterior = null;
                    decimal? limiteSuperior = null;
                    TextBox txtLimiteInferior = (TextBox) e.Item.FindControl("txtLimiteInferior");
                    TextBox txtLimiteSuperior = (TextBox) e.Item.FindControl("txtLimiteSuperior");
                    if(!string.IsNullOrEmpty(txtLimiteInferior.Text))
                        limiteInterior = Convert.ToDecimal(txtLimiteInferior.Text);
                    if(!string.IsNullOrEmpty(txtLimiteSuperior.Text))
                        limiteSuperior=Convert.ToDecimal(txtLimiteSuperior.Text);

                    Label lblIdFuncao = (Label)e.Item.FindControl("lblIdFuncao");

                    objFuncao.SalvarAmplitude(idfAmplitude, Convert.ToInt32(lblIdFuncao.Text), limiteInterior, limiteSuperior);
                    
                    e.Item.Edit = false;
                    CarregarGrid();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion


        #region gvAmplitudeSalarial_PageIndexChanged
        protected void gvAmplitudeSalarial_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion

        public void CarregarGrid()
        {

            int totalRegistros = 200;

            gvAmplitudeSalarial.PageSize = totalRegistros;

            int? idfCategoria = Convert.ToInt32(rcbCategoria.SelectedValue);

            Funcao objFuncao;
            if (!Funcao.CarregarPorDescricao(txtFuncao.Text, out objFuncao))
                objFuncao = null;

            if (objFuncao != null)
                UIHelper.CarregarRadGrid(gvAmplitudeSalarial, Funcao.CarregarAmplitudesPorFuncao(objFuncao.IdFuncao, idfCategoria), totalRegistros);
            else
                UIHelper.CarregarRadGrid(gvAmplitudeSalarial, Funcao.CarregarAmplitudesPorFuncao(null, idfCategoria), totalRegistros);
            
            

        }
    }
}
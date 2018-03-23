using System;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Telerik.Web.UI;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class VisualizadoCV : BaseUserControl
    {

        #region Propriedades

        #region IdFilial - Variavel 6
        /// <summary>
        /// Propriedade que armazena e recupera o IdEmpresa
        /// </summary>
        public int? IdFilial
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel6.ToString()].ToString());
                return null;
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel6.ToString()] = value;
            }
        }
        #endregion

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvVisualizadoCV.PageSize = 6;
                CarregarGrid();
            }
        }
        #endregion
        
        #region Events

        #region btnFiltrar_Click
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            gvVisualizadoCV.CurrentPageIndex = 0;
            this.CarregarGrid();
        }
        #endregion 
        
        #region gvVisualizadoCV_PageIndexChanged
        protected void gvVisualizadoCV_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gvVisualizadoCV.CurrentPageIndex = e.NewPageIndex;
            CarregarGrid();
        }
        #endregion

        #endregion 

        #region Methods

        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros = 8;
            UIHelper.CarregarRadGrid(gvVisualizadoCV, CurriculoVisualizacao.CarregarSalaAdministradora(this.IdFilial.Value, tbxFiltroBusca.Text, gvVisualizadoCV.CurrentPageIndex, gvVisualizadoCV.PageSize, out totalRegistros), totalRegistros);
        }
        #endregion
        
        #region RetornarFuncao
        protected string RetornarFuncao(string funcao)
        {
            if (string.IsNullOrEmpty(funcao))
                return string.Empty;

            return funcao.Substring(0, funcao.Length - 1);
        }
        #endregion

        #endregion

    }
}